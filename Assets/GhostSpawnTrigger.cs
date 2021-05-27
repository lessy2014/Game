using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class GhostSpawnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawnPoint;
    public Ghost ghost;
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
            StartCoroutine(SpawnGhosts());
        }
    }
    private IEnumerator SpawnGhosts()
    {
        for (var i = 0; i < 10; i++)
        {
            Instantiate(ghost, spawnPoint.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(ActionCooldown());
    }

    private IEnumerator ActionCooldown()
    {
        yield return new WaitForSeconds(10f);
        canSpawn = true;
    }
}
