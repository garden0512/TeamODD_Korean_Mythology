using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MovePlayer : MonoBehaviour
{
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int xDir = Animator.StringToHash("xDir");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int yDir = Animator.StringToHash("yDir");

    Rigidbody2D rd;
    Animator anim;
    SpriteRenderer spriteRenderer;

    private float playerSpeed = 0f;
    public float moveSpeed = 10.0f;

    [SerializeField] private float dashSpeed = 50.0f;
    [SerializeField] private float dashDuration = 0.1625f;
    [SerializeField] public static float dashCooltime = 0.935f;
    
    private Vector2 movement = Vector2.zero;
    private Vector2 _playerRotation = Vector2.zero;
    private Vector2 snappedDirection = Vector2.zero;

    public static bool isMoving = false;
    public static bool isDashing = false;
    private bool isAttacking = false;
    private bool attackcombo1 = true;
    private bool attackcombo2 = false;
    private bool attackcombo3 = false;
    public static bool getHitted = false;
    public static bool muzeok = false;

    [SerializeField] public float attackDuration1 = 0.35f;
    [SerializeField] public float attackDuration2 = 0.35f;
    [SerializeField] public float attackDuration3 = 0.5f;
    [SerializeField] private float comboDuration = 1f;

    private float countAttackTime = 0f;
    private float countComboTime = 0f;
    private float countDashTime = 0f;
    public static float countCoolTime = 0f;

    public GameObject AttackEffectObject;
    private Vector2 EffectPosition = Vector2.zero;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rd = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        AttackEffectObject.SetActive(false);
    }

    void Update()
    {
        PlayerRotation();
        UpdateState();
        Skills();
    }
    private void FixedUpdate()
    {
        MoveCharactor();
    }
    
    private void PlayerRotation()
    {
        if (!isDashing && !isAttacking)
        {

            if (Input.GetKeyDown(KeyCode.W))
            {
                _playerRotation.x = 0;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _playerRotation.x = 0;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                _playerRotation.y = 0;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                _playerRotation.y = 0;
            }

            if (Input.GetKey(KeyCode.W))
            {
                _playerRotation.y = 1;

                if (Input.GetKeyUp(KeyCode.A))
                {
                    _playerRotation.x = 0;
                }
                if (Input.GetKeyUp(KeyCode.D))
                {
                    _playerRotation.x = 0;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    _playerRotation.y = 0;
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _playerRotation.y = -1;

                if (Input.GetKeyUp(KeyCode.A))
                {
                    _playerRotation.x = 0;
                }
                if (Input.GetKeyUp(KeyCode.D))
                {
                    _playerRotation.x = 0;
                }

            }

            if (Input.GetKey(KeyCode.D))
            {
                _playerRotation.x = 1;

                if (Input.GetKeyUp(KeyCode.W))  
                {
                    _playerRotation.y = 0;
                }
                if (Input.GetKeyUp(KeyCode.S))
                {
                    _playerRotation.y = 0;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    _playerRotation.x = 0;
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _playerRotation.x = -1;

                if (Input.GetKeyUp(KeyCode.W))
                {
                    _playerRotation.y = 0;
                }
                if (Input.GetKeyUp(KeyCode.S))
                {
                    _playerRotation.y = 0;
                }
            }
                        
        }

        anim.SetFloat(Horizontal, _playerRotation.x);
        anim.SetFloat(Vertical, _playerRotation.y);
    }
    private void UpdateState()
    {
        if (Mathf.Approximately(movement.x, 0) && Mathf.Approximately(movement.y , 0))
        {
            isMoving = false;
            anim.SetBool("isMove", false);
        }
        else
        {
            isMoving = true;
            anim.SetBool("isMove", true);
        }

        anim.SetFloat(xDir, movement.x);
        anim.SetFloat(yDir, movement.y);
    }

    private void Skills()
    {
        countComboTime += Time.deltaTime;
        countCoolTime += Time.deltaTime;

        if (countComboTime > comboDuration)
        {
            ComboReset();
        }

        if (isAttacking)
        {
            countAttackTime += Time.deltaTime;

            if (countAttackTime >= attackDuration1 && attackcombo1) // 1st
            {
                attackcombo1 = false;
                attackcombo2 = true;
                EndAttack();
            }

            else if (countAttackTime >= attackDuration2 && attackcombo2) // 2nd
            {
                attackcombo2 = false;
                attackcombo3 = true;
                EndAttack();
            }

            else if (countAttackTime >= attackDuration3 && attackcombo3) // 3rd
            {
                attackcombo3 = false;
                attackcombo1 = true;
                EndAttack();
            }

        }

        else if (isDashing)
        {
            countDashTime += Time.deltaTime;

            if (countDashTime >= dashDuration)
            {
                EndDash();
                anim.SetBool("isDash", false);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AttackCombo();
        }

        if (Input.GetKeyDown(KeyCode.Space) && countCoolTime >= dashCooltime && isMoving)
        {
            EndAttack();

            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement.Normalize();
            PlayerRotation();

            StartDash();
        }

    }
    private void MoveCharactor()
    {

        if (!isDashing && !isAttacking)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            movement.Normalize();

            playerSpeed = moveSpeed;
        }

        else if (isAttacking)
        {
            movement.x = snappedDirection.x;
            movement.y = snappedDirection.y;

            movement.Normalize();

            playerSpeed = 5f;
        }

        else if (isDashing)
        {
            if (countDashTime >= dashDuration - 0.075f)
            {
                playerSpeed = 0.5f;
            }
            else
            {
                playerSpeed = dashSpeed;
            }
        }

        rd.velocity = movement * playerSpeed;
    }
        

    
    private void StartDash()
    {
        isDashing = true;
        anim.SetBool("isAttack", false);
        anim.SetBool("isAttack1", false);
        anim.SetBool("isAttack2", false);
        anim.SetBool("isAttack3", false);
        anim.SetBool("isMove", true);
        anim.SetBool("isDash", true);
        gameObject.tag = "DashingPlayer";
        countDashTime = 0f;
        countCoolTime = 0f;
    }
    private void EndDash()
    {
        isDashing = false;
        gameObject.tag = "Player";

        if (Input.GetKey(KeyCode.W))
        {
            _playerRotation.x = 0;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _playerRotation.x = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _playerRotation.y = 0;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _playerRotation.y = 0;
        }
    }

    private void AttackCombo()
    {
        anim.SetBool("isAttack", true);

        if (attackcombo1)
        {
            anim.SetBool("isAttack1", true);
        }
        else if (attackcombo2)
        {
            anim.SetBool("isAttack2", true);
        }
        else if (attackcombo3)
        {
            anim.SetBool("isAttack3", true);
        }

        StartAttack();
    }
    private void StartAttack()
    {
        isAttacking = true;
        countAttackTime = 0f;
        countComboTime = 0f;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = Mathf.Round(angle / 45) * 45;
        float radian = angle * Mathf.Deg2Rad;

        snappedDirection = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

        _playerRotation.x = snappedDirection.x;
        _playerRotation.y = snappedDirection.y;

        EffectPosition = transform.position + transform.forward * 5f;
        AttackEffectObject.transform.position = EffectPosition;

        AttackEffectObject.SetActive(true);
    }
    private void EndAttack()
    {
        isAttacking = false;
        isMoving = false;
        anim.SetBool("isMove", false);
        anim.SetBool("isAttack", false);
        anim.SetBool("isAttack1", false);
        anim.SetBool("isAttack2", false);
        anim.SetBool("isAttack3", false);
        AttackEffectObject.SetActive(false);

        if (Input.GetKey(KeyCode.W))
        {
            _playerRotation.x = 0;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _playerRotation.x = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _playerRotation.y = 0;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _playerRotation.y = 0;
        }
    }
    private void ComboReset()
    {
        attackcombo1 = true;
        attackcombo2 = false;
        attackcombo3 = false;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && gameObject.tag == "Player" && getHitted == false)
        {
            getHitted = true;
            StartCoroutine("BlinkTime");
        }
    }

    IEnumerator BlinkTime()
    {
        int countTime = 0;
        PlayerUI.currentHealth -= PlayerUI.damage;
        while (countTime < 6)
        {
            if (countTime % 2 == 0)
            {
                spriteRenderer.color = new Color32(255, 255, 255, 150);
            }
            else
            {
                spriteRenderer.color = new Color32(255, 255, 255, 200);
            }
            yield return new WaitForSeconds(0.3f);

            countTime++;
        }
        getHitted = false;
        spriteRenderer.color = new Color32(255, 255, 255, 255);

        yield return null;
    }

}