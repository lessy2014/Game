using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Trigger : MonoBehaviour
{
    public bool isTriggered = false;

    public GameObject wall;
    public TrashMonster monster;

    public Transform homePoint;
    // Start is called before the first frame update
    void Start()
    {
        monster.homePoint = homePoint;
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
        Instantiate(monster, new Vector3(50, 0.82f, 0), Quaternion.identity);
        wall.SetActive(true);
    }
}
