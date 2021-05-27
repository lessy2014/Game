using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class TrashMonstersSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawnPoint;
    public TrashMonster monster;
    public bool canSpawn = true;
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
        if (canSpawn)
        {
            canSpawn = false;
            StartCoroutine(SpawnMonsters());
        }
    }
    private IEnumerator SpawnMonsters()
    {
        for (var i = 0; i < 10; i++)
        {
            monster.homePoint = spawnPoint.transform;
            Instantiate(monster, spawnPoint.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine(ActionCooldown());
    }

    private IEnumerator ActionCooldown()
    {
        yield return new WaitForSeconds(6f);
        canSpawn = true;
    }
}
