using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class RangeAttacker : Entity
{
    public projectile bullet;
    
    public Transform groundCheck;
    public Transform rightWallCheck;
    public Transform leftWallCheck;

    public bool isGrounded;
    public bool isMovingRight;
    public bool isStuck;
    public bool isAttackOnCooldown;

    private float groundRadius = 0.3f;
    private float partitionStep = 0.2f;
    public float attackCooldown = 2;
    public float speed = 1.5f;
    [FormerlySerializedAs("attackRange")] public float maxAttackRange = 8;
    public float attackHeight = 3;
    private float jumpForce = 3;
    public float movementX;
    public float movementY;
    
    
    public LayerMask layerGround;
    public LayerMask playerLayer;
    
    private new Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    
    private Transform playerTransform;

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, layerGround);
        rigidbody.velocity = new Vector2(movementX, rigidbody.velocity.y + movementY);
        isStuck = Physics2D.OverlapCircle(rightWallCheck.position, groundRadius, layerGround) || Physics2D.OverlapCircle(leftWallCheck.position, groundRadius, layerGround);
    }
    
    private void GetComponents()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponents();
    }

    private void Update()
    {
        if (GetDxToPlayer() <= maxAttackRange && !isAttackOnCooldown && IsCanReachPlayer())
            Attack();
        else if (GetDxToPlayer() > maxAttackRange || !IsCanReachPlayer())
            MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        isMovingRight = playerTransform.position.x > transform.position.x;
        if (isMovingRight)
            movementX = speed;
        else
            movementX = -speed;
        
        if (isStuck && isGrounded)
            rigidbody.velocity = new Vector2(movementX, jumpForce);

    }

    private void Attack()
    {
        movementX = 0;
        
        if (!IsCanReachPlayer()) return;
        isAttackOnCooldown = true;

        var projectile = Instantiate(bullet, transform.position, Quaternion.identity);
        projectile.Launch(CalculateProjectileVector());
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
    }

    private double GetDxToPlayer()
    {
        return transform.position.x - playerTransform.position.x;
    }

    private double GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, playerTransform.position);
    }

    private Vector2 CalculateProjectileVector()
    {
        var distance = Math.Min(Math.Abs(GetDxToPlayer()), maxAttackRange);
        var angleSin = 4 * attackHeight /
                       Math.Sqrt(16 * attackHeight * attackHeight + distance * distance);
        var angleCos = Math.Sqrt(1 - angleSin * angleSin);
        var velocity = Math.Sqrt( 2 * attackHeight * 9.81 / (angleSin * angleSin));

        if (GetDxToPlayer() > 0)
            return new Vector2(-(float) angleCos, (float) angleSin).normalized * (float) velocity;
        return new Vector2((float) angleCos, (float) angleSin).normalized * (float) velocity;
    }

    private bool IsCanReachPlayer()
    {
        var isReachGround = false;
        var isReachPlayer = false;
        var distance = CalculateShotDistance();
        var position = transform.position;
        var isPlayerRight = playerTransform.position.x > position.x;
        var previous = position;
        for (var i = partitionStep; i < maxAttackRange && !(isReachGround || isReachPlayer); i += partitionStep)
        {
            float parabolaValue;
            var x = i;
            if (isPlayerRight)
                parabolaValue = -1 * (8 * attackHeight / distance) / (2 * distance) * x * (x - distance);
            else
            {
                x *= -1;
                parabolaValue = -1 * (8 * attackHeight / distance) / (2 * distance) * x * (x + distance);
            }

            var current = new Vector3(x + position.x, parabolaValue + position.y);
            Debug.DrawLine(previous, current, Color.red, 2f);
            var target = Physics2D.Raycast(previous,
                current - previous, (current - previous).magnitude, (1 << 8) | (1 << 10)).collider;
            if (!(target is null))
            {
                if (target.CompareTag("Player"))
                    isReachPlayer = true;
                else
                    isReachGround = true;
            }
            previous = current;
        }

        return isReachPlayer;
    }
    
    private float CalculateShotDistance()
    {
        var dx = Math.Abs(transform.position.x - playerTransform.position.x);
        var dy = playerTransform.position.y - transform.position.y;
        var distance = -attackHeight * dx * dx
                       / (2 * dy - attackHeight * dx);
        return Math.Min(distance, maxAttackRange);
    }
}
