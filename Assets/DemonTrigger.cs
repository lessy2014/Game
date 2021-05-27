using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public Demon demon;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (Demon.Instance.hpToTeleport >= 100)
            Demon.Instance.readyToFight = true;
        else
            Demon.Instance.readyToFight = false;
    }
}
