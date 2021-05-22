using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private Camera camera;
    private Archer archer;
    private Koldun koldun;
    private Player player;
    private GameObject UI;

    private void Awake()
    {
        archer = FindObjectOfType<Archer>();
        koldun = FindObjectOfType<Koldun>();
        UI = GameObject.FindGameObjectWithTag("UserInterface");
        camera = Camera.main;
        
        Time.timeScale = 0f;
        koldun.GetComponent<Koldun>().enabled = false;
        archer.GetComponent<Archer>().enabled = false;
        UI.SetActive(false);
        camera.orthographicSize = 1.3f;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        player.input.Disable();
    }

    public void Play()
    {

    }

    public void Quit()
    {
        
    }

    public void Settings()
    {
        
    }
}
