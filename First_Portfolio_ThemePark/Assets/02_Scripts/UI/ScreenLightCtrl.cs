using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenLightCtrl : MonoBehaviour
{
    public Slider slider;
    public Light ScreenLight;
    private float mIntensity = 0f;

    public void LightUpdate()
    {
        mIntensity = Mathf.Clamp(slider.value, 0.1f, 0.6f);
        ScreenLight.intensity = mIntensity;
    }
}
