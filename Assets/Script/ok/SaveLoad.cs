using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    private string saveFilePath;
    private bool isNewGame = true;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "gamesave.txt");

        if (!File.Exists(saveFilePath))
        {
            Debug.Log("Save file not found. Creating a new one.");
            CreateNewSaveFile();
        }
        else
        {
            Debug.Log("Save file found. Checking newgame status...");

            string[] fileLines = File.ReadAllLines(saveFilePath);
            if (fileLines.Length > 0 && bool.TryParse(fileLines[0], out bool newGameStatus))
            {
                isNewGame = newGameStatus;
                Debug.Log($"NEWGAME status loaded: {isNewGame}");
            }
            else
            {
                Debug.LogWarning("Invalid NEWGAME status in save file. Defaulting to true.");
                isNewGame = true;
            }

            if (!isNewGame)
            {
                LoadGameState();
            }
        }
    }

    public void SetNewGame(bool newGameStatus)
    {
        isNewGame = newGameStatus;

        string[] fileLines = File.Exists(saveFilePath) ? File.ReadAllLines(saveFilePath) : new string[0];
        using (StreamWriter writer = new StreamWriter(saveFilePath))
        {
            writer.WriteLine(isNewGame);

            for (int i = 1; i < fileLines.Length; i++)
            {
                writer.WriteLine(fileLines[i]);
            }
        }

        Debug.Log($"NEWGAME status updated to: {isNewGame}");
    }

    public void SaveGameState()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found! Make sure the player has the 'Player' tag.");
            return;
        }

        Vector2 playerPosition = player.transform.position;

        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        List<string> itemDataList = new List<string>();

        foreach (GameObject item in items)
        {
            Vector2 itemPosition = item.transform.position;
            PickUpItems pickUpScript = item.GetComponent<PickUpItems>();

            if (pickUpScript != null)
            {
                bool isPickedUp = pickUpScript.itemsAlreadyPickUp;
                itemDataList.Add($"{itemPosition.x},{itemPosition.y},{isPickedUp}");
            }
        }

        if (InventoryManager.Instance != null)
        {
            foreach (var collectedItem in InventoryManager.Instance.collectedItems)
            {
                itemDataList.Add($"CollectedItem:{collectedItem.Name},{collectedItem.description},{collectedItem.value}");
            }
        }

        using (StreamWriter writer = new StreamWriter(saveFilePath))
        {
            writer.WriteLine(isNewGame);
            writer.WriteLine($"{playerPosition.x},{playerPosition.y}");

            foreach (string itemData in itemDataList)
            {
                writer.WriteLine(itemData);
            }
        }

        Debug.Log("Game state saved, including collected items.");
    }

    private void CreateNewSaveFile()
    {
        using (StreamWriter writer = new StreamWriter(saveFilePath))
        {
            writer.WriteLine(true);
        }

        Debug.Log("New save file created with NEWGAME=true.");
    }

    public void LoadGameState()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("Save file not found.");
            return;
        }

        string[] fileLines = File.ReadAllLines(saveFilePath);

        if (fileLines.Length < 2)
        {
            Debug.LogWarning("Save file is empty or missing required data.");
            return;
        }

        LoadPlayerPosition(fileLines[1]);

        for (int i = 2; i < fileLines.Length; i++)
        {
            if (fileLines[i].StartsWith("CollectedItem:"))
            {
                Debug.Log($"Skipping collected item data: {fileLines[i]}");
            }
            else
            {
                LoadItemData(fileLines[i]);
            }
        }

        Debug.Log("Game state loaded.");
    }

    private void LoadPlayerPosition(string line)
    {
        string[] playerPositionData = line.Split(',');
        if (playerPositionData.Length == 2 &&
            float.TryParse(playerPositionData[0], out float playerX) &&
            float.TryParse(playerPositionData[1], out float playerY))
        {
            Vector2 playerPosition = new Vector2(playerX, playerY);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = playerPosition;
                Debug.Log($"Player position loaded: {playerPosition}");
            }
        }
        else
        {
            Debug.LogWarning("Invalid player position data in save file.");
        }
    }

    private void LoadItemData(string line)
    {
        string[] itemData = line.Split(',');

        if (itemData.Length == 3 &&
            float.TryParse(itemData[0], out float itemX) &&
            float.TryParse(itemData[1], out float itemY) &&
            bool.TryParse(itemData[2], out bool isPickedUp))
        {
            Vector2 itemPosition = new Vector2(itemX, itemY);

            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            GameObject closestItem = null;
            float minDistance = float.MaxValue;

            foreach (GameObject item in items)
            {
                float distance = Vector2.Distance(item.transform.position, itemPosition);
                if (distance < minDistance)
                {
                    closestItem = item;
                    minDistance = distance;
                }
            }

            if (closestItem != null)
            {
                closestItem.transform.position = itemPosition;

                PickUpItems pickUpScript = closestItem.GetComponent<PickUpItems>();
                if (pickUpScript != null)
                {
                    pickUpScript.itemsAlreadyPickUp = isPickedUp;
                }

                Debug.Log($"Item loaded at {itemPosition} with pickedUp status: {isPickedUp}");
            }
        }
        else
        {
            Debug.LogWarning("Invalid item data in save file.");
        }
    }

    public void OnNewGameButtonPressed()
    {
        SetNewGame(true);
        Debug.Log("New Game button pressed. NEWGAME=true.");
    }

    public void OnContinueButtonPressed()
    {
        SetNewGame(false);
        Debug.Log("Continue button pressed. NEWGAME=false.");
    }
}
