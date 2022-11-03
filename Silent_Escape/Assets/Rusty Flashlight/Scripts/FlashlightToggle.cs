using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlashlightToggle : MonoBehaviour
{
    public GameObject lightGO;
    private bool isOn = false;
    private Color lensColor = new Color(0.75f, 0.75f, 0.75f);
    public Material[] lens;

    void Start()
    {
        lightGO.SetActive(isOn);
        lens = GetComponentInChildren<Renderer>().materials;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isOn = !isOn;

            if (isOn)
            {
                lightGO.SetActive(true);
                lens[0].SetColor("_EmissionColor", Color.white);
                lens[1].SetColor("_EmissionColor", lensColor);
            }
            else
            {
                lightGO.SetActive(false);
                lens[0].SetColor("_EmissionColor", Color.black);
                lens[1].SetColor("_EmissionColor", Color.black);
            }
        }
    }
}
