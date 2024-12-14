using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public string abilityName;//abilityName
    public string description;//shows the info about ability
    public int damage;// dmg to character
    public int mana;// cost of mana

    public void Activate(CharacterHP targe,CharacterMANA cost)//show info list
    {
        targe.TakeDamage(damage);
        cost.CostMana(mana);
        Debug.Log($"{abilityName} used, causing {damage} damage and costing {mana} mana.");
    }
}

public class AbilityList : MonoBehaviour
{
    public List<Ability> playerAbilities = new List<Ability>();//build list of Ability current have 


    private void Awake()
    {
        // Initial abilities
        playerAbilities.Add(new Ability() { abilityName = "test use", description = "aasdasd", damage = 100, mana = 50 });
        playerAbilities.Add(new Ability() { abilityName = "Slash", description = "Cool Slash", damage = 15 , mana = 8});
        playerAbilities.Add(new Ability() { abilityName = "Fireball", description = " a big Fireball", damage = 20 , mana = 12});

    }

    public void UseAbility(int index, CharacterHP target, CharacterMANA cost)//cast to do dmg 
    {
        if (index >= 0 && index < playerAbilities.Count)
        {
            playerAbilities[index].Activate(target,cost);
        }
    }

    public void UnlockAbility()// the skill which could add to list when unlock
    {
        playerAbilities.Add(new Ability() { abilityName = "Waterbending", description = "A water impact", damage = 50, mana = 25 });
    }

   
    //public void UnlockPredefinedAbilities()
    //{
    //    UnlockAbility("雷击", "一召唤的闪电", 20);
    //    UnlockAbility("雷电斩", "一个雷电斩击", 10);
    //    UnlockAbility("御水术", "一个水流冲击", 5);
    //}
}
