using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 position;

    private void Awake()
    {
        if (!player)
        {
            player = FindObjectOfType<Player>().transform;
        }
    }

    void Update()
    {
        position = player.position;
        position.z = -10f;
        position.y += 2;
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime);
    }
}
