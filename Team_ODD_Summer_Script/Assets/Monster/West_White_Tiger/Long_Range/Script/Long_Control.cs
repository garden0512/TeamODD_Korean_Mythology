using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Long_Control : MonoBehaviour
{
    public GameObject AttackEffectObject;
    private Vector2 EffectPosition = Vector2.zero;

    public Long_Control instance;
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
    public float _health;
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
    [Header("Background Settings")]
    public SpriteRenderer background;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    private float attackInterval = 3f;
    private float attackTimer;
    private bool isAttacking = false;
    private Vector2 initialAttackPosition;
    private float attackChargeTimer;
    private Animator anim;
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private float lastHorizontal = 0f;
    private float lastVertical = 0f;
    private bool isHeal = false;
    private float NOD = 0f;
    private ObjectPool effectPool;
    private GameObject currentEffect;

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
        anim = GetComponent<Animator>();
        SetRandomDirection();
        _health = maxHealth;
        attackChargeTimer = attackChargeTime;
        //OnEnable();
    }

    void Start()
    {
        effectPool = FindObjectOfType<ObjectPool>();
        AttackEffectObject.SetActive(false);
    }

    private void Update()
    {
        TakeDamage();
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
                AnimeUpdate(dirVec);
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
            CheckAndCorrectDirectionWithinBounds();
            Vector2 nextVec = randomDirection * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            AnimeUpdate(randomDirection);
        }

        rigid.velocity = Vector2.zero;
    }

    void SetRandomDirection()
    {
        randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        changeDirectionTimer = changeDirectionTime;
    }

    void CheckAndCorrectDirectionWithinBounds()
    {
        Bounds bounds = background.bounds; // 배경의 경계

        // 현재 위치
        Vector2 position = rigid.position;

        // 경계와의 거리 계산
        float distanceToLeft = position.x - bounds.min.x;
        float distanceToRight = bounds.max.x - position.x;
        float distanceToBottom = position.y - bounds.min.y;
        float distanceToTop = bounds.max.y - position.y;

        // 북쪽 경계와의 거리가 가까운 경우
        if (distanceToTop < 1f && randomDirection.y > 0)
        {
            SetRandomDirectionExcluding(Vector2.up);
        }
        // 남쪽 경계와의 거리가 가까운 경우
        else if (distanceToBottom < 1f && randomDirection.y < 0)
        {
            SetRandomDirectionExcluding(Vector2.down);
        }
        // 동쪽 경계와의 거리가 가까운 경우
        else if (distanceToRight < 1f && randomDirection.x > 0)
        {
            SetRandomDirectionExcluding(Vector2.right);
        }
        // 서쪽 경계와의 거리가 가까운 경우
        else if (distanceToLeft < 1f && randomDirection.x < 0)
        {
            SetRandomDirectionExcluding(Vector2.left);
        }
    }

    void SetRandomDirectionExcluding(Vector2 excludedDirection)
    {
        Vector2 newDirection;
        do
        {
            newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        } while (Vector2.Dot(newDirection, excludedDirection) > 0.7f); // 제외할 방향과 비슷한 경우 다시 랜덤 생성

        randomDirection = newDirection;
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

    private void AnimeUpdate(Vector2 direction)
    {
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            anim.SetFloat(Horizontal, direction.x > 0 ? 1 : -1);
            anim.SetFloat(Vertical, 0);
            lastHorizontal = direction.x > 0 ? 1 : -1;
            lastVertical = 0;
        }
        else
        {
            anim.SetFloat(Horizontal, 0);
            anim.SetFloat(Vertical, direction.y > 0 ? 1 : -1);
            lastHorizontal = 0;
            lastVertical = direction.y > 0 ? 1 : -1;
        }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        anim.SetFloat(Horizontal, lastHorizontal);
        anim.SetFloat(Vertical, lastVertical);
        attackTimer = attackInterval;
        initialAttackPosition = rigid.position;

        Vector2 direction = transform.position.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = Mathf.Round(angle / 45) * 45;
        float radian = angle * Mathf.Deg2Rad;

        Vector2 snappedDirection = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

        EffectPosition = (Vector2)transform.position + snappedDirection ;
        Quaternion effectRotation = Quaternion.Euler(0, 0, angle + 90);

        AttackEffectObject.transform.position = EffectPosition;
        AttackEffectObject.transform.rotation = effectRotation;
        AttackEffectObject.SetActive(true);

        PlayerUI playerHealth = target.GetComponent<PlayerUI>();
        if(playerHealth != null)
        {
            playerHealth.Damage(10f);
        }

        yield return new WaitForSeconds(0.5f);

        // Resume charging attack
        while (attackChargeTimer > 0)
         {
             attackChargeTimer -= Time.deltaTime;
             attackIndicator.fillAmount = (attackChargeTime - attackChargeTimer) / attackChargeTime;
             yield return null;
        }


        yield return new WaitUntil(()=> anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        isAttacking = false;
        anim.SetBool("isAttacking", false);
        AttackEffectObject.SetActive(false);
    }

    public void TakeDamage()
    {
        if (!isLive)
            return;

        if (_health <= 0)
        {
            if(NOD == 1)
            {
                Die();
            }
            else
            {
                FakeDie();
            }
        }
    }

    void FakeDie()
    {
        isLive = false;
        anim.SetBool("isFakeDie", true);
        anim.SetFloat(Horizontal, lastHorizontal);
        anim.SetFloat(Vertical, lastVertical);
        Debug.Log("Monster fakedied");
        isHeal = true;

        StartCoroutine(Resurrection());
    }

    IEnumerator Resurrection()
    {
        yield return new WaitUntil(()=> anim.GetCurrentAnimatorStateInfo(0).IsName("FakeDie") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        if(isHeal)
        {
            anim.SetBool("isHeal", true);
            Debug.Log("Monster Healing");

            yield return new WaitUntil(()=> anim.GetCurrentAnimatorStateInfo(0).IsName("Resurrection") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

            isLive = true;
            anim.SetBool("isLive", true);
            anim.SetBool("isHeal", false);
            isHeal = false;
            anim.SetBool("isFakeDie", false);
            _health = 1000f;
            NOD++;
            Debug.Log("부활");
        }
        else
        {
            Debug.Log("오류");
        }
    }

    void Die()
    {
        isLive = false;
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        anim.SetBool("isRealDie", true);
        anim.SetBool("isLive", false);
        anim.SetFloat(Horizontal, lastHorizontal);
        anim.SetFloat(Vertical, lastVertical);
        Debug.Log("Monster died");
        yield return new WaitUntil(()=> anim.GetCurrentAnimatorStateInfo(0).IsName("Die") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Destroy(gameObject);
    }

    void OnEnable()
    {
        effectPool = FindObjectOfType<ObjectPool>();
        if (MovePlayer.instance != null)
        {
            target = MovePlayer.instance.GetComponent<Rigidbody2D>();
            if (target == null)
            {
                Debug.LogError("MovePlayer.instance does not have a Rigidbody2D component.");
            }
        }
        else
        {
            Debug.LogError("MovePlayer.instance is not assigned.");
        }

        if (World_Manager.instance != null)
        {
            if (World_Manager.instance.backimage != null)
            {
                background = World_Manager.instance.backimage.GetComponent<SpriteRenderer>();
                if (background == null)
                {
                    Debug.LogError("World_Manager.backimage does not have a SpriteRenderer component.");
                }
            }
            else
            {
                Debug.LogError("World_Manager.instance.backimage is not assigned.");
            }
        }
        else
        {
            Debug.LogError("World_Manager.instance is not assigned.");
        }
        if (effectPool != null)
        {
            currentEffect = effectPool.GetEffect(); // Effect 프리팹을 가져옴
            if (currentEffect != null)
            {
                AttackEffectObject = currentEffect;
            }
            else
            {
                Debug.LogError("Failed to get an effect from the ObjectPool.");
            }
        }
        else
        {
            Debug.LogError("ObjectPool is not assigned or found.");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Effect")
        {
            _health -= 250;
        }
    }

    public void ShowAttackEffect()
    {
        if (currentEffect != null)
        {
            effectPool.ReturnEffect(currentEffect);
        }
        currentEffect = effectPool.GetEffect();
        currentEffect.transform.position = transform.position;

        Vector2 direction = (target.position - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float adjustedAngle = angle - 90f;
        currentEffect.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        currentEffect.SetActive(true);
    }

    public void HideAttackEffect()
    {
        if (currentEffect != null)
        {
            effectPool.ReturnEffect(currentEffect);
            currentEffect = null;
        }
        AttackEffectObject.SetActive(false);
    }
}
