using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    public GameObject buttonPrefab; // 按钮预制件
    public Transform buttonParent;  // 按钮的父对象（通常是 Scroll View 的 Content 对象）
    public CharacterHP characterHP;     // 玩家 HP 引用
    public CharacterMANA characterMANA; // 玩家 MANA 引用

    private void Start()
    {
        GenerateItemButtons(); // 在游戏开始时生成物品按钮
    }

    // 获取玩家持有的物品并生成按钮
    private void GenerateItemButtons()
    {
        // 获取玩家当前持有的物品列表
        List<Item> playerItems = InventoryManager.Instance?.collectedItems;

        if (playerItems == null || playerItems.Count == 0)
        {
            Debug.LogWarning("玩家没有持有任何物品。");
            return;
        }

        // 清空之前的按钮
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        // 为每个物品生成一个按钮
        foreach (Item item in playerItems)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent); // 添加到 Scroll View 的 Content 对象中
            Button buttonComponent = newButton.GetComponent<Button>();
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null)
            {
                buttonText.text = item.Name; // 设置按钮文字为物品名称
            }
            else
            {
                Debug.LogWarning("按钮预制件中缺少 TextMeshProUGUI 组件。");
            }

            if (buttonComponent != null)
            {
                // 捕获局部变量，避免闭包问题
                Item capturedItem = item;
                buttonComponent.onClick.AddListener(() => OnItemButtonClicked(capturedItem));
            }
            else
            {
                Debug.LogWarning("按钮预制件中缺少 Button 组件。");
            }
        }
    }

    // 当玩家点击物品按钮时触发
    private void OnItemButtonClicked(Item item)
    {
        if (item != null)
        {
            item.Use(characterHP, characterMANA); // 使用物品
            InventoryManager.Instance?.collectedItems.Remove(item); // 从物品列表中移除
            GenerateItemButtons(); // 重新生成按钮，更新显示
        }
        else
        {
            Debug.LogWarning("要使用的物品为空。");
        }
    }
}
