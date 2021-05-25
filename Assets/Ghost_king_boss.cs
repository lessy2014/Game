using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_king_boss : Entity
{
    public int damage = 10;
    public float speed = 3;
    public float hp = 1000;
    public Transform thronePosition;
    public Vector3 direction;
    public bool solidSnake;
    public bool canChangeAction = true;
    public bool cameToThrone;
    public bool readyToNewRush;
    public int currentAction;
    public int attackCounter;
    public int rushCounter = 0;
    public int rushConstant = 3;
    public GameObject ghost;
    
    private BoxCollider2D collider;
    private Animator animator;

    void Start()
    {
        GetComponents();
    }
    
    private void GetComponents()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canChangeAction)
        {
            if (attackCounter<3)
                currentAction = Random.Range(0, 3);
            else
            {
                currentAction = 3;
            }
        }

        if (currentAction == 0)
            RushAttack();
        else if (currentAction == 1)
            RangeAttack();
        else if (currentAction == 2)
            SendGhosts();
        else if (currentAction == 3)
            Tragedy();
    }

    void RushAttack()
    {
        if (canChangeAction)
        {
            speed = 0;
            canChangeAction = false;
            direction = Player.Instance.transform.position - gameObject.transform.position;
            direction += 10 * direction.normalized; 
            SetDirection(direction.x);
            animator.Play("ToRushTransition");
            attackCounter++;
        }
        else if (readyToNewRush)
        {
            speed = 0;
            direction = Player.Instance.transform.position - gameObject.transform.position;
            direction += 10 * direction.normalized; 
            SetDirection(direction.x);
            animator.Play("ToRushTransition");
        }
        if (direction.magnitude >= 0.5 && !readyToNewRush)
        {
            speed = 10;
            gameObject.transform.position += speed * Time.deltaTime * direction.normalized;
            direction -= speed * Time.deltaTime * direction.normalized;
        }
        else if (direction.magnitude < 0.5)
        {
            rushCounter++;
            EndedRush();
        }

        if (rushCounter == rushConstant)
        {
            animator.Play("Idle");
            speed = 3;
            canChangeAction = true;
            rushCounter = 0;
        }

    }

    void RangeAttack()
    {
        if (canChangeAction)
        {
            StartCoroutine(ActionCooldown(3f));
            animator.Play("RangeAttack");
            attackCounter++;
        }
    }

    void SendGhosts()
    {
        if (canChangeAction)
        {
            StartCoroutine(ActionCooldown(3f));
            animator.Play("SendGhosts");
            attackCounter++;
        }
    }

    void Tragedy()
    {
        canChangeAction = false;
        attackCounter = 0;
        direction = thronePosition.transform.position - gameObject.transform.position;
        SetDirection(direction.x);
        if (direction.magnitude >= 1)
        {
            gameObject.transform.position += speed * Time.deltaTime * direction.normalized;
            cameToThrone = false;
        }
        else if (!cameToThrone )
        {
            cameToThrone = true;
            animator.Play("Tragedy");
            StartCoroutine(ActionCooldown(3f));
        }
    }
    
    public override void GetDamage(int damage)
    {
        if (solidSnake)
            hp -= damage;
        else
            hp += damage;
        if (hp < 0)
        {
            animator.Play("Death");
            collider.enabled = false;
            Destroy(this);
        }
    }

    private IEnumerator ActionCooldown(float length)
    {
        canChangeAction = false;
        yield return new WaitForSeconds(length);
        cameToThrone = false;
        canChangeAction = true;
    }

    private IEnumerator SpawnGhosts(float quantity)
    {
        for (var i = 0; i < quantity; i++)
        {
            Instantiate(ghost, gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SetDirection(float x)
    {
        if (x > 0)
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void Solid() => solidSnake = true;

    private void Ghost() => solidSnake = false;

    private void InRush() => readyToNewRush = false;

    private void EndedRush() => readyToNewRush = true;


}
