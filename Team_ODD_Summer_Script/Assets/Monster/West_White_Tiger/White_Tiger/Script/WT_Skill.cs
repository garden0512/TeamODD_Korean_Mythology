using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WT_Skill : MonoBehaviour
{
    public Rigidbody2D target;
    private bool isAttacking = false;
    private bool isRushing = false;
    private float attackInterval = 3f;
    private float attackTimer;
    private Rigidbody2D rigid;
    private Vector2 initialAttackPosition;
    private float rushSpeedMultiplier = 3f;
    private WT_Control control;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        attackTimer = attackInterval;
        control = WT_Control.instance;
    }

    //종(가로)베기
    public void VerticalSlash()
    {
        if (isAttacking)
            return;
        StartCoroutine(PerformAttack());
    }

    //횡(세로)베기
    public void HorizontalSlash()
    {
        if (isAttacking)
            return;
        StartCoroutine(PerformAttack());
        
    }

    //내려치기
    public void TapDown()
    {
        if (isAttacking)
            return;
        StartCoroutine(PerformAttack());
        //MovePlayer.getHitted = true;
    }

    //돌진
    public void Rush()
    {
        // if(isRushing)
        // {
        //     return;
        // }
        // StartCoroutine(RushToPlayer());
    }

    //은신
    public void Stealth()
    {

    }

    //이올
    public void Iol()
    {

    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;
        attackTimer = attackInterval;
        initialAttackPosition = rigid.position;

        yield return new WaitForSeconds(0.5f);

        PlayerUI playerHealth = target.GetComponent<PlayerUI>();
        if(playerHealth != null)
        {
            playerHealth.Damage(10f);
        }

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
    }

    // IEnumerator RushToPlayer()
    // {
    //     isRushing = true;
    //     float rushDuration = 6f;
    //     float rushSpeed = WT_Control.speed * rushSpeedMultiplier;
    //     Vector2 direction = (target.position - rigid.position).normalized;
    //     while (rushDuration > 0f)
    //     {
    //         rigid.velocity = direction * rushSpeed;
    //         float distanceToPlayer = Vector2.Distance(rigid.position, target.position);
    //         if (distanceToPlayer <= 1f) // 충돌 감지 거리
    //         {
    //             Debug.Log("제압 실행");
    //             // 여기에 제압 스킬 발동 로직을 추가할 수 있습니다.
    //             break;
    //         }
    //         rushDuration -= Time.deltaTime;
    //         yield return null;
    //     }
    //     rigid.velocity = Vector2.zero;
    //     isRushing = false;
    // }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (isRushing && collision.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("제압 실행");
    //     }
    // }
}
