using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnForg : MonoBehaviour
{
    public GameObject forg;

    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(SpawnForgs());
    }

    private IEnumerator SpawnForgs()
    {
        while (true)
        {
            Instantiate(forg, new Vector3(Random.Range(-25f, 27f), -1.7f, 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            yield return new WaitForSeconds(0.7f);
        }
    }
}
