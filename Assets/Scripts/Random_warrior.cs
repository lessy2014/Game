using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Random_warrior : Entity
{
    public float groundRadius;
    public Transform groundCheck;
    public Transform rightWallCheck;

    public GameObject slownessApplier;

    [SerializeField] private bool isGrounded;
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
    public int damage = 10;
    public int randomModifier = 3;
    public bool hyperArmor;
    [SerializeField] private float speed = 5;
    [SerializeField] private int patrolRadius = 5;
    [SerializeField] private bool movingRight;
    public Transform homePoint;
    public Transform player;
    public float stoppingDistance;

    
    private bool readyToAttack = true;
    private bool onSelf = false;
    private bool preparingAttack = false;
    public Transform swordAttackPosition;
    public Transform greatSwordAttackPosition;
    public Transform lanceAtttackPosition;
    public Transform rightAttackPosition;
    public float attackRange = 1;
    public LayerMask players;
    public bool isDead;

    private static readonly int IsDying = Animator.StringToHash("isDying");
    private static readonly int SwordAttack = Animator.StringToHash("Attack1");
    private static readonly int GreatSwordAttack = Animator.StringToHash("Attack2");
    private static readonly int LanceAttack = Animator.StringToHash("Attack3");
    private static readonly int BeforeSwordAttack = Animator.StringToHash("BeforeAttack1");
    private static readonly int BeforeGreatSwordAttack = Animator.StringToHash("BeforeAttack2");
    private static readonly int BeforeLanceAttack = Animator.StringToHash("BeforeAttack3");

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
    }

    public override void FlyFromTornado()
    {
        rigidbody.velocity = new Vector2(0, 10);
        DisableMovement(1f);
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
        jump = Physics2D.OverlapCircle(rightWallCheck.position, groundRadius, layerGrounds);
        var distanceToPlayer = Math.Abs(player.transform.position.x - transform.position.x);
        if (distanceToPlayer < 3 && !preparingAttack && enabled && readyToAttack && !gotDamage)
            StartCoroutine(WaitBeforeAttack());
    }

    IEnumerator WaitBeforeAttack()
    {
        preparingAttack = true;
        var id = Random.Range(0, randomModifier);
        var attack = 0;
        var waitBeforeAttack = 0f;
        if (id == 0)
        {
            attack = SwordAttack;
            id = BeforeSwordAttack;
            rightAttackPosition = swordAttackPosition;
            attackRange = 1;
            waitBeforeAttack = 0.5f;
            damage = 10;
        }

        if (id == 1)
        {
            attack = GreatSwordAttack;
            id = BeforeGreatSwordAttack;
            rightAttackPosition = greatSwordAttackPosition;
            attackRange = 1.5f;
            waitBeforeAttack = 0.75f;
            damage = 50;
            hyperArmor = true;
        }

        if (id == 2)
        {
            attack = LanceAttack;
            id = BeforeLanceAttack;
            rightAttackPosition = lanceAtttackPosition;
            attackRange = 1.3f;
            waitBeforeAttack = 0.65f;
            damage = 30;
        }
        animator.Play(id);
        yield return new WaitForSeconds(waitBeforeAttack);
        var distanceToPlayer = Math.Abs(player.transform.position.x - transform.position.x);
        if (distanceToPlayer < 3)
        {
            yield return new WaitForSeconds(0.5f);
            distanceToPlayer = Math.Abs(player.transform.position.x - transform.position.x);
            if (distanceToPlayer < 3)
            {
                onSelf = false;
                Attack(attack);
            }
            else
                preparingAttack = false;
        }
        else
        {
            preparingAttack = false;
            animator.Play("Run");
        }
        
    }

    private void Update()
    {
        // spriteRenderer.flipX = movingRight;
        if (!movingRight)
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (Vector2.Distance(transform.position, player.position) < stoppingDistance && readyToAttack)
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

            readyToAttack = false;
            StartCoroutine(AttackAnimation(id));
            StartCoroutine(AttackCoolDown());
        }
    }
    private void OnAttack()
    {
        var enemiesOnHit = Physics2D.OverlapCircleAll(rightAttackPosition.position, attackRange, players);
            foreach (var enemy in enemiesOnHit)
            {
                enemy.GetComponent<Player>().TakeDamage(damage);
            }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(rightAttackPosition.position, attackRange);
    }

    IEnumerator AttackAnimation(int id)
    {
        yield return new WaitForSeconds(0.1f);
        animator.Play(id);
        hyperArmor = false;
        // animator.SetBool(id, false);
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
        if (!hyperArmor)
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
        animator.Play("Death");
        enabled = false;
        Destroy(slownessApplier);
        StopAllCoroutines();
    }

    private void TotallyDead()
    {
        Destroy(gameObject);
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
            movementY = 0.5f;
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
            movementY = 0.5f;
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

    private void JustAttacker() => preparingAttack = false;
}
