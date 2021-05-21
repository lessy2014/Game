using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementZoomTrigger : MonoBehaviour
{
    public bool isTriggered = false;

    public GameObject wall;
    public GameObject basement;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isTriggered)
            return;
        isTriggered = true;
        CameraController.Instance.objectInFocus = basement;
        CameraController.Instance.isFocused = true;
    }
}
