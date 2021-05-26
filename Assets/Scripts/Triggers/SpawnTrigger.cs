using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isTriggered;
    public Random_warrior randomWarior;
    public GameObject spawnPoint;
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
        randomWarior.homePoint = spawnPoint.transform;
        Instantiate(randomWarior, spawnPoint.transform.position, Quaternion.identity);
    }
}
