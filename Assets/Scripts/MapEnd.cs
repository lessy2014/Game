using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnd : MonoBehaviour
{
    private MapStart start;
    
    public double distanceToStart;

    void Start()
    {
        start = FindObjectOfType<MapStart>();
    }
    
}
