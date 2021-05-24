using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TeleportTofirstScene : MonoBehaviour
{
    public GameObject fill;
    private Image background;
    private GameObject player;

    private bool isTriggered;
    private string sceneName = "NewScene1";
    
    private void Awake()
    {
        fill.SetActive(false);
        background = fill.GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isTriggered)
            return;
        isTriggered = true;
        StartCoroutine(Blackout());
    }
    
    private IEnumerator Blackout()
    {
        fill.SetActive(true);
        var backgroundColor = background.color;
        while (background.color.a < 1)
        {
            backgroundColor = new Color(backgroundColor.r,
                backgroundColor.g,
                backgroundColor.b,
                backgroundColor.a + 0.05f);
            background.color = backgroundColor;
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        var currentScene = SceneManager.GetActiveScene();
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(sceneName));
        player.transform.position = new Vector3(0, 1);
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
