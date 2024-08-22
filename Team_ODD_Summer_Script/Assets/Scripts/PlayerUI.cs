using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public float maxHealth = 150f;
    public static float currentHealth;
    [SerializeField] public static float damage = 30f;
    public Slider HpBarSlider;
    public Slider ComboSlider;
    //public TMP_Text textHP;
    
    public Image DashCoolTimeImage;
    public Image AttackCoolTimeImage;    

    public void Start()
    {
        SetHp(150f);
    }
    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }

        if (MovePlayer.getHitted == true)
        {
            Damage(damage);
        }


    }

    private void FixedUpdate()
    {
        if(MovePlayer.countComboTime <= MovePlayer.comboDuration)
        {
            ComboSlider.value = MovePlayer.countComboTime / MovePlayer.comboDuration;

        }
        if (MovePlayer.countCoolTime <= MovePlayer.dashCooltime)
        {
            DashCoolTimeImage.fillAmount = MovePlayer.countCoolTime / MovePlayer.dashCooltime;

        }
        if (MovePlayer.countAttackTime <= 0.3f)
        {
            AttackCoolTimeImage.fillAmount = (0.25f-MovePlayer.countAttackTime) / 0.25f;

        }
        
    }

    public void SetHp(float amount) //*Hp설정
    {
        maxHealth = amount;
        currentHealth = maxHealth;
    }

    public void CheckHp() //*HP 갱신
    {
        if (HpBarSlider != null)
        {
            HpBarSlider.value = currentHealth / maxHealth;

            //textHP.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }

    public void Damage(float damage) //* 데미지 받는 함수
    {
        CheckHp(); //* 체력 갱신

        if (currentHealth <= 0)
        {
            Die();    
        }

    }

    public void Die()
    {

    }

}
