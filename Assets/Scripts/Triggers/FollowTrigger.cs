using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTrigger : MonoBehaviour
{

    public Support follower;
    public bool isTriggered;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isTriggered)
            return;
        
        isTriggered = true;
        follower.isFollowPlayer = true;
        follower.SetToPlayerPosition();

    }
}
