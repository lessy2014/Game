using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Support : MonoBehaviour
{
    public bool isRight;
    // public bool movingRight;
    public bool isJumping;
    public bool isInJump;
    public bool isDead;
    public bool isGrounded;
    public bool isFollowPlayer;
    public float speed = 4f;
    public float jumpForce = 7;
    private float distanceToPlayer;
    public float realDistanceToPlayer;
    public float groundRadius = 1;
    
    public LayerMask layerGrounds;
    public LayerMask destructibleObjects;
    public float movementX;
    public float movementY;
    
    public new Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    private float maxDistanceToPlayer;
    public float previousPositionX;
    public float previousPositionY;

    public Transform player;

    public Transform playerPosition;
    // public Transform leftPosition;
    // public Transform rightPosition;
    public Transform rightFall;
    public Transform leftFall;
    public Transform groundCheck;
    public static Support Instance;


    // private static readonly int IsIdle = Animator.StringToHash("isIdling");
    public static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsFalling = Animator.StringToHash("isFalling");
    public static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    // private static readonly int IsDying = Animator.StringToHash("isDying");
    public static readonly int IsAttack = Animator.StringToHash("isAttack");
    // Start is called before the first frame update
    public virtual void Awake()
    {
        Instance = this;
        // throw new NotImplementedException();
        // input = new InputMaster();
        // BindMovement();
    }
    public void GetComponents()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public virtual void FixedUpdate()
    {
        if (isFollowPlayer)
        {
            State();
            animator.SetBool(IsJumping, movementY > 0 && !isGrounded);
            animator.SetBool(IsFalling, movementY < 0 && !isGrounded);
            animator.SetBool(IsRunning, movementX != 0 && previousPositionX - transform.position.x != 0 || previousPositionY - transform.position.y != 0);
            rigidbody.velocity = new Vector2(movementX, rigidbody.velocity.y);
            previousPositionX = this.transform.position.x;
            previousPositionY = this.transform.position.y;
        }
    }


    public void State()
    {
        isRight = player.GetComponent<Player>().right;
        isDead = player.GetComponent<Player>().isDead;
        movementY = rigidbody.velocity.y;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, layerGrounds) || Physics2D.OverlapCircle(groundCheck.position, groundRadius, destructibleObjects);
        distanceToPlayer = playerPosition.position.x - transform.position.x;
        // if (distanceToPlayer > 0)
        //     isRight = true;
        // else
        //     isRight = false;
        // if (isRight)
        //     distanceToPlayer = Math.Abs(leftPosition.position.x - transform.position.x);
        // else
        //     distanceToPlayer = Math.Abs(rightPosition.position.x - transform.position.x);
        realDistanceToPlayer = Math.Abs((player.position - transform.position).magnitude);
        if (isDead)
        {
            animator.SetBool(IsDead, true);
            enabled = false;
            StopAllCoroutines();
        }
        
        if (isJumping)
        {
            jump();
        }
        else if (!isGrounded)
            AirControl();
        else if (realDistanceToPlayer > 0.5 && this.tag =="Koldun" || realDistanceToPlayer > 4 && this.tag == "Archer")
        {
            movingToPlayer(playerPosition);
        }
        else
        {
            movementX = 0;
            idle();
            // animator.SetBool(IsRunning, false);
        }
        if (realDistanceToPlayer > 15)
            TeleportToPlayer();
    }

    public bool Grounded(Transform fall)
    {
        return Physics2D.OverlapCircle(fall.position, groundRadius, layerGrounds);
    }
    private void idle()
    {
        if (isRight)
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        // spriteRenderer.flipX = !isRight;
    }

    public void movingToPlayer(Transform playerPosition)
    {
        // animator.SetBool(IsRunning, true);
        var delta = transform.position.x - playerPosition.position.x;
        var extraDelta = transform.position.x - player.position.x;
        if (delta < 0.1 && delta > 0 || delta > -0.1 && delta < 0 ||
            !Grounded(rightFall) && Math.Abs(extraDelta) < 0.2)
        {
            movementX = 0;
            idle();
        }
        else if (delta > 0)
        {
            movementX = -speed;
            if (!isGrounded)
                movementX = 1.3f * (-speed);
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            // spriteRenderer.flipX = true;
        }
        else
        {
            movementX = speed;
            if (!isGrounded)
                movementX = 1.3f * speed;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            // spriteRenderer.flipX = false;
        }
    }
    public void TeleportToPlayer()
    {
        transform.position = player.transform.position;
    }

    virtual public void jump()
    {
        // animator.SetBool(IsRunning, false);
        if (isGrounded)
        {
            idle();
            // spriteRenderer.flipX = !isRight;
            rigidbody.velocity = new Vector2(movementX, jumpForce);
        }

    }

    virtual public void AirControl()
    {
        movingToPlayer(player.transform);
    }

    public void TakeDamage(int damage)
    {
        
    }

}
