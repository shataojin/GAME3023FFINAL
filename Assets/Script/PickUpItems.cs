using UnityEngine;
using System.Collections;

public class PickUpItems : MonoBehaviour
{
    public Item itemData;
    public GameObject interactionPrompt;
    public GameObject itemDescriptionPopup;
    public float popupDuration = 2f;

    private bool playerInRange = false;
    private bool isPopupActive = false;
    public bool itemsAlreadyPickUp = false;
    public Color pickedUpColor = Color.gray;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("No SpriteRenderer found on the object.");
        }

        interactionPrompt.SetActive(false);
        itemDescriptionPopup.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && !itemsAlreadyPickUp)
        {
            PickUpItem();
        }

        if (itemsAlreadyPickUp)
        {
            ChangeColor(pickedUpColor);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !itemsAlreadyPickUp)
        {
            playerInRange = true;
            interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !itemsAlreadyPickUp)
        {
            playerInRange = false;
            interactionPrompt.SetActive(false);
        }
    }

    private void PickUpItem()
    {
        itemsAlreadyPickUp = true;
        interactionPrompt.SetActive(false);
        ShowItemDescription();

        // Add the item to InventoryManager
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemData);
        }
    }

    private void ShowItemDescription()
    {
        itemDescriptionPopup.SetActive(true);

        var descriptionText = itemDescriptionPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (descriptionText != null)
        {
            descriptionText.text = $"Picked up: {itemData.Name}\n{itemData.description}";
        }

        isPopupActive = true;
        StartCoroutine(HideItemDescriptionAfterDelay());
    }

    private IEnumerator HideItemDescriptionAfterDelay()
    {
        yield return new WaitForSeconds(popupDuration);
        if (isPopupActive)
        {
            itemDescriptionPopup.SetActive(false);
            isPopupActive = false;
        }
    }

    private void ChangeColor(Color newColor)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = newColor;
        }
    }
}
