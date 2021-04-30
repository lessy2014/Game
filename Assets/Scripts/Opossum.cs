using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : Entity
{
    [SerializeField] private float speed = 5;
    [SerializeField] private int patrolRadius = 5;
    [SerializeField] private bool movingRight;
    public Transform homePoint;
    public Transform player;
    public float stoppingDistance;
    [SerializeField] bool idle;
    [SerializeField] bool angry;
    [SerializeField] bool backHome;
    [SerializeField] double opossum_homePoint_dist;
    [SerializeField] double opossum_player_dist;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine("");
    }
    private void Idle()
    {
        if (transform.position.x > homePoint.position.x + patrolRadius)
        {
            movingRight = false;
            
        }
        if (transform.position.x <= homePoint.position.x - patrolRadius)
        {
            movingRight = true;
        }
            
        if (movingRight)
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        else
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
    }

    private void Angry()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed*Time.deltaTime);
    }

    private void BackHome()
    {
        transform.position = Vector2.MoveTowards(transform.position, homePoint.position, speed * Time.deltaTime);
    }

    private void Update()
    {
        opossum_homePoint_dist = Vector2.Distance(transform.position, homePoint.position);
        opossum_player_dist = Vector2.Distance(transform.position, player.position);
        idle = true;
        if (Vector2.Distance(transform.position, homePoint.position) < patrolRadius && !angry)
        {
            angry = false;
            idle = true;
            print("idle");
            backHome = false;
        }

        else if(Vector2.Distance(transform.position, player.position) < stoppingDistance)
        {
            angry = true;
            print("angry");
            idle = false;
            backHome = false;
        }

        else if (Vector2.Distance(transform.position, homePoint.position) > stoppingDistance)
        {
            idle = false;
            backHome = true;
            print("backHome");
            angry = false;
        }

        if (idle)
            Idle();
        else if (angry)
            Angry();
        else if (backHome)
            BackHome();
    }
}
