using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrashMonster : Entity
{
    public float groundRadius;
    public Transform groundCheck;
    public Transform rightWallCheck;
    public Transform leftWallCheck;

    public GameObject slownessApplier;

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

    public float hp = 100;
    [SerializeField] private float speed = 5;
    [SerializeField] private int patrolRadius = 5;
    [SerializeField] private bool movingRight;
    public Transform homePoint;
    public Transform player;
    public float stoppingDistance;
    [SerializeField] private int layer;
    private static readonly int IsDead = Animator.StringToHash("isDead");

    private void Awake()
    {
        GetComponents();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(RandomSpeed());
    }
    private void GetComponents()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        layer = this.gameObject.layer;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        animator.SetBool(IsDead, true);
        animator.SetTrigger("Dying");
        Debug.Log("You killed me dude");
        // boxCollider.enabled = false;
        enabled = false;
        Destroy(slownessApplier);
        // Destroy(gameObject);
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.GetType() == this.GetType())
    //     {
    //         return;
    //     }
    // }
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
        {
            movementX = -speed;
            movingRight = false;
        }
        else
        {
            movementX = speed;
            movingRight = true;
        }

        if (jump && isGrounded)
            movementY = 1.2f;
        else if (isGrounded)
            movementY = 0.4f;
        else
            movementY = 0;
    }
    
    
    IEnumerator RandomSpeed()
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
        spriteRenderer.flipX = movingRight;

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
