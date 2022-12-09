using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItemRimColor : MonoBehaviour
{
    [SerializeField] private Color RimColor;
    [SerializeField] [Range(0,20)] private float RimPower = 3f;


    public void SetOnRimLight()
    {
        Debug.Log("SetOn");
        transform.GetComponent<Renderer>().material.SetColor("_RimColor", RimColor);
        transform.GetComponent<Renderer>().material.SetFloat("_RimPower", RimPower);
    }

    public void SetOffRimLight()
    {
        Debug.Log("SetOff");
        transform.GetComponent<Renderer>().material.SetColor("_RimColor", Color.black);
        transform.GetComponent<Renderer>().material.SetFloat("_RimPower", RimPower);
    }
}
