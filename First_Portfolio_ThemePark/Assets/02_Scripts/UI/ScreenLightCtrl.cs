using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenLightCtrl : MonoBehaviour
{
    public Slider slider;
    public Light ScreenLight;

    private void Update()
    {
        ScreenLight.intensity = slider.value;
        if(ScreenLight.intensity<=0.1f)
        {
            ScreenLight.intensity = 0.11f;
        }    
    }

}
