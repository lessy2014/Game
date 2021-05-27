using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isTriggered;
    public TrashMonster trashmonster;
    public GameObject spawnPoint;
    public int count;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SpawnMonsters()
    {
        for (var i = 0; i < count; i++)
        {
            trashmonster.homePoint = spawnPoint.transform;
            Instantiate(trashmonster, spawnPoint.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isTriggered)
            return;
        isTriggered = true;
        StartCoroutine(SpawnMonsters());
    }
}
