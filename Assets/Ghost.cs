using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ghost : Entity
{
    public int damage = 10;
    public float speed = 3;
    public float startPosition;
    public Vector3 direction;
    public bool rushIsReady;
    public bool preparingRush;
    public bool angry;
    public bool movingRight;

    private void Start()
    {
        angry = Random.Range(0, 2) == 1;
        movingRight = Random.Range(0, 2) == 1;
    }

    public override void GetDamage(int damage)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 13)
            Destroy(gameObject);
    }
    private void Update()
    {
        direction = Player.Instance.transform.position - gameObject.transform.position;
        if (direction.magnitude <= 0.5f)
        {
            Player.Instance.TakeDamage(damage);
            Destroy(gameObject);
        }
        if (direction.magnitude >= 4)
        {
            if (angry)
                gameObject.transform.position += speed * Time.deltaTime * direction.normalized;
            else
            {
                speed = 2;
                if (movingRight)
                    gameObject.transform.position += speed * Time.deltaTime * Vector3.right;
                else
                    gameObject.transform.position += speed * Time.deltaTime * Vector3.left;
                if (Math.Abs(startPosition - gameObject.transform.position.x) >= 10)
                    Destroy(gameObject);
            }
        }
        else
        {
            if (!rushIsReady && !preparingRush)
                StartCoroutine(WaitBeforeRush());
            if (rushIsReady)
                gameObject.transform.position += speed * Time.deltaTime * direction.normalized ;
        }
    }
    

    private IEnumerator WaitBeforeRush()
    {
        preparingRush = true;
        speed = 0;
        yield return new WaitForSeconds(2f);
        speed = 6;
        rushIsReady = true;
    }
}
