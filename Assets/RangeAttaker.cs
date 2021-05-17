using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttaker : MonoBehaviour
{
    public Transform groundCheck;
    public Transform rightWallCheck;
    public Transform leftWallCheck;

    public bool isGrounded;
    public bool isMovingRight;
    public bool isStuck;
    
    private float groundRadius = 0.3f;
    public float speed = 0.2f;
    private float attackRange = 10;
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

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponents();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < attackRange)
            Angry();
        else
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

    private void Angry()
    {
        
    }
}
