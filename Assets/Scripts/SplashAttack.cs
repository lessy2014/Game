using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashAttack : MonoBehaviour
{
    private static int order = 0;
    public float attackRange = 1f;
    public float cleavePower = 5f;
    // public CapsuleCollider2D Collider2D;
    public Animator animator;
    private readonly LayerMask enemies = (1 << 9) | (1 << 14);
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        Splash(order);
    }

    public void Splash(int number)
    {
        if (number == 0)
        {
            order = 1;
            animator.Play("Splash1");
        }
        else if (number == 1)
        {
            order = 0;
            animator.Play("Splash2");
        }
    }
    
    private void OnAttack()
    {
        var enemiesOnHit = Physics2D.OverlapCircleAll(gameObject.transform.position, attackRange, enemies);
        for (var i = 0; i < cleavePower; i++)
        {
            if (i > enemiesOnHit.Length-1) break;
            enemiesOnHit[i].GetComponent<Entity>().GetDamage(50);
            Player.Instance.rage += 20;
            Player.Instance.rageBar.SetHealth(Player.Instance.rage);
            if (Player.Instance.rage >= 100)
            {
                Player.Instance.rageMode = true;
                Player.Instance.rage = 100;
            }
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
