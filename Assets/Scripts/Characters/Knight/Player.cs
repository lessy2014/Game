using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    #region init
    public float speed = 5;
    public bool isJumpCancel = false;
    public float shortJumpForce = 4;
    public float jumpForce = 7;
    [SerializeField]public float groundRadius;
    public Transform groundCheck;
    public Transform cellCheck;
    public LayerMask layerGrounds;
    public int health = 100;
    public HealthBar healthBar;
    public int cleavePower = 3;
    public float ySpeed;

    public bool isGrounded;
    private bool isCelled;
    //private bool crouching;
    //private bool crouchingUnpressed;
    public bool isDead;
    public bool isJumping;
    
    private float movementX;
    
    private new Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    
    private InputMaster input;
    // private float currentAxis = 1;

    public static Player Instance;
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    //private static readonly int IsCrouching = Animator.StringToHash("isCrouching");
    private static readonly int IsFalling = Animator.StringToHash("isFalling");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private static readonly int IsDying = Animator.StringToHash("isDying");
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    
    
    public bool right = true;
    public Transform rightAttackPosition;
    public Transform leftAttackPosition;
    public float attackRange;
    public LayerMask enemies;
    #endregion 

    private void Awake()
    {
        GetComponents();
        Instance = this;
        input = new InputMaster();
        BindMovement();
        healthBar.SetMaxHealth(health);
    }

    private void BindMovement()
    {
        input.Player.Move.performed += context => Move(context.ReadValue<float>());
        input.Player.Move.canceled += context => Move(0);
        input.Player.Jump.performed += context => Jump();
        input.Player.Jump.canceled += context => CancelJump();
        input.Player.Attack.performed += context => Attack();
    }

    private void CancelJump()
    {
        if (rigidbody.velocity.y > shortJumpForce)
            rigidbody.velocity = new Vector2(movementX, shortJumpForce);
    }

    private void GetComponents()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        ySpeed = rigidbody.velocity.y;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
        animator.SetBool(IsJumping, rigidbody.velocity.y > 0 && !isGrounded);
        animator.SetBool(IsFalling, rigidbody.velocity.y < 0 && !isGrounded);
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(movementX * speed, rigidbody.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, layerGrounds);
        if (isGrounded)
            isJumping = false;
        isCelled = Physics2D.OverlapCircle(cellCheck.position, groundRadius, layerGrounds);
    }

    private void Move(float axis)
    {
        if (axis != 0)
            spriteRenderer.flipX = axis < 0 && !isDead;
        if (axis < 0)
            right = false;
        else if (axis > 0)
            right = true;
        movementX = axis;
        animator.SetBool(IsRunning, movementX != 0);
    }

    private void Jump()
    {
        if (!isGrounded)
            return;
        rigidbody.velocity = new Vector2(movementX, jumpForce);
        isJumping = true;
    }

    private void Attack()
    {
        if (isGrounded)
            animator.SetBool(IsAttack, true);
    }

    private void onAttack()
    {
        if (right)
        {
            var enemiesOnHit = Physics2D.OverlapCircleAll(rightAttackPosition.position, attackRange, enemies);
            for (var i = 0; i < cleavePower; i++)
            {
                if (i > enemiesOnHit.Length-1) break;
                enemiesOnHit[i].GetComponent<Entity>().GetDamage(50);
            }
        }
        else
        {
            var enemiesOnHit = Physics2D.OverlapCircleAll(leftAttackPosition.position, attackRange, enemies);
            for (var i = 0; i < cleavePower; i++)
            {
                if (i > enemiesOnHit.Length-1) break;
                enemiesOnHit[i].GetComponent<Entity>().GetDamage(50);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(rightAttackPosition.position, attackRange);
        Gizmos.DrawWireSphere(leftAttackPosition.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 11)
        {
            speed *= 0.8f;
            if (jumpForce > 3)
                jumpForce *= 0.8f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 11)
        {
            speed *= 1.25f;
            if (jumpForce < 7)
                jumpForce *= 1.25f;
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            isDead = true;
            Death();
        }
    }

    private void Dead()
    {
        animator.SetBool(IsDead, true);
    }

    private void Death()
    {
        animator.SetBool(IsDying, true);
        input.Disable();
    }
    
    private void OnEnable() => input.Enable();

    private void OnDisable() => input.Disable();
}
