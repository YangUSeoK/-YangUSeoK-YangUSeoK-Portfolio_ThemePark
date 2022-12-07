using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLightManager : MonoBehaviour
{
    private Light[] m_StreetLights = null;

    private void Awake()
    {
        m_StreetLights = GetComponentsInChildren<Light>();
    }

    public void TurnOffAllLight()
    {
        for (int i = 0; i < m_StreetLights.Length; ++i)
        {
            m_StreetLights[i].enabled = false;
        }
    }

}
