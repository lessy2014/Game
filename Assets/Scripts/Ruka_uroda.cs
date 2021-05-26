using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruka_uroda : MonoBehaviour
{
    public int damage = 10;
    public BoxCollider2D collider;
    void Start()
    {
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void EnableCollider()
    {
        collider.enabled = true;
    }

    private void DisableCollider()
    {
        collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
            other.GetComponent<Player>().TakeDamage(damage);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
