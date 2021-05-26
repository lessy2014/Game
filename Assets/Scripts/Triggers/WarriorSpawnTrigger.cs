using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class WarriorSpawnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawnPoint;
    public Random_warrior warrior;
    public bool canSpawn = true;
    public int count;
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
            StartCoroutine(SpawnWarriors());
        }
    }
    private IEnumerator SpawnWarriors()
    {
        for (var i = 0; i < count; i++)
        {
            warrior.homePoint = spawnPoint.transform;
            Instantiate(warrior, spawnPoint.transform.position, Quaternion.identity);
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
