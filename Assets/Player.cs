using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 4;
    public float speedMultiplicator = 1;
    public float jumpForce = 7;
    public float groundRadius;
    public Transform groundCheck;
    public Transform cellCheck;
    public LayerMask layerGrounds;

    private bool isGrounded;
    private bool isCelled;
    private bool crouching;
    private bool crouchingUnpressed;
    
    private float movementX;
    
    private new Rigidbody2D rigidbody;
    private new BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    
    private InputMaster input;
    private float currentAxis = 1;

    public static Player Instance;
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsCrouching = Animator.StringToHash("isCrouching");
    private static readonly int IsFalling = Animator.StringToHash("isFalling");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    private void Awake()
    {
        GetComponents();
        Instance = this;
        input = new InputMaster();
        BindMovement();
    }

    private void BindMovement()
    {
        input.Player.Move.performed += context => Move(context.ReadValue<float>());
        input.Player.Move.canceled += context => Move(0);
        input.Player.Jump.performed += context => Jump();
        input.Player.Crouch.performed += context =>
        {
            crouchingUnpressed = false;
            Crouch();
        };
        input.Player.Crouch.canceled += context =>
        {
            crouchingUnpressed = true;
            Uncrouch();
        };
    }

    private void GetComponents()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (crouchingUnpressed)
            Uncrouch();
        animator.SetBool(IsJumping, rigidbody.velocity.y > 0);
        animator.SetBool(IsFalling, rigidbody.velocity.y < 0);
        animator.SetBool(IsCrouching, crouching);
    }

    private void Move(float axis)
    {
        if (axis != 0)
            spriteRenderer.flipX = axis < 0;
        movementX = axis * speed;
        animator.SetBool(IsRunning, movementX != 0);
    }

    private void Crouch()
    {
        if (crouching)
            return;
        
        crouching = true;
        
        // Райдер пишет, что последовательный доступ к полям компонента неэффективен
        var size = boxCollider.size;
        size = new Vector2(size.x, size.y / 2 - 0.2f);
        boxCollider.size = size;
        
        var offset = boxCollider.offset;
        offset = new Vector2(offset.x, offset.y * 2);
        boxCollider.offset = offset;
    }

    private void Uncrouch()
    {
        if (isCelled || !crouching)
            return;
        
        crouching = false;
        
        var size = boxCollider.size;
        size = new Vector2(size.x, (size.y + 0.2f) * 2);
        boxCollider.size = size;
        
        var offset = boxCollider.offset;
        offset = new Vector2(offset.x, offset.y / 2);
        boxCollider.offset = offset;
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

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.layer == 9)
    //     {
    //         speed *= 0.8f;
    //         
    //     }
    // }
    //
    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     if (other.gameObject.layer == 9)
    //         speed *= 1.25f;
    // }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(movementX, rigidbody.velocity.y);
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, layerGrounds);
        isCelled = Physics2D.OverlapCircle(cellCheck.position, groundRadius, layerGrounds);
    }

    private void Jump()
    {
        if (!isGrounded)
            return;
        
        rigidbody.velocity = new Vector2(movementX, jumpForce);
    }
    
    private void OnEnable() => input.Enable();

    private void OnDisable() => input.Disable();
}
