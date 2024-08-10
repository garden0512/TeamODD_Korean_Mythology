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
    [SerializeField] private float damageDelay = 2f;
    public Slider HpBarSlider;
    public TMP_Text textHP;
    
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
        if (currentHealth <= 0)
        {
            Die();
        }

        if (MovePlayer.getHitted == true)
        {
            Damage(damage);
        }

    }
        public void SetHp(float amount) //*Hp����
    {
        maxHealth = amount;
        currentHealth = maxHealth;
    }

    public void CheckHp() //*HP ����
    {
        if (HpBarSlider != null)
        {
            HpBarSlider.value = currentHealth / maxHealth;
            textHP.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }

    public void Damage(float damage) //* ������ �޴� �Լ�
    {
       /* if (maxHealth == 0 || currentHealth <= 0) //* �̹� ü�� 0���ϸ� �н�
            return;*/

        CheckHp(); //* ü�� ����
    }

    public void Die()
    {
        return;
    }


}
