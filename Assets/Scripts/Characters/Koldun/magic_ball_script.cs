using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic_ball_script : MonoBehaviour
{
    public float speed = 10;
    public new Rigidbody2D rigidbody;
    public Animator animator;
    public CircleCollider2D collider;
    public LayerMask enemies;
    public LayerMask destructibleObjects;
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        collider = gameObject.GetComponent<CircleCollider2D>();
        StartCoroutine(SelfDestroyAfterTime());
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = (transform.up + transform.right) * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8 && other.gameObject.tag != "Platform" || other.gameObject.layer == 9 || other.gameObject.layer == 14)
        {
            speed = 0;
            DealDamage();
            animator.Play("EXPLOSION");
        }
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.layer == 8 && other.gameObject.tag != "Platform" || other.gameObject.layer == 9 || other.gameObject.layer == 14)
    //     {
    //         speed = 0;
    //         DealDamage();
    //         animator.Play("EXPLOSION");
    //     }
    // }

    private void DealDamage()
    {
        var enemiesOnHit = Physics2D.OverlapCircleAll(transform.position, 1f, enemies);
        var objOnHit = Physics2D.OverlapCircleAll(transform.position, 1f, destructibleObjects);
        if (enemiesOnHit.Length != 0)
            foreach(var enemy in enemiesOnHit)
            {
                enemy.GetComponent<Entity>().GetDamage(50);
            }
        if (objOnHit.Length != 0)
            foreach (var obj in objOnHit)
            {
                obj.GetComponent<Entity>().GetDamage(50);
            }
    }

    IEnumerator SelfDestroyAfterTime()
    {
        yield return new WaitForSeconds(3f);
        DealDamage();
        animator.Play("EXPLOSION");
    }

    private void SelfDestroyCollision()
    {
        // Destroy(rigidbody);
        Destroy(collider);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
