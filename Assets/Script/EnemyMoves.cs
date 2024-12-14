using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyMove : MonoBehaviour
{
    private battleSystem BattleSystem;
    public CharacterHP PlayerHP;
    public CharacterHP EnemyHP;
    public AbilityList abilityList;
    public CharacterMANA enemyMANA;
    public bool RandomNumberSet = false;
    private int enemyAction;
    private int blockHP;
    private bool block=false;
    private void Start()
    {
        BattleSystem = FindObjectOfType<battleSystem>();
    }

    private void Update()
    {
        if (BattleSystem.PlayerTurn == false)
        {
            if (!RandomNumberSet)
            {
                enemyAction = UnityEngine.Random.Range(1, 5);//could use floatto random selection based on percentage
                //1: Melee
                //2: Ability
                //3: Defense
                //4: Flee
                RandomNumberSet = true;
                Debug.Log("enemyAction:" + enemyAction);
                switch (enemyAction)//call action with the number
                {
                    case 1:
                        PerformMeleeAttack();
                        break;
                    case 2:
                        UseAbility();
                        break;
                    case 3:
                        Defend();
                        break;
                    case 4:
                        Flee();
                        break;
                }
            }
            BattleSystem.PlayerTurn = true;
            RandomNumberSet = false;
        }

        if (EnemyHP.currentHealth <= 0)//for enemy died
        {
            gameObject.SetActive(false);
        }

        if (BattleSystem.RestEnemy == true)//for reset enemy hp
        {
            EnemyHP.currentHealth = EnemyHP.health;
            enemyMANA.currentmana = enemyMANA.mana;
            BattleSystem.RestEnemy = false;
        }

        
    }


    private void PerformMeleeAttack()//do melee dmg
    {
        Debug.Log("MeleeAttack!");
        if (PlayerHP != null)
        {
            int damage = 5;
            PlayerHP.TakeDamage(damage);
            Debug.Log($"Player takes {damage} damage. Remaining HP: {PlayerHP.currentHealth}");
        }

    }

    private void UseAbility()//do Ability dmg
    {
        Debug.Log("UseAbility!");

        bool abilityUsed = false;
        int attempts = 0; 

        while (!abilityUsed && attempts < abilityList.playerAbilities.Count)
        {
            int choose = UnityEngine.Random.Range(attempts, abilityList.playerAbilities.Count); 
            var selectedAbility = abilityList.playerAbilities[choose];

            if (PlayerHP != null && enemyMANA.currentmana >= selectedAbility.mana)
            {
                abilityList.UseAbility(choose, PlayerHP, enemyMANA);
                Debug.Log($"Ability {selectedAbility.abilityName} was used.");
                Debug.Log($"Player HP: {PlayerHP.currentHealth}");
                Debug.Log($"Enemy MANA: {enemyMANA.currentmana}");
                abilityUsed = true;
            }
            else
            {
                Debug.Log("Enemy does not have enough MANA to cast this ability, choosing another.");
                attempts++;
            }
        }

        if (!abilityUsed)
        {
            Debug.Log("Enemy has insufficient MANA for any ability.");
            PerformMeleeAttack();
        }
    }

        private void Defend()//do block dmg
    {
        int chance = UnityEngine.Random.Range(1, 11);
        Debug.Log("Defend!");
        int blockHP = EnemyHP.currentHealth;
       
            if (chance >= 7)
            {
                Debug.Log("Block DMG Success!");
            block = true;
            }
            else
            {
                Debug.Log("Block DMG Fail!");
            block = false;
        }
      

    }

    private void Flee()// low hp , ran away
    {
        Debug.Log("Flee!");
        int chance = UnityEngine.Random.Range(1, 11);
        if (EnemyHP.currentHealth <= EnemyHP.health * 0.15f)
        {
            if (chance >= 7)
            {
                BattleSystem.FightOver = true;
                Debug.Log("Escape Success!");
            }
            else
            {
                Debug.Log("Escape Fail!");
            }
        }
        else
        {
            Debug.Log("NO Flee!");
            enemyAction = UnityEngine.Random.Range(1, 4);
            Debug.Log("New Action after NO Flee: " + enemyAction);

            switch (enemyAction)
            {
                case 1:
                    PerformMeleeAttack();
                    break;
                case 2:
                    UseAbility();
                    break;
                case 3:
                    Defend();
                    break;
            }
        }
    }
}