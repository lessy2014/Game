using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Koldun : Support
{
    public GameObject magicBall;
    public AudioSource sound;
    public AudioClip fireballSound;
    
    public Transform hatPos;
    public Transform magicBallPos;
    
    public bool jumped;
    public bool isAtacking;
    
    public float offset = - 45;
    
    public Quaternion rotation;
    
    public static Koldun Instance;

    public void Awake()
    {
        GetComponents();
        sound = gameObject.GetComponentInChildren<AudioSource>();
        Instance = this;
        animator.Play("get_back_mage");
    }
    
    public override void FixedUpdate()
    {
        if (!isFollowPlayer) return;
        State();
        rigidbody.velocity = new Vector2(movementX, rigidbody.velocity.y);
        animator.SetBool(IsRunning, movementX != 0);
        
        if (!isGrounded || !jumped) return;
        animator.SetBool(IsJumping, false);
        jumped = false;
    }

    public void Update()
    {
        if ((!Input.GetKeyDown(KeyCode.F) && !Input.GetKeyDown(KeyCode.F)) || isAtacking || !isFollowPlayer) return;
        isAtacking = true;
        StartCoroutine(AttackCooldown());
        var difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - magicBallPos.position;
        Attack(difference);
    }

    public void Attack(Vector3 direction)
    {
        var roatZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation = Quaternion.Euler(0, 0, roatZ + offset);
        animator.Play("do_magic_mage");
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(3f);
        isAtacking = false;
    }
    public void Magic()
    {
        Instantiate(magicBall, magicBallPos.position, rotation);
    }

    public override void jump()
    {
        print("here");
        rigidbody.velocity = new Vector2(0, movementY);
        movementX = 0;
        animator.SetBool(IsJumping, isFollowPlayer);
        jumped = true;
    }

    public override void AirControl()
    {
        rigidbody.velocity = new Vector2(0, movementY);
        movementX = 0;
        animator.SetBool(IsJumping, isFollowPlayer);
        jumped = true;
    }

    public void TeleportToPlayer()
    {
        print("teleported");
        transform.position = player.transform.position;
    }

    public void Smoke()
    {
        Instantiate(smoke, transform.position, Quaternion.Euler(0, 0, 0));
    }

    public void PlayFireBallSound()
    {
        sound.PlayOneShot(fireballSound);
    }
    
}
