using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow_fly : MonoBehaviour
{
    public float speed = 10;
    public float distance = 0;
    public new Rigidbody2D rigidbody;
    public CapsuleCollider2D Collider2D;
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        Collider2D = gameObject.GetComponent<CapsuleCollider2D>();
        StartCoroutine(SelfDestroyAfterTime());
    }
    void FixedUpdate()
    {
        rigidbody.velocity = transform.right * speed;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 9)
        {
            other.gameObject.GetComponent<Entity>().GetDamage(50);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == 8 )
        {
            Destroy(gameObject);
        }
    }
    
    IEnumerator SelfDestroyAfterTime()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
