using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WT_Skill : MonoBehaviour
{
    public Rigidbody2D target;
    private bool isAttacking = false;
    public bool isRushing = false;
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
        //control = WT_Control.instance;
    }

    void Start()
    {
        control = WT_Control.instance;
        if (control == null)
        {
            Debug.LogError("WT_Control 인스턴스를 찾을 수 없습니다. WT_Control 스크립트가 제대로 할당되었는지 확인하십시오.");
        }
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
        if(isRushing)
        {
            return;
        }
        StartCoroutine(RushToPlayer());
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

    IEnumerator RushToPlayer()
    {
        isRushing = true;
        float rushDuration = 6f;
        float halfDuration = rushDuration / 2f;
        float elapsedTime = 0f;
        Vector2 direction = (target.position - rigid.position).normalized;

        while (elapsedTime < rushDuration)
        {
            // Calculate smooth speed based on the time elapsed
            float t = elapsedTime < halfDuration 
                ? Mathf.SmoothStep(0, 1, elapsedTime / halfDuration)  // Accelerate in the first half
                : Mathf.SmoothStep(1, 0, (elapsedTime - halfDuration) / halfDuration);  // Decelerate in the second half

            float currentSpeed = control.speed * rushSpeedMultiplier * t;
            rigid.velocity = direction * currentSpeed;

            float distanceToPlayer = Vector2.Distance(rigid.position, target.position);
            if (distanceToPlayer <= 1f)
            {
                Debug.Log("제압 실행");
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rigid.velocity = Vector2.zero;
        isRushing = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isRushing && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("제압 실행");
        }
    }
}
