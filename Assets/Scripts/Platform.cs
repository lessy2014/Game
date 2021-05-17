using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D effector;
    private float waitTime = 0.1f;
    private float resetTime = 0.2f;
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            waitTime = 0.1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            if(waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                resetTime = 0.2f;
                waitTime = 0.1f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        if(resetTime > 0)
            resetTime -= Time.deltaTime;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || resetTime <= 0)
        {
            effector.rotationalOffset = 0;
        }
    }
}
