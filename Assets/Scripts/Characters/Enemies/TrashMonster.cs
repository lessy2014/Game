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
    // [SerializeField] private bool angry;
    [SerializeField] private bool jump;
    private bool gotDamage = false;
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
    // [SerializeField] private int layer;
    

    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    private static readonly int IsOnSelfAttack = Animator.StringToHash("isOnSelfAttack");
    // private bool isAttacking = false;
    private bool readyToAttack = true;
    private bool onSelf = false;
    private bool preparingAttack = false;
    public Transform rightAttackPosition;
    public Transform leftAttackPosition;
    public Transform onSelfAttackPosition;
    public float attackRange = 1;
    public LayerMask players;
    public bool isDead;

    private static readonly int IsDying = Animator.StringToHash("isDying");

    private void Awake()
    {
        GetComponents();
        readyToAttack = true;
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
        // layer = this.gameObject.layer;
    }


    private void FixedUpdate()
    {
        if (preparingAttack)
            movementX = 0;
        if (gotDamage && movingRight)
            movementX = -speed;
        else if (gotDamage && !movingRight)
            movementX = speed;
        rigidbody.velocity = new Vector2(movementX, rigidbody.velocity.y + movementY);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, layerGrounds);
        jump = Physics2D.OverlapCircle(rightWallCheck.position, groundRadius, layerGrounds) || Physics2D.OverlapCircle(leftWallCheck.position, groundRadius, layerGrounds);
        var distanceToPlayer = Math.Abs(player.transform.position.x - transform.position.x);
        if (distanceToPlayer < 3 && !preparingAttack && enabled)
            StartCoroutine(WaitBeforeAttack());
    }

    IEnumerator WaitBeforeAttack()
    {
        preparingAttack = true;
        yield return new WaitForSeconds(0.5f);
        var distanceToPlayer = Math.Abs(player.transform.position.x - transform.position.x);
        if (distanceToPlayer < 3)
        {
            yield return new WaitForSeconds(0.5f);
            distanceToPlayer = Math.Abs(player.transform.position.x - transform.position.x);
            if (distanceToPlayer < 3 && distanceToPlayer > 1.5)
            {
                onSelf = false;
                Attack(IsAttack);
            }
            else if (distanceToPlayer <= 1.5)
            {
                onSelf = true;
                Attack(IsOnSelfAttack);
            }
        }

        preparingAttack = false;
    }

    private void Update()
    {
        spriteRenderer.flipX = movingRight;

        if (Vector2.Distance(transform.position, player.position) < stoppingDistance)
        {
            Angry();
        }
        else
        {
            Idle();
        }
    }


    private void Attack(int id)
    {
        if (readyToAttack)
        {
            animator.SetBool(id, true);
            // isAttacking = true;
            readyToAttack = false;

            StartCoroutine(AttackAnimation(id));
            StartCoroutine(AttackCoolDown());
        }
    }

    private void JumpOnPlayer()
    {
        if (movingRight)
            gameObject.transform.position = rightAttackPosition.position;
        else
            gameObject.transform.position = leftAttackPosition.position;
    }

    private void JumpFromPlayer()
    {
        if (movingRight)
            gameObject.transform.position = leftAttackPosition.position;
        else
            gameObject.transform.position = rightAttackPosition.position;
    }

    private void OnAttack()
    {
        if (onSelf)
        {
            var enemiesOnHit = Physics2D.OverlapCircleAll(onSelfAttackPosition.position, attackRange, players);
            foreach (var enemy in enemiesOnHit)
            {
                enemy.GetComponent<Player>().TakeDamage(10);
            }
            onSelf = false;
        }
        else if (movingRight)
        {
            var enemiesOnHit = Physics2D.OverlapCircleAll(rightAttackPosition.position, attackRange, players);
            foreach (var enemy in enemiesOnHit)
            {
                enemy.GetComponent<Player>().TakeDamage(10);
            }
        }
        else
        {
            var enemiesOnHit = Physics2D.OverlapCircleAll(leftAttackPosition.position, attackRange, players);
            foreach (var enemy in enemiesOnHit)
            {
                enemy.GetComponent<Player>().TakeDamage(10);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(rightAttackPosition.position, attackRange);
        Gizmos.DrawWireSphere(leftAttackPosition.position, attackRange);
    }

    IEnumerator AttackAnimation(int id)
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool(id, false);
        // isAttacking = false;
    }

    IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(3f);
        readyToAttack = true;
    }

    public override void GetDamage(int damage)
    {
        preparingAttack = false;
        hp -= damage;
        DoKnockBack();

        if (hp <= 0 && !isDead)
        {
            isDead = true;
            Death();
        }
    }
    
    public void DoKnockBack()
    {
        StartCoroutine(DisableMovement(1f));
        rigidbody.velocity = new Vector2(0, 5f);
    }
    IEnumerator DisableMovement(float time)
    {
        gotDamage = true;
        yield return new WaitForSeconds(time);
        gotDamage = false;
    }

    private void Death()
    {
        animator.SetBool(IsDying, true);
        enabled = false;
        Destroy(slownessApplier);
        StopAllCoroutines();
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
        var delta = transform.position.x - player.transform.position.x;
        if ((delta < 2 && delta > 0)
            ||(delta > -2 && delta < 0))
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
        else if (isGrounded && !preparingAttack)
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
}
