using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject music;
    bool isTriggered;
    public AudioClip newMusic;
    private AudioSource audio;
    void Start()
    {
        music = GameObject.FindGameObjectWithTag("Music");
        audio = music.GetComponent<AudioSource>();
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
        audio.clip = newMusic;
        audio.Play();
    }
}
