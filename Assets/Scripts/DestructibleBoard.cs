using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBoard : Entity
{
    // Start is called before the first frame update
    [SerializeField] private int hp = 100;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void GetDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
            Die();
    }
}
