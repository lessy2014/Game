using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject fill;
    private Image background;

    private void Awake()
    {
        fill.SetActive(false);
        background = fill.GetComponent<Image>();
    }

    public void Play()
    {
        StartCoroutine(Blackout());
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Settings()
    {
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

        Player.Difficult = 2;
        SceneManager.LoadScene("Forest");
    }
}
