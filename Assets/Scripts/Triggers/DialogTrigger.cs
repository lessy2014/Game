using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.AddComponent(typeof(TextDisplay));
        var a = other.gameObject.GetComponent<TextDisplay>();
        a.textFont = Resources.Load<Font>("Fonts/CoolFont");
        a.text = "test";
        a.fontSize = 40;
        a.labelWidth = 300;
        a.outlineColor = Color.black;
        a.textColor = Color.white;
        a.duration = 5;
        
    }
}
