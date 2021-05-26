using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Magic : MonoBehaviour
{
    public float speed = 4;
    public int damage = 10;
    public float existTime = 4f;
    public Vector3 direction;
    public new Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        direction = new Vector3(Random.Range(-1f, 1f),Random.Range(-1f, 1f), 0);
        var roatZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, roatZ);
        StartCoroutine(Initiation());
        StartCoroutine(SelfDestroyAfterTime());
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            Player.Instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator Initiation()
    {
        yield return new WaitForSeconds(1f);
        speed = 10;
        direction = Player.Instance.transform.position - gameObject.transform.position;
        var roatZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, roatZ);
    }

    private IEnumerator SelfDestroyAfterTime()
    {
        yield return new WaitForSeconds(existTime);
    }
}
