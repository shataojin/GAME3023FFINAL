using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using Unity.VisualScripting;
using static UnityEngine.EventSystems.EventTrigger;

// inspired by Joss Code about the event called function. This script is to trigger the battle screen
//( will move some of battle codes to this script after test the function works)
//taojin sha
// 2024/10/18
public class battleSystem : MonoBehaviour
{
    public event Action OnBattleOver;
    public bool FightOver = false;
    public bool PlayerTurn = true;
    public bool RestEnemy = false;
    public CharacterHP playerHP;
    public CharacterMANA playerMANA;
    public GameObject enemyPrefab;
    public Transform spawnPoints;

   AudioManager audioManager;

    public void Awake()
    {
        FightOver = false;//for ez trigger the OnBattleOver action in other script
        PlayerTurn = true;// for test who turn now

        if (playerHP == null)
        {
            playerHP = FindObjectOfType<CharacterHP>();
        }
        if (playerMANA == null)
        {
            playerMANA = FindObjectOfType<CharacterMANA>();
        }

        audioManager = FindObjectOfType<AudioManager>();

       
    }

 

    public void NeedUpdate()
    {
        Debug.Log("战斗触发");
        Debug.Log("PlayerTurn:"+ PlayerTurn);
       
        if (FightOver==true)// for player run out from fight
        {
            RestAll();
        }
        else if(AreAllEnemiesDefeated() && RestEnemy == false) // for player win the fight
        {
            Debug.Log("你赢了");
            RestAll();

        }
        else if(playerHP.currentHealth <=0) //for player loss all hp
        {
            Debug.Log("你输了");
            RestAll();
        }

      

    }

    private bool AreAllEnemiesDefeated()// for test player kill all the enemy or not
    {
        
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        return allEnemies.Length == 0;
    }
    private void RestAll()// for reset all the used infor for next fight
    {
       
        FightOver = false;
        enemyPrefab.SetActive(true);
        RestEnemy = true;
        OnBattleOver();
    }
}
