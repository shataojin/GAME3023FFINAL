using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<Item> collectedItems = new List<Item>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Item item)
    {
        if (!collectedItems.Contains(item))
        {
            collectedItems.Add(item);
            Debug.Log($"Added item: {item.Name} to inventory.");
        }
        else
        {
            Debug.Log($"Item: {item.Name} is already in inventory.");
        }
    }

    public bool IsItemInInventory(Item item)
    {
        return collectedItems.Contains(item);
    }

    public void ShowInventory()
    {
        Debug.Log("Current Inventory:");
        foreach (var item in collectedItems)
        {
            Debug.Log($"- {item.Name}: {item.description}");
        }
    }
}
