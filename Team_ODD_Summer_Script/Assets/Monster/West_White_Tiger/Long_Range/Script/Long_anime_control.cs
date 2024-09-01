using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Long_anime_control : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnAttackReady()
    {
        anim.SetFloat("AttackSpeed", 0.6f);
    }

    void OnAttackBegin()
    {
        anim.SetFloat("AttackSpeed", 0.3f);
    }

    void OnAttackEnd()
    {
        anim.SetFloat("AttackSpeed", 0.6f);
    }
}
