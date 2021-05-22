using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public float speed = 10;
    public float distance = 0;
    public new Rigidbody2D rigidbody;
    public Collider2D collider2D;
    public Animator animator;
    public Vector3 startPosition;
    public static readonly int IsEnd = Animator.StringToHash("Ended");
    // Start is called before the first frame update
    void Start()
    {
        startPosition = gameObject.transform.position;
        animator = gameObject.GetComponent<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        collider2D = gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.velocity = transform.right * speed;
        if (Math.Abs(startPosition.x - transform.position.x) < 5)
        {
            animator.SetBool(IsEnd, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            other.gameObject.GetComponent<Entity>().GetDamage(50);
            other.gameObject.GetComponent<Entity>().FlyFromTornado();
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.layer == 9)
    //     {
    //         other.gameObject.GetComponent<Entity>().GetDamage(50);
    //         Destroy(gameObject);
    //     }
    //     else if (other.gameObject.layer == 8 )
    //     {
    //         Destroy(gameObject);
    //     }
    // }
}
