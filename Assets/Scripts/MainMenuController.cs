using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject fill;
    private Image background;
    public GameObject settings;
    public GameObject mainMenu;
    public Slider soundSlider;
    public Slider difficultSlider;

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
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void BackToMenu()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OnSoundSlider()
    {
        PlayerPrefs.SetFloat("sound", soundSlider.value);
        AudioListener.volume = soundSlider.value;
    }

    public void OnDifficultSlider()
    {
        PlayerPrefs.SetInt("difficult", (int) difficultSlider.value);
        difficultSlider.GetComponent<DifficultSliderInit>().SetDifficultText((int) difficultSlider.value);
        
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
        
        SceneManager.LoadScene("Forest");
    }
}
