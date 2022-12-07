using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMinimap : MonoBehaviour
{
    [SerializeField] private GameMenuManager m_GameMenuManager = null;

    public void IsGrabbed()
    {
        m_GameMenuManager.IsMinimapOpen = true;
        Destroy(gameObject);
    }
}
