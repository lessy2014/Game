using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Entity
{

    private float hp = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == Player.Instance.gameObject)
        {
            hp--;
        }

        if(hp < 1)
        {
            Die();
        }
    }
}
