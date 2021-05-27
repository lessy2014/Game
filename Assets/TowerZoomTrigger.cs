using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerZoomTrigger : MonoBehaviour
{
    public bool isTriggered = false;

    public GameObject zoomObject;
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
        CameraController.Instance.size = 10;
        if (!other.CompareTag("Player") || isTriggered)
            return;
        isTriggered = true;
        CameraController.Instance.objectInFocus = zoomObject;
        CameraController.Instance.isFocused = true;
    }
}
