using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Opossum : Entity
{
    public float groundRadius;
    public Transform groundCheck;
    public Transform rightWallCheck;
    public Transform leftWallCheck;
    private bool isGrounded;
    [SerializeField] private bool angry;
    [SerializeField] private bool jump;
    public LayerMask layerGrounds;
    private float movementX;
    private float movementY;
    
    private new Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    
    [SerializeField] private float speed = 5;
    [SerializeField] private int patrolRadius = 5;
    [SerializeField] private bool movingRight;
    public Transform homePoint;
    public Transform player;
    public float stoppingDistance;

    private void Awake()
    {
        GetComponents();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(OpossumSpeed());
    }
    private void GetComponents()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetType() == this.GetType())
        {
            return;
        }
    }
    private void Idle()
    {
        if (transform.position.x > homePoint.position.x + patrolRadius)
        {
            movingRight = false;
            
        }
        if (transform.position.x <= homePoint.position.x - patrolRadius)
        {
            movingRight = true;
        }

        if (movingRight)
            movementX = speed;
        else
            movementX = -speed;

        if (jump && isGrounded)
            movementY = 1.2f;
        else
            movementY = 0;
    }

    private void Angry()
    {
        var delta = transform.position.x - player.transform.position.x - 0.7;
        if (delta < 0.3 && delta > 0)
            movementX = 0;
        else if (delta > 0)
            movementX = -speed;
        else 
            movementX = speed;

        if (jump && isGrounded)
            movementY = 1.2f;
        else if (isGrounded)
            movementY = 0.4f;
        else
            movementY = 0;
    }
    
    
    IEnumerator OpossumSpeed()
    {
        for(; ; )
        {
            speed = Random.Range(3.0f, 4.0f);
            yield return new WaitForSeconds(1);
        }
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(movementX, rigidbody.velocity.y + movementY);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, layerGrounds);
        jump = Physics2D.OverlapCircle(rightWallCheck.position, groundRadius, layerGrounds) || Physics2D.OverlapCircle(leftWallCheck.position, groundRadius, layerGrounds);
    }

    private void Update()
    {

        if(Vector2.Distance(transform.position, player.position) < stoppingDistance)
        {
            // print("angry");
            angry = true;
            Angry();
        }
        else
        {
            // print("idle");
            angry = false;
            Idle();
        }
    }
}
