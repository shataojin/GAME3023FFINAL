using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    public string Name;
    public string description;
    public string category; // 用于区分 "HP" 或 "MANA" 类型
    public Sprite icon;
    public int value; // 回复值

    public void Use(CharacterHP characterHP, CharacterMANA characterMANA)
    {
        if (category == "HP" && characterHP != null)
        {
            characterHP.currentHealth = Mathf.Min(characterHP.health, characterHP.currentHealth + value);
            Debug.Log($"{Name} used. HP restored by {value}. Current HP: {characterHP.currentHealth}");
        }
        else if (category == "MANA" && characterMANA != null)
        {
            characterMANA.currentmana = Mathf.Min(characterMANA.mana, characterMANA.currentmana + value);
            Debug.Log($"{Name} used. MANA restored by {value}. Current MANA: {characterMANA.currentmana}");
        }
        else
        {
            Debug.LogWarning($"Item {Name} has an unknown category or missing target.");
        }
    }
}
