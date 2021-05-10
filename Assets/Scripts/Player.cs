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
    // public float speedMultiplicator = 1;
    public float jumpForce = 7;
    [SerializeField]public float groundRadius;
    public Transform groundCheck;
    public Transform cellCheck;
    public LayerMask layerGrounds;
    public int health = 100;
    public HealthBar healthBar;
    public int cleavePower = 3;

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
    private static readonly int IsSecondAttack = Animator.StringToHash("isSecondAttack");
    private static readonly int IsThirdAttack = Animator.StringToHash("isThirdAttack");

    private bool isAttacking = false;
    
    public bool right = true;
    public Transform rightAttackPosition;
    public Transform leftAttackPosition;
    public float attackRange;
    //private bool[] comboAttack = new[] {true, false, false};
    public LayerMask enemies;
    [SerializeField]int attackCounter;
    private bool canRunFirstAttack = true;
    private bool canRunSecondAttack;
    private bool canRunThirdAttack;
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
        if (attackCounter == 1 && canRunFirstAttack)
            firstAttack();
        if (attackCounter >= 2 && canRunSecondAttack)
            secondAttack();
        if (attackCounter == 3 && canRunThirdAttack)
            thirdAttack();
        animator.SetBool(IsJumping, rigidbody.velocity.y > 0 && !isGrounded);
        animator.SetBool(IsFalling, rigidbody.velocity.y < 0 && !isGrounded);
        //animator.SetBool(IsCrouching, crouching);
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(movementX, rigidbody.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, layerGrounds);
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
        if(isGrounded && attackCounter<3)
            attackCounter++;
    }
    private void RunSecondAttack()
    {
        canRunSecondAttack = true;
        canRunFirstAttack = false;
    }

    private void RunThirdAttack()
    {
        canRunThirdAttack = true;
        canRunSecondAttack = false;
    }
    private void firstAttack()
    {
        print("first attack");
        cleavePower = 3;
        animator.SetBool(IsAttack, true);
        StartCoroutine(AttackAnimation(IsAttack, 0.6f));
        StartCoroutine(AttackCoolDown(2f));
    }
    private void secondAttack()
    {
        if (isGrounded && attackCounter >= 2)
        {
            print("second attack");
            cleavePower = 5;
            animator.SetBool(IsSecondAttack, true);
            StartCoroutine(AttackAnimation(IsSecondAttack, 0.45f));
            canRunSecondAttack = false;
        }
    }

    private void thirdAttack()
    {
        if (isGrounded && attackCounter >= 3)
        {
            print("third attack");
            cleavePower = 7;
            animator.SetBool(IsThirdAttack, true);
            StartCoroutine(AttackAnimation(IsThirdAttack, 0.683f));
            attackCounter++;
            canRunThirdAttack = false;
        }
    }
    IEnumerator AttackAnimation(int id, float attackDuration)
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
        animator.SetBool(id, false);
    }

    IEnumerator AttackCoolDown(float attackDuration)
    {
        yield return new WaitForSeconds(attackDuration);
        canRunFirstAttack = true;
        canRunSecondAttack = false;
        canRunThirdAttack = false;
        attackCounter = 0;
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
                // print(enemiesOnHit[i].gameObject.name);
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
        // Destroy(boxCollider);
        // Destroy(rigidbody);
    }
    
    private void OnEnable() => input.Enable();

    private void OnDisable() => input.Disable();
}
