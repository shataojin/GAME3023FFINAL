using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightPlayeStateUI : MonoBehaviour
{
    public Slider healthSlider;
    public Slider manaSlider;

    public CharacterHP characterHP;
    public CharacterMANA characterMana;

    private void Start()
    {
        // 初始化滑块最大值
        healthSlider.maxValue = characterHP.health;
        manaSlider.maxValue = characterMana.mana;

        // 初始化滑块当前值
        healthSlider.value = characterHP.currentHealth;
        manaSlider.value = characterMana.currentmana;
    }

    private void Update()
    {
        // 实时更新滑块的值
        healthSlider.value = characterHP.currentHealth;
        manaSlider.value = characterMana.currentmana;
    }
}
