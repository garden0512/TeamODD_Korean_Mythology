using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WT_Control : MonoBehaviour
{
    public static WT_Control instance;
    [Tooltip("몬스터(백호)의 움직임에 관한 변수들입니다.")]
    [Header("Monster Move Info")]
    public float speed;
    private Vector2 randomDirection;
    private float changeDirectionTime = 2f;
    private float changeDirectionTimer;
    [Tooltip("몬스터(백호)의 플레이어 감지에 관련된 변수들입니다.")]
    [Header("Recognition Info")]
    public float detectionRadius = 5f; // 감지 범위
    public Rigidbody2D target;
    private bool _isPlayerInRange;
    public bool isPlayerInRange
    {
        get => _isPlayerInRange;
        set => _isPlayerInRange = value;
    }
    [Tooltip("몬스터(백호)의 체력에 대한 변수들입니다.")]
    [Header("Monster HP Info")]
    public float maxHealth = 1000f;
    public static float _health;
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
    [Tooltip("몬스터(백호)의 공격에 대한 변수들입니다.")]
    [Header("Monster Attack Info")]
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    private float attackInterval = 3f;
    private float attackTimer;
    private bool isAttacking = false;
    private WT_Skill wtskill;
    private Vector2 initialAttackPosition;
    
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
        wtskill = GetComponent<WT_Skill>();
        SetRandomDirection();
        _health = maxHealth;
    }

    void FixedUpdate()
    {
        if (!isLive || isAttacking || wtskill.isRushing)
            return;

        TakeDamage();

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
            //Debug.Log("isPlayerInRange = true");
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
        if(isAttacking)
            return;

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            UseRandomSkill();
            attackTimer = attackInterval;
        }
    }

    void UseRandomSkill()
    {
        if(wtskill == null)
        {
            return;
        }
        int randomSkill = Random.Range(0, 4);
        switch (randomSkill)
        {
            case 0:
                wtskill.VerticalSlash();
                Debug.Log("Using Vertical Slash");
                break;
            case 1:
                wtskill.HorizontalSlash();
                Debug.Log("Using Horizontal Slash");
                break;
            case 2:
                wtskill.TapDown();
                Debug.Log("Using Tap Down");
                break;
            case 3:
                wtskill.Rush();
                Debug.Log("Using Rush");
                break;
        }
    }

    public void TakeDamage()
    {
        if (!isLive)
            return;

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