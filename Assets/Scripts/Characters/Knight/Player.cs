using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int damage = 50;
    public int cleavePower = 3;
    public float speed = 4;
    public float shortJumpForce = 3;
    public float jumpForce = 7;
    public float groundRadius = 0.2f;
    public float attackRange = 3f;
    public float maxHealth = 300;
    public float health = 300;
    public float rage = 0;
    public float rageMoidfier = 1;

    public float movementX;
    public float movementY;

    public bool swordInJump;
    public bool isGrounded;
    public bool isDead;
    public bool right;
    public bool rolling;
    public bool blocked;
    public bool isWithSword;
    public bool canBlock = true;
    public bool rageMode;
    public bool specialAttack;
    public bool isHpBottleFull;
    public bool canFillBottle;
    public bool gameOver;
    public bool victory;
    public bool isCelled;
    private bool canRoll = true;

    public Transform groundCheck;
    public Transform cellCheck;
    public Transform attackPosition;
    public Transform supportPosition;
    
    public LayerMask layerGrounds;
    public LayerMask destructibleObjects;
    public HealthBar healthBar;
    public HealthBar rageBar;
    public CapsuleCollider2D collider;
    public GameObject splash;
    public GameObject tornado;
    private Animator animator;
    private InputMaster input;
    private new Rigidbody2D rigidbody;
    private readonly LayerMask enemies = (1 << 9) | (1 << 14);
    
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsDead = Animator.StringToHash("isDead");
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
    public static int Difficult;
    
    private void Awake()
    {
        Difficult = PlayerPrefs.GetInt("difficult");
        GetComponents();
        BindMovement();
        Instance = this;
        input = new InputMaster();
        healthBar.SetMaxHealth(health);
        rageBar.SetMaxHealth(100);
        rageBar.SetHealth(0);
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
        input.Player.Attack.performed += context => Attack();
        input.Player.Roll.performed += context =>
        {
            if (isGrounded && canRoll)
                Roll();
        };
        input.Player.Block.performed += context =>
        {
            if (isGrounded && canBlock)
                Block();
        };
        input.Player.UseHPBottle.performed += context =>
        {
            if (isHpBottleFull && health != 100)
            {
                isHpBottleFull = false;
                health = health + 75;
                health = health > maxHealth ? maxHealth : health;
            }
            else if(canFillBottle)
            {
                isHpBottleFull = true;
            }
        };
    }
    void Update()
    {
        animator.SetBool(IsJumping, rigidbody.velocity.y > 0 && !isGrounded);
        animator.SetBool(IsFalling, rigidbody.velocity.y < 0 && !isGrounded);
        swordInJump = animator.GetBool(IsSecondAttack);
        isWithSword = animator.GetBool(IsSecondAttack);
        
        if (!rageMode) return;
        speed = 6;
        animator.speed = 1.5f;
        rage -= 30 * Time.deltaTime;
        rageBar.SetHealth(rage);
        
        if (!(rage <= 0)) return;
        animator.speed = 1;
        speed = 4;
        rageMode = false;
    }


    private void FixedUpdate()
    {
        healthBar.SetHealth(health);
        movementY = rigidbody.velocity.y;
        rigidbody.velocity = new Vector2(movementX * speed, movementY);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, layerGrounds)
                     || Physics2D.OverlapCircle(groundCheck.position, groundRadius, destructibleObjects);
        isCelled = Physics2D.OverlapCircle(cellCheck.position, groundRadius, layerGrounds)
                   || Physics2D.OverlapCircle(cellCheck.position, groundRadius, destructibleObjects);
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
    }

    private void Block()
    {
        animator.Play("blockParry");
        StartCoroutine(BlockCooldown());
    }

    private IEnumerator BlockCooldown()
    {
        canBlock = false;
        yield return new WaitForSeconds(2f);
        canBlock = true;
    }

    private void Roll()
    {
        animator.Play("NEW roll");
        StartCoroutine(RollColldown());
    }
    private IEnumerator RollColldown()
    {
        canRoll = false;
        yield return new WaitForSeconds(2f);
        canRoll = true;
    }

    private void Attack()
    {
        animator.SetBool(IsAttack, true);
    }

    private void OnAttack()
    {
        if (rageMode && !specialAttack || Difficult == 0 && !specialAttack)
            CreateSplash();
        var enemiesOnHit = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemies);
        for (var i = 0; i < cleavePower; i++)
        {
            if (i > enemiesOnHit.Length-1) break;
            enemiesOnHit[i].GetComponent<Entity>().GetDamage(damage);
            rage += damage * rageMoidfier;
            rageBar.SetHealth(rage);
            
            if (!(rage >= 100)) continue;
            rageMode = true;
            rage = 100;
        }
    }

    public void CreateSplash()
    {
        if (right)
            Instantiate(splash, attackPosition.position + 1.5f * Vector3.right, Quaternion.Euler(0, 0, 0));
        else
            Instantiate(splash, attackPosition.position + 1.5f * Vector3.left, quaternion.Euler(0, 180, 0));
    }

    public void CreateTornado()
    {
        if (specialAttack)
        {
            if (right)
                Instantiate(tornado, attackPosition.position + 1.5f * Vector3.right + Vector3.up, Quaternion.Euler(0, 0, 0));
            else
                Instantiate(tornado, attackPosition.position + 1.5f * Vector3.left + Vector3.up, quaternion.Euler(0, 180, 0));
            specialAttack = false;
        }
        else if (Difficult == 1)
        {
            if (right)
                Instantiate(splash, attackPosition.position + 1.5f * Vector3.right, Quaternion.Euler(0, 0, 0));
            else
                Instantiate(splash, attackPosition.position + 1.5f * Vector3.left, quaternion.Euler(0, 180, 0));
        }
    }
    

    private void PlayAttackSound()
    {
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
        sound.Stop();
    }

    private void GameOver()
    {
        gameOver = true;
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
            
            if (!(health <= 0) || isDead) return;
            isDead = true;
            animator.SetBool(IsDead, true);
            animator.Play("Death");
            OnDisable();
        }
        else
            blocked = false;
    }

    private void DisableWithoutMovement()
    {
        DisableInputException(input.Player.Move);
    }
    
    public void DisableInputException(InputAction exception)
    {
        input.Player.Attack.Disable();
        input.Player.Jump.Disable();
        input.Player.Roll.Disable();
    }
    
    public void OnEnable() => input.Enable();

    public void OnDisable() => input.Disable();

    public void SetRageModifierAfterScene() => rageMoidfier = 0.1f;
}
