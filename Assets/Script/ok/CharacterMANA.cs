using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMANA : MonoBehaviour
{
    public string characterName;
    public int mana;
    public int currentmana;


    private void Start()
    {
       
    }

    public void CostMana(int mana)
    {
        currentmana -= mana;
        if (currentmana <= 0)
        {
            currentmana = 0;

        }
    }

    public void ResetMana()
    {
        currentmana = mana;
    }
}
