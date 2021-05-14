using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Koldun : Support
{
    public bool jumped;
    public static Koldun Instance;

    public override void Awake()
    {
        GetComponents();
        Instance = this;
    }
    
    public override void FixedUpdate()
    {
        State();
        animator.SetBool(IsRunning, movementX != 0);
        if (isGrounded && jumped)
        {
            animator.SetBool(IsJumping, false);
            jumped = false;
            print("fallen");
        }

    }

    public override void jump()
    {
        animator.SetBool(IsJumping, true);
            jumped = true;
            print("jumped");
    }

    public override void AirControl()
    {
        animator.SetBool(IsJumping, true);
        jumped = true;
        print("flying");
    }

    public void TeleportToPlayer()
    {
        transform.position = player.transform.position;
        print("teleported");
    }
    
}
