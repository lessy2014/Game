using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow_fly : MonoBehaviour
{
    public float speed = 10;
    public float distance = 0;
    public new Rigidbody2D rigidbody;
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(SelfDestroyAfterTime());
    }
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance);
        if (!(hitInfo.collider is  null))
        {
            if (hitInfo.collider.gameObject.layer == 9)
            {
                hitInfo.collider.gameObject.GetComponent<Entity>().GetDamage(50);
                Destroy(gameObject);
            }
            else if (hitInfo.collider.gameObject.layer == 8)
            {
                Destroy(gameObject);
            }
        }

        rigidbody.velocity = transform.right * speed;
    }
    
    IEnumerator SelfDestroyAfterTime()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
