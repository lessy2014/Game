using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject GameOverUi;
    public GameObject Victory;
    private bool active;

    private void Awake()
    {
        GameOverUi.SetActive(false);
    }
    void Update()
    {
        if (Player.Instance.gameOver && !active)
        {
                OpenGameOver();
        }
        if(Player.Instance.victory && !active)
        {
            OpenVictory();
        }
    }

    public void OpenGameOver()
    {
        GameOverUi.SetActive(true);
        Time.timeScale = 0f;
        active = true;
    }

    public void OpenVictory()
    {
        Victory.SetActive(true);
        Time.timeScale = 0f;
        active = true;
    }

    public void Resume()
    {
        GameOverUi.SetActive(false);
        Player.Instance.gameOver = false;
        active = false;
        Time.timeScale = 1f;
        var currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "SampleScene" || currentScene == "Castle")
            SceneManager.LoadScene("Castle");
        else
            SceneManager.LoadScene("Forest");


    }

    public void BackToMenu()
    {
        GameOverUi.SetActive(false);
        active = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

}
