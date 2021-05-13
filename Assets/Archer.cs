using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    public bool isRight;
    // public bool movingRight;
    public bool isJumping;
    public bool isInJump;
    public bool isDead;
    public bool isGrounded;
    public float speed = 5;
    public float jumpForce = 7;
    private float distanceToPlayer;
    public float groundRadius = 1;
    
    public LayerMask layerGrounds;
    private float movementX;
    private float movementY;
    
    private new Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private InputMaster input;

    public Transform player;
    public Transform leftPosition;
    public Transform rightPosition;
    public Transform rightFall;
    public Transform leftFall;
    public Transform groundCheck;
    public static Archer Instance;


    // private static readonly int IsIdle = Animator.StringToHash("isIdling");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsFalling = Animator.StringToHash("isFalling");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    // private static readonly int IsDying = Animator.StringToHash("isDying");
    // private static readonly int IsAttack = Animator.StringToHash("isAttack");
    // Start is called before the first frame update
    private void Awake()
    {
        GetComponents();
        Instance = this;
        // input = new InputMaster();
        // BindMovement();
    }
    private void GetComponents()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }
    private void BindMovement()
    {

    }

    private void FixedUpdate()
    {
        isRight = player.GetComponent<Player>().right;
        isJumping = player.GetComponent<Player>().isJumping;
        isDead = player.GetComponent<Player>().isDead;
        // movementY = player.GetComponent<Player>().ySpeed;
        movementY = rigidbody.velocity.y;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, layerGrounds);
        if (isGrounded)
            isInJump = false;
        animator.SetBool(IsJumping, movementY>0 && !isGrounded);
        animator.SetBool(IsFalling, movementY<0 && !isGrounded);
        animator.SetBool(IsRunning, movementX != 0);
        if (isRight)
            distanceToPlayer = Math.Abs(leftPosition.position.x - transform.position.x);
        else
            distanceToPlayer = Math.Abs(rightPosition.position.x - transform.position.x);
        if (isDead)
        {
            animator.SetBool(IsDead, true);
            enabled = false;
            StopAllCoroutines();
        }
        if (isJumping)
            jump();
        else if (!isGrounded)
            AirControl();
        else if (distanceToPlayer > 1 && isRight && isGrounded && Grounded(leftFall)) 
            movingToPlayer(leftPosition);
        else if (distanceToPlayer > 1 && !isRight && isGrounded && Grounded(rightFall))
            movingToPlayer(rightPosition);
        else
        {
            movementX = 0;
            spriteRenderer.flipX = !isRight;
            // animator.SetBool(IsRunning, false);
        }
        rigidbody.velocity = new Vector2(movementX, rigidbody.velocity.y);

    }

    private bool Grounded(Transform fall)
    {
        return Physics2D.OverlapCircle(fall.position, groundRadius, layerGrounds);
    }
    private void idle()
    {
        
    }

    private void movingToPlayer(Transform playerPosition)
    {
        // animator.SetBool(IsRunning, true);
        var delta = transform.position.x - playerPosition.position.x;
        if (delta < 0.2 && delta > 0 ||delta > -0.2 && delta < 0)
            movementX = 0;
        else if (delta > 0)
        {
            movementX = -speed;
            if (!isGrounded)
                movementX = 1.3f * (-speed);
            spriteRenderer.flipX = true;
        }
        else
        {
            movementX = speed;
            if (!isGrounded)
                movementX = 1.3f * speed;
            spriteRenderer.flipX = false;
        }
    }

    private void jump()
    {
        // animator.SetBool(IsRunning, false);
        if (isGrounded)
        {
            spriteRenderer.flipX = !isRight;
            print("DidThis");
            rigidbody.velocity = new Vector2(movementX, jumpForce);
        }

    }

    private void AirControl()
    {
        movingToPlayer(player.transform);
    }

    public void TakeDamage(int damage)
    {
        
    }

}
