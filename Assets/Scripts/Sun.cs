using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    private MapEnd end;
    private MapStart start;

    private Camera camera;
    private Player player;

    private double step;
    private double angle;
    private double farestPlayerPosition;
    void Start()
    {
        end = FindObjectOfType<MapEnd>();
        start = FindObjectOfType<MapStart>();
        camera = Camera.main;
        player = FindObjectOfType<Player>();
        step = end.distanceToStart / 180;
    }
    
    void Update()
    {
        var position = camera.transform.position;
        transform.position = new Vector3(transform.position.x + (position.x - transform.position.x) * Time.deltaTime, transform.position.y);
    }
}
