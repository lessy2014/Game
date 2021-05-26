using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow_fly : MonoBehaviour
{
    public float speed = 10;
    public new Rigidbody2D rigidbody;
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(SelfDestroyAfterTime());
    }
    void FixedUpdate()
    {
        rigidbody.velocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 14)
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
