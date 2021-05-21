using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 4;
    public float shortJumpForce = 3;
    public float jumpForce = 7;
    public float groundRadius = 0.2f;
    public float attackRange = 1f;
    public int health = 100;
    public int cleavePower = 3;
    
    public float movementX;
    public float movementY;

    public bool swordInJump;
    public bool isGrounded;
    public bool isDead;
    public bool right;
    public bool rolling;
    public bool blocked;
    // public bool isJumping;
    public bool isCelled;
    // private bool crouching;

    public Transform groundCheck;
    public Transform cellCheck;
    public Transform attackPosition;
    public Transform supportPosition;
    
    public LayerMask layerGrounds;
    public LayerMask enemies;
    public HealthBar healthBar;
    public CapsuleCollider2D collider;
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private InputMaster input;
    
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    // private static readonly int IsCrouching = Animator.StringToHash("isCrouching");
    private static readonly int IsFalling = Animator.StringToHash("isFalling");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    private static readonly int IsSecondAttack = Animator.StringToHash("isSecondAttack");

    public AudioSource sound;
    public AudioClip removeSword;
    public AudioClip attackSound;
    public AudioClip runSound;
    public AudioClip landingSound;
    public AudioClip jumpSound;

    public static Player Instance;
    
    private void Awake()
    {
        GetComponents();
        Instance = this;
        input = new InputMaster();
        BindMovement();
        healthBar.SetMaxHealth(health);
    }

    private void GetComponents()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
        collider = gameObject.GetComponent<CapsuleCollider2D>();
        sound = gameObject.GetComponentInChildren<AudioSource>();
    }
    private void BindMovement()
    {
        input.Player.Move.performed += context => Move(context.ReadValue<float>());
        input.Player.Move.canceled += context => Move(0);
        input.Player.Jump.performed += context =>
        {
            if (isGrounded)
                Jump();
        };
        // input.Player.Jump.canceled += context => CancelJump();
        input.Player.Attack.performed += context => Attack();
        input.Player.Roll.performed += context =>
        {
            if (isGrounded)
                Roll();
        };
        input.Player.Block.performed += context =>
        {
            if (isGrounded)
                Block();
        };
    }
    void Update()
    {
        animator.SetBool(IsJumping, rigidbody.velocity.y > 0 && !isGrounded);
        animator.SetBool(IsFalling, rigidbody.velocity.y < 0 && !isGrounded);
        swordInJump = animator.GetBool(IsSecondAttack);
    }

    private void FixedUpdate()
    {
        movementY = rigidbody.velocity.y;
        rigidbody.velocity = new Vector2(movementX * speed, movementY);
        // print(rigidbody.velocity.x);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, layerGrounds);
        // if (isGrounded && isJumping)
        //     isJumping = false;
        isCelled = Physics2D.OverlapCircle(cellCheck.position, groundRadius, layerGrounds);
    }

    private void Move(float axis)
    {
        if (!rolling)
        {
            if (axis < 0)
            {
                right = false;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (axis > 0)
            {
                right = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            movementX = axis;
        }

        animator.SetBool(IsRunning, movementX != 0 && !rolling);
    }

    private void Jump()
    {
        rigidbody.velocity = new Vector2(movementX, jumpForce);
        FindObjectOfType<Support>().jump();
        animator.Play("NEW jump");
        // swordInJump = false;
        // isJumping = true;
    }

    private void Block()
    {
        animator.Play("blockParry");
    }

    private void Roll()
    {
        animator.Play("NEW roll");
    }

    private void Attack()
    {
        animator.SetBool(IsAttack, true);
    }

    private void OnAttack()
    {
        var enemiesOnHit = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemies);
        for (var i = 0; i < cleavePower; i++)
        {
            if (i > enemiesOnHit.Length-1) break;
            enemiesOnHit[i].GetComponent<Entity>().GetDamage(50);
        }
    }

    private void PlayAttackSound()
    {
        sound.mute = false;
        sound.pitch = UnityEngine.Random.Range(0.8f, 1.4f);
        sound.PlayOneShot(attackSound);
    }

    private void PlayRemoveSwardSound()
    {
        sound.PlayOneShot(removeSword);
    }

    private void PlayRunSound()
    {
        sound.PlayOneShot(runSound);
    }

    private void PlayLandingSound()
    {
        sound.PlayOneShot(landingSound);
    }

    private void PlayJumpSound()
    {
        sound.PlayOneShot(jumpSound);
    }

    private void StopSound()
    {
        print("stop");
        sound.Stop();
    }


    private void OnDrawGizmosSelected()
     {
         Gizmos.DrawWireSphere(attackPosition.position, attackRange);
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
        if (!blocked)
        {
            health -= damage;
            healthBar.SetHealth(health);
            if (health <= 0)
            {
                isDead = true;
                animator.SetBool(IsDead, true);
                animator.Play("Death");
                input.Disable();
            }
        }
        else
            blocked = false;
    }

    private void DisableWithoutMovement()
    {
        DisableInputException(input.Player.Move);
    }
    private void DisableInputException(InputAction exception)
    {
        input.Player.Attack.Disable();
        input.Player.Jump.Disable();
        input.Player.Roll.Disable();
        // OnDisable();
        // exception.Enable();
    }
    
    
    public void OnEnable() => input.Enable();

    public void OnDisable() => input.Disable();
}
