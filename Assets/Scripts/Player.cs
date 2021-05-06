using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region init
    public float speed = 5;
    // public float speedMultiplicator = 1;
    public float jumpForce = 7;
    [SerializeField]public float groundRadius;
    public Transform groundCheck;
    public Transform cellCheck;
    public LayerMask layerGrounds;
    public int health = 100;
    public HealthBar healthBar;
    
    private Stack<Coroutine> gettingDamageStack = new Stack<Coroutine>();

    [SerializeField]private bool isGrounded;
    private bool isCelled;
    //private bool crouching;
    //private bool crouchingUnpressed;
    private bool isDead;
    
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

    public bool isAttacking = false;
    public bool readyToAttack = true;

    public Transform attackPosition;
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
        readyToAttack = true;
    }

    private void BindMovement()
    {
        input.Player.Move.performed += context => Move(context.ReadValue<float>());
        input.Player.Move.canceled += context => Move(0);
        input.Player.Jump.performed += context => Jump();
        input.Player.Attack.performed += context => Attack();
        //input.Player.Crouch.performed += context =>
        //{
        //    crouchingUnpressed = false;
        //    Crouch();
        //};
        //input.Player.Crouch.canceled += context =>
        //{
        //    crouchingUnpressed = true;
        //    Uncrouch();
        //};
    }

    private void GetComponents()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        //if (crouchingUnpressed)
        //    Uncrouch();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
        animator.SetBool(IsJumping, rigidbody.velocity.y > 0 && !isGrounded);
        animator.SetBool(IsFalling, rigidbody.velocity.y < 0 && !isGrounded);
        //animator.SetBool(IsCrouching, crouching);
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(movementX, rigidbody.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, layerGrounds);
        isCelled = Physics2D.OverlapCircle(cellCheck.position, groundRadius, layerGrounds);
        //if(health <= 0 && !isDead)
        //{
        //    isDead = true;
        //    //foreach (var damage in gettingDamageStack)
        //    //{
        //    //    StopCoroutine(damage);
        //    //}
        //    Death();
        //}
    }

    private void Move(float axis)
    {
        if (axis != 0)
            spriteRenderer.flipX = axis < 0 && !isDead;
        movementX = axis * speed;
        animator.SetBool(IsRunning, movementX != 0);
    }

    private void Jump()
    {
        if (!isGrounded)
            return;
        rigidbody.velocity = new Vector2(movementX, jumpForce);
    }

    private void Attack()
    {
        if(isGrounded && readyToAttack)
        {
            animator.SetBool(IsAttack, true);
            isAttacking = true;
            readyToAttack = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }

    private void onAttack()
    {
        var enemiesOnHit = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemies);
        foreach (var enemy in enemiesOnHit)
        {
            Debug.Log("Take it!");
            enemy.GetComponent<TrashMonster>().TakeDamage(50);
        }
    }


    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    // }

    IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool(IsAttack, false);
        isAttacking = false;
    }

    IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(1f);
        readyToAttack = true; 
    }
    //private void Crouch()
    //{
    //    if (crouching)
    //        return;

    //    crouching = true;

    //    // Райдер пишет, что последовательный доступ к полям компонента неэффективен
    //    var size = boxCollider.size;
    //    size = new Vector2(size.x, size.y / 2 - 0.2f);
    //    boxCollider.size = size;

    //    var offset = boxCollider.offset;
    //    offset = new Vector2(offset.x, offset.y * 2);
    //    boxCollider.offset = offset;
    //}

    //private void Uncrouch()
    //{
    //    if (isCelled || !crouching)
    //        return;

    //    crouching = false;

    //    var size = boxCollider.size;
    //    size = new Vector2(size.x, (size.y + 0.2f) * 2);
    //    boxCollider.size = size;

    //    var offset = boxCollider.offset;
    //    offset = new Vector2(offset.x, offset.y / 2);
    //    boxCollider.offset = offset;
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 11)
        {
            speed *= 0.8f;
            if (jumpForce > 3)
                jumpForce *= 0.8f;
            //if (health > 0)
            //{
            //    gettingDamageStack.Push(StartCoroutine(GettingDamage()));
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 11)
        {
            speed *= 1.25f;
            if (jumpForce < 7)
                jumpForce *= 1.25f;
            //StopCoroutine(gettingDamageStack.Pop());
        }
    }

    //IEnumerator GettingDamage()
    //{
    //    for (;;)
    //    {
    //        health -= 10;
    //        healthBar.SetHealth(health);
    //        yield return new WaitForSeconds(3f);
    //    }
    //}

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
        speed = 0;
        jumpForce = 0;
        // Destroy(boxCollider);
        // Destroy(rigidbody);
    }
    
    private void OnEnable() => input.Enable();

    private void OnDisable() => input.Disable();
}
