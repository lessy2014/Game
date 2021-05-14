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
        rigidbody.velocity = new Vector2( movementX, rigidbody.velocity.y);
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
        rigidbody.velocity = new Vector2(0, movementY);
        movementX = 0;
        animator.SetBool(IsJumping, true);
        jumped = true;
    }

    public override void AirControl()
    {
        rigidbody.velocity = new Vector2(0, movementY);
        movementX = 0;
        animator.SetBool(IsJumping, true);
        jumped = true;
    }

    public void TeleportToPlayer()
    {
        transform.position = player.transform.position;
        print("teleported");
    }
    
}
