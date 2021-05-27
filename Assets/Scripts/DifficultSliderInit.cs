using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultSliderInit : MonoBehaviour
{
    public Text difficultText;
    private string[] difficults = { "Легкая", "Средняя", "Сложная" };
    private Color[] colors = { Color.white, Color.yellow, Color.red };
    private void Awake()
    {
        var slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetInt("difficult");
        SetDifficultText((int) slider.value);
    }

    public void SetDifficultText(int difficultLevel)
    {
        difficultText.text = difficults[difficultLevel];
        difficultText.color = colors[difficultLevel];
    }
}
