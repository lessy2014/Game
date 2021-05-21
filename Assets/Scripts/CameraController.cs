using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Player player;
    private Camera camera;
    private Vector3 position;

    private void Awake()
    {
        if (!player)
        {
            player = FindObjectOfType<Player>();
        }
        camera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (player.isWithSword && camera.orthographicSize < 5)
        {
            position.y += 2;
            camera.orthographicSize += Time.deltaTime * 4;
        }
        else if (camera.orthographicSize > 3)
        {
            position.y += 5;
            camera.orthographicSize -= Time.deltaTime * 4;
        }
           
        position = player.transform.position;
        position.z = -10f;
        
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime);
    }
}
