using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMinimap : MonoBehaviour
{
    [SerializeField] private GameMenuManager m_GameMenuManager = null;
    [SerializeField] private GameObject mTutorialMapUI=null;
    public void IsGrabbed()
    {
        m_GameMenuManager.IsMinimapOpen = true;
        //221208 ±èÁØ¿ì
        mTutorialMapUI.SetActive(true);
        Destroy(gameObject);
    }
}
