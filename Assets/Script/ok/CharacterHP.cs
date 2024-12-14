using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

// this script was made by myself, It stores the function  which affects character hp
// taojin sha
//2024.10.18
public class CharacterHP : MonoBehaviour
{
    public string characterName;
    public int health;
    public int currentHealth;


    private void Start()
    {
       
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
           
        }
    }

    public void ResetHealth()
    {
        currentHealth = health;
    }

}
