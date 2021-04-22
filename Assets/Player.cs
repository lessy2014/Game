using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 4;
    public float jumpForce = 7;
    public Transform groundCheck;
    public Transform cellCheck;
    public float groundRadius;
    public LayerMask layerGrounds;

    private bool isGrounded;
    private bool isCelled;
    private bool crouching;
    private bool crouchingUnpressed;
    private float movementX;
    private new Rigidbody2D rigidbody;
    private new BoxCollider2D boxCollider;
    private InputMaster input;
    private Animator animator;
    private float currentAxis = 1;
    private SpriteRenderer spriteRenderer;
    public static Player Instance { get; set; }

    private void Awake()
    {
        input = new InputMaster();
        Instance = this;
        
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        
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
    
    void Update()
    {
        if (crouchingUnpressed)
            Uncrouch();
        animator.SetBool("isJumping", rigidbody.velocity.y > 0);
        animator.SetBool("isFalling", rigidbody.velocity.y < 0);
        animator.SetBool("isCrouching", crouching);
    }

    private void Move(float axis)
    {
        if (axis != 0)
            spriteRenderer.flipX = axis < 0;
        movementX = axis * speed;
        animator.SetBool("isRunning", movementX != 0);
    }

    private void Crouch()
    {
        if (crouching)
            return;
        
        crouching = true;
        
        // Райдер пишет, что последовательный доступ к полям компонента неэффективен.
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
