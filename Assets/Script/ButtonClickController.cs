using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using TMPro;

public class ButtonClickController : MonoBehaviour
{
  
    private battleSystem BattleSystem;
    public CharacterHP enemy;
    public CharacterMANA playerMANA;
    public AbilityList abilityList;
    public GameObject textGameObject;
    public List<Button> abilityButtons = new List<Button>();
    public Ability ability;
    public bool AbilityButtonShows = false;
    //public bool firstWin = false;
    //public bool PlayerTurn = true;
    //public bool addedskill=false;
    public GameObject BagList;
    public Button abilityButton1;
    public Button abilityButton2;
    public Button abilityButton3;
    public Button abilityButton4;

    private void Start()
    {

        BattleSystem = FindObjectOfType<battleSystem>();
        if (BattleSystem == null)
        {
            Debug.Log("cant find BattleSystem go fix it!!!");
        }
        BattleSystem.FightOver = false; // in case something affect the fight screen set default to false
        BattleSystem.PlayerTurn = true; // in case something affect the fight screen set default to true

        if (BagList != null)
        {
            BagList.gameObject.SetActive(false); // 确保背包在初始状态下隐藏
        }
        //PlayerTurn = true;
        //if(firstWin == true && addedskill==false)
        //{
        //    abilityList.UnlockAbility();
        //    addedskill=true;
        //}
    }

    //ability works//

    private void SetupAbilityButtons()// for set each Ability to the button
    {
        if (abilityList == null || abilityList.playerAbilities.Count == 0) return;
        AssignAbilityToButton(abilityButton1, 0);
        AssignAbilityToButton(abilityButton2, 1);
        AssignAbilityToButton(abilityButton3, 2);
        AssignAbilityToButton(abilityButton4, 3);
    }


    private void AssignAbilityToButton(Button button, int abilityIndex) //for active the button which have Ability 
    {
        if (abilityIndex < abilityList.playerAbilities.Count)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = abilityList.playerAbilities[abilityIndex].abilityName+ abilityList.playerAbilities[abilityIndex].mana;
            }
            button.gameObject.SetActive(true);
            button.onClick.RemoveAllListeners(); 
            button.onClick.AddListener(() => OnAbilityButtonClicked(abilityIndex));

        }
        else
        {
            button.gameObject.SetActive(false);
        }
    }


    public void OnAbilityButtonClicked(int index)//for one of button being clicked , trigger damage and mana .then hide all button
    {
        var selectedAbility = abilityList.playerAbilities[index];

        if (enemy != null && playerMANA.currentmana>= selectedAbility.mana)
        {
            abilityList.UseAbility(index, enemy, playerMANA);
            BattleSystem.PlayerTurn = false; // change to enemy turn
            Debug.Log($"Ability {abilityList.playerAbilities[index].abilityName} was used.");
            Debug.Log($"Remaining HP: {enemy.currentHealth}");
            Debug.Log($"Remaining MANA: {playerMANA.currentmana}");
            abilityButton1.gameObject.SetActive(false);
            abilityButton2.gameObject.SetActive(false);
            abilityButton3.gameObject.SetActive(false);
            abilityButton4.gameObject.SetActive(false);
            textGameObject.SetActive(true);
        }
        else
        {
            Debug.Log("you have not enough MANA!!!!");
        }
    }

    public void OnAbilityButtonClicked()// for the button when you click shows the ability button
    {
        BagList.gameObject.SetActive(false);
        if (!BattleSystem.PlayerTurn)
        {
            Debug.Log("not ur turn");
            return;
        }

        Debug.Log("OnAbilityButtonClicked");

        if(AbilityButtonShows==false)
        {
            textGameObject.SetActive(false);
            SetupAbilityButtons();
            AbilityButtonShows = true;
        }
        else
        {
            AbilityButtonShows = false;
            textGameObject.SetActive(true);
            abilityButton1.gameObject.SetActive(false);
            abilityButton2.gameObject.SetActive(false);
            abilityButton3.gameObject.SetActive(false);
            abilityButton4.gameObject.SetActive(false);
        }
       
    }

    //attack works//
    public void OnAttackButtonClicked()
    {
        abilityButton1.gameObject.SetActive(false);
        abilityButton2.gameObject.SetActive(false);
        abilityButton3.gameObject.SetActive(false);
        abilityButton4.gameObject.SetActive(false);
        BagList.gameObject.SetActive(false);
        textGameObject.SetActive(true);
        Debug.Log("OnAttackButtonClicked");
        if (!BattleSystem.PlayerTurn)
        {
            Debug.Log("not ur turn");
            return;
        }
        else
        {
            BattleSystem.PlayerTurn = false; // change to enemy turn
        }

        if (enemy != null)
        {
            int damage = 10;
            enemy.TakeDamage(damage);
            Debug.Log($"Enemy takes {damage} damage. Remaining HP: {enemy.currentHealth}");

        }
    }

    //run works//
    public void OnRunButtonClicked()
    {
        abilityButton1.gameObject.SetActive(false);
        abilityButton2.gameObject.SetActive(false);
        abilityButton3.gameObject.SetActive(false);
        abilityButton4.gameObject.SetActive(false);
        BagList.gameObject.SetActive(false);
        textGameObject.SetActive(true);

        Debug.Log("OnRunButtonClicked");
       
        if (!BattleSystem.PlayerTurn)
        {
            Debug.Log("not ur turn");
            return;
        }
        else
        {
            Debug.Log("palyer run away.");
            BattleSystem.FightOver = true;
        }
    }

    public void OnBagButtonClicked()
{
        abilityButton1.gameObject.SetActive(false);
        abilityButton2.gameObject.SetActive(false);
        abilityButton3.gameObject.SetActive(false);
        abilityButton4.gameObject.SetActive(false);
        // 切换 BagList 的激活状态
        BagList.gameObject.SetActive(!BagList.gameObject.activeSelf);

    // 切换 textGameObject 的激活状态
    textGameObject.gameObject.SetActive(!textGameObject.gameObject.activeSelf);
}




}
