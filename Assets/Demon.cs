using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Entity
{
    public float groundRadius;
    public Transform groundCheck;
    public Transform rightWallCheck;
    public Transform cagePos;

    public GameObject slownessApplier;

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool teleport;
    private bool gotDamage = false;
    public LayerMask layerGrounds;
    private float movementX;
    private float movementY;
    
    
    private new Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public GameObject rukaUroda;

    public float hp = 1000;
    public static readonly int HpToTeleportConst = 100;
    public float hpToTeleport = 100;
    public int damage = 10;
    public bool hyperArmor;
    [SerializeField] private float speed = 2;
    // [SerializeField] private int patrolRadius = 5;
    [SerializeField] private bool movingRight;
    // public Transform homePoint;
    public Transform player;
    public static Demon Instance;


    private bool readyToAttack = true;
    private bool onSelf = false;
    private bool preparingAttack = false;
    public bool readyToFight = true;
    public Transform attackPosition;
    public float attackRange = 1;
    public LayerMask players;
    public bool isDead;
    
    private static readonly int SimpleAttack = Animator.StringToHash("LongAttack");
    private static readonly int OnSelfAttack = Animator.StringToHash("InOnSelfAttack");
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        GetComponents();
        readyToAttack = true;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void GetComponents()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
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
        teleport = Physics2D.OverlapCircle(rightWallCheck.position, groundRadius, layerGrounds);
        var distanceToPlayer = Math.Abs(player.transform.position.x - transform.position.x);
        if (distanceToPlayer < 10 && !preparingAttack && enabled && readyToAttack && !gotDamage)
            StartCoroutine(WaitBeforeAttack());
    }
    IEnumerator WaitBeforeAttack()
    {
        preparingAttack = true;
        yield return new WaitForSeconds(0.5f);
        var distanceToPlayer = Math.Abs(player.transform.position.x - transform.position.x);
        if (distanceToPlayer < 10)
        {
            yield return new WaitForSeconds(0.5f);
            distanceToPlayer = Math.Abs(player.transform.position.x - transform.position.x);
            if (distanceToPlayer < 10 && distanceToPlayer > 5)
            {
                onSelf = false;
                Attack(SimpleAttack);
            }
            else if (distanceToPlayer <= 5)
            {
                onSelf = true;
                Attack(OnSelfAttack);
            }
        }
        else
            animator.Play("Teleport");

        preparingAttack = false;
    }
    private void Update()
    {
        if (!movingRight)
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (!gotDamage)
            Angry();
    }
    private void Angry()
    {
        var delta = transform.position.x - player.transform.position.x;
        if (Math.Abs(delta) > 10 && readyToAttack)
            animator.Play("Teleport");
        if ((delta < 2 && delta > 0)
            || (delta > -2 && delta < 0))
        {
            movementX = 0;
            if (delta > 0)
                movingRight = false;
            else
                movingRight = true;
        }
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

        if (teleport && isGrounded)
            animator.Play("Teleport");
        else
            movementY = 0;
    }

    private void Telepot()
    {
        StartCoroutine(DisableMovement(3f));
        gameObject.transform.position = cagePos.position;
        if (readyToFight)
            StartCoroutine(TeleportCooldown());
    }

    private IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(3f);
        if (Player.Instance.right)
            gameObject.transform.position = player.transform.position + 2 * Vector3.left + Vector3.up;
        else 
            gameObject.transform.position = player.transform.position + 2 * Vector3.right + Vector3.up;
        animator.Play("TeleportTo");
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
        var enemiesOnHit = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, players);
        foreach (var enemy in enemiesOnHit)
        {
            enemy.GetComponent<Player>().TakeDamage(damage);
        }
    }
    IEnumerator AttackAnimation(int id)
    {
        yield return new WaitForSeconds(0.1f);
        animator.Play(id);
    }

    IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(3f);
        readyToAttack = true;
    }

    private void SendRukuUroda()
    {
        Instantiate(rukaUroda, player.transform.position + 0.8f * Vector3.up + 2 * Vector3.right, Quaternion.Euler(0, 0, 0));
        Instantiate(rukaUroda, player.transform.position + 0.8f * Vector3.up + Vector3.right, Quaternion.Euler(0, 0, 0));
        Instantiate(rukaUroda, player.transform.position + 0.8f * Vector3.up + 3 * Vector3.right, Quaternion.Euler(0, 0, 0));
    }
    public override void GetDamage(int damage)
    {
        preparingAttack = false;
        hp -= damage;
        hpToTeleport -= damage;
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
        if (hpToTeleport <= 0)
        {
            StartCoroutine(DisableMovement(2f));
            animator.Play("Teleport");
            hpToTeleport = HpToTeleportConst;
        }
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
        Instantiate(rukaUroda, transform.position + 3 * Vector3.up, Quaternion.Euler(0, 0, 90));
        Instantiate(rukaUroda, transform.position + 2 * Vector3.down, Quaternion.Euler(0, 0, -90));
        Instantiate(rukaUroda, transform.position + Vector3.up , Quaternion.Euler(0, 0, 90));
        enabled = false;
        Destroy(slownessApplier);
        StopAllCoroutines();
        readyToFight = false;
    }

    private void TotallyDead()
    {
        Destroy(gameObject);
    }
    
}
