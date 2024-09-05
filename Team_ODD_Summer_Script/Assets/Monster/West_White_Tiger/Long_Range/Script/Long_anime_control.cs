using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Long_anime_control : MonoBehaviour
{
    Animator anim;
    Long_Control longControl;

    void Start()
    {
        anim = GetComponent<Animator>();
        longControl = GetComponent<Long_Control>();
    }

    void OnAttackReady()
    {
        anim.SetFloat("AttackSpeed", 0.6f);
        if (longControl != null)
        {
            longControl.HideAttackEffect();
        }
    }

    void OnAttackBegin()
    {
        anim.SetFloat("AttackSpeed", 0.2f);
        if (longControl != null)
        {
            longControl.ShowAttackEffect();
        }
    }

    void OnAttackEnd()
    {
        anim.SetFloat("AttackSpeed", 0.6f);
        if (longControl != null)
        {
            longControl.HideAttackEffect();
        }
    }
}
