using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossSpawnTrigger : MonoBehaviour
{
    public Ghost_king_boss boss;
    public Transform spawnPos;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var hp = GameObject.FindGameObjectWithTag("KingHPBar");
            for(var i =0; i<3;i ++)
            {
                hp.transform.GetChild(i).gameObject.SetActive(true);
            }
            boss.thronePosition = spawnPos;
            Instantiate(boss, spawnPos.position, Quaternion.identity);
            Destroy(this);
        }
    }
}
