using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public float maxHealth = 150f;
    protected float currentHealth;
    [SerializeField] private float damage = 30f;
    [SerializeField] private float damageDelay = 2f;
    private float countDamageDelay = 2f;
    public Slider HpBarSlider;
    [SerializeField] private TextMeshProUGUI textHP;
    
    public Image img_Skill1;
    public Image img_Skill2;    
    public float count1 = 0f;
    public float count2 = 0f;

    public void Start()
    {
        SetHp(150f);
    }
    private void Update()
    {
        countDamageDelay += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Damage(damage);
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
            textHP.text = currentHealth + " / " + maxHealth;
        }
    }

    public void Damage(float damage) //* 데미지 받는 함수
    {

        if (countDamageDelay >= damageDelay)
        {
            if (maxHealth == 0 || currentHealth <= 0) //* 이미 체력 0이하면 패스
                return;
            currentHealth -= damage;
        }

        CheckHp(); //* 체력 갱신

        if (currentHealth <= 0)
        {
            Die();    
        }

        countDamageDelay = 0f;
    }

    public void Die()
    {

    }


}
