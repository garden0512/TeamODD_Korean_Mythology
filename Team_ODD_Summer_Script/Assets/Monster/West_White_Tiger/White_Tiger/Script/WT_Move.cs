using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WT_Move : MonoBehaviour
{
    public static WT_Move instance;
    [Tooltip("몬스터의 움직임에 관한 변수들입니다.")]
    [Header("Monster Move Info")]
    public float speed;
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
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    private Vector2 randomDirection;
    private float changeDirectionTime = 2f;
    private float changeDirectionTimer;
    private float attackInterval = 3f;
    private float attackTimer;
    private bool isAttacking = false;
    private Vector2 initialAttackPosition;

    
}
