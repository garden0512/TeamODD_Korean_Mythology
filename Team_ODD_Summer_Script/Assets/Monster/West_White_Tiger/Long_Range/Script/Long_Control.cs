using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Long_Control : MonoBehaviour
{
    public static Long_Control instance;
    [Tooltip("몬스터의 움직임에 관한 변수들입니다.")]
    [Header("Monster Move Info")]
    public float speed;
    private Vector2 randomDirection;
    private float changeDirectionTime = 2f;
    private float changeDirectionTimer;
    [Tooltip("몬스터의 플레이어 감지에 관련된 변수들입니다.")]
    [Header("Recognition Info")]
    public float detectionRadius = 5f; // 감지 범위
    public Rigidbody2D target;
    private bool _isPlayerInRange;
    public bool isPlayerInRange
    {
        get => _isPlayerInRange;
        set => _isPlayerInRange = value;
    }
    [Tooltip("몬스터의 체력에 대한 변수들입니다.")]
    [Header("Monster HP Info")]
    public float maxHealth = 1000f;
    private float _health;
    public float health
    {
        get => _health;
        set => _health = value;
    }
    private bool isLive = true;
    public bool IsLive
    {
        get => isLive;
        set => isLive = value;
    }
    [Header("Attack Indicator")]
    public Image attackIndicator; // UI Image component for the filling glass
    public float attackChargeTime = 2f; // Time it takes to fully charge the attack

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    private float attackInterval = 3f;
    private float attackTimer;
    private bool isAttacking = false;
    private Vector2 initialAttackPosition;
    private float attackChargeTimer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        SetRandomDirection();
        _health = maxHealth;
        attackChargeTimer = attackChargeTime;
    }

    void FixedUpdate()
    {
        if (!isLive || isAttacking)
            return;

        float distanceToPlayer = Vector2.Distance(rigid.position, target.position);

        if (distanceToPlayer < detectionRadius)
        {
            if (distanceToPlayer <= 5f)
            {
                AttackPlayer();
                rigid.velocity = Vector2.zero;
                return;
            }
            else
            {
                Vector2 dirVec = target.position - rigid.position;
                Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
                rigid.MovePosition(rigid.position + nextVec);
            }
            _isPlayerInRange = true;
            Debug.Log("isPlayerInRange = true");
        }
        else
        {
            _isPlayerInRange = false;
            //Debug.Log("isPlayerInRange = false");
            changeDirectionTimer -= Time.fixedDeltaTime;
            if (changeDirectionTimer <= 0)
            {
                SetRandomDirection();
            }
            Vector2 nextVec = randomDirection * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }

        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!isLive || isAttacking)
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    void SetRandomDirection()
    {
        randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        changeDirectionTimer = changeDirectionTime;
    }

    void AttackPlayer()
    {
        if (isAttacking)
            return;

        attackChargeTimer -= Time.deltaTime;

        if (attackChargeTimer <= 0)
        {
            StartCoroutine(PerformAttack());
        }
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

        // // Resume charging attack
        // while (attackChargeTimer > 0)
        // {
        //     attackChargeTimer -= Time.deltaTime;
        //     attackIndicator.fillAmount = (attackChargeTime - attackChargeTimer) / attackChargeTime;
        //     yield return null;
        // }
    }

    public void TakeDamage(float damage)
    {
        if (!isLive)
            return;

        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isLive = false;
        Debug.Log("Monster died");
        Destroy(gameObject);
    }
}
