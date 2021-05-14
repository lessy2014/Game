using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koldun : Support
{

    public void Awake()
    {
        GetComponents();
        Instance = this;
    }
    
}
