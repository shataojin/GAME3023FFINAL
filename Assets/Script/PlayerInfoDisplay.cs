using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // 引入 TextMeshPro 的命名空间

public class PlayerInfoDisplay : MonoBehaviour
{
    public GameObject infoPanel;
    public GameObject itemButtonPrefab; // 用于生成物品按钮
    public Transform contentTransform;

    public Slider healthSlider; // 血量 Slider
    public Slider manaSlider;   // 魔法值 Slider

    public CharacterHP characterHP;
    public CharacterMANA characterMANA;

    private bool isPanelVisible = false;

    private void Start()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false); // 初始隐藏面板
        }

        // 初始化血量和魔法值滑条
        if (characterHP != null && healthSlider != null)
        {
            healthSlider.maxValue = characterHP.health;
            healthSlider.value = characterHP.currentHealth;
        }

        if (characterMANA != null && manaSlider != null)
        {
            manaSlider.maxValue = characterMANA.mana;
            manaSlider.value = characterMANA.currentmana;
        }

        UpdateScrollViewContent(); // 初始化物品信息
    }

    private void Update()
    {
        // 实时更新血量和魔法值滑条
        UpdateSliders();

        if (Input.GetKeyDown(KeyCode.I))
        {
            TogglePlayerInfoPanel();
        }
    }

    private void TogglePlayerInfoPanel()
    {
        isPanelVisible = !isPanelVisible;

        if (infoPanel != null)
        {
            infoPanel.SetActive(isPanelVisible);

            if (isPanelVisible)
            {
                UpdateScrollViewContent();
            }
        }
    }

    private void UpdateScrollViewContent()
    {
        // 清空之前的内容
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        // 从 InventoryManager 获取物品列表
        var collectedItems = InventoryManager.Instance?.collectedItems;
        if (collectedItems != null)
        {
            foreach (var item in collectedItems)
            {
                GameObject newItemButton = Instantiate(itemButtonPrefab, contentTransform);
                Button itemButton = newItemButton.GetComponent<Button>();
                TextMeshProUGUI itemText = newItemButton.GetComponentInChildren<TextMeshProUGUI>();

                if (itemText != null)
                {
                    itemText.text = item.Name; // 设置按钮上的文字为物品名称
                }
                else
                {
                    Debug.LogWarning("Button's TextMeshProUGUI component not found.");
                }

                if (itemButton != null)
                {
                    itemButton.onClick.AddListener(() => OnItemUse(item)); // 点击按钮使用物品
                }
            }
        }
        else
        {
            Debug.LogWarning("InventoryManager.Instance or collectedItems is null.");
        }
    }

    private void OnItemUse(Item item)
    {
        if (item != null)
        {
            item.Use(characterHP, characterMANA); // 使用物品
            InventoryManager.Instance?.collectedItems.Remove(item); // 从物品列表移除
            UpdateScrollViewContent(); // 更新物品显示内容
        }
    }

    private void UpdateSliders()
    {
        if (characterHP != null && healthSlider != null)
        {
            healthSlider.value = characterHP.currentHealth; // 实时更新 HP
        }

        if (characterMANA != null && manaSlider != null)
        {
            manaSlider.value = characterMANA.currentmana; // 实时更新 MANA
        }
    }
}
