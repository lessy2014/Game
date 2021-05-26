using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnTrigger : MonoBehaviour
{
    public Ghost_king_boss boss;
    public Transform spawnPos;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.thronePosition = spawnPos;
            Instantiate(boss, spawnPos.position, Quaternion.identity);
            Destroy(this);
        }
    }
}
