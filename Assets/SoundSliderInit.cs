using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderInit : MonoBehaviour
{
    private void Awake()
    {
        var slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("sound");
    }
}
