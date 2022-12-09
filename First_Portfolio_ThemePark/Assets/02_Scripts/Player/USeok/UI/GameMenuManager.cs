using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Menu = null;
    [SerializeField] private GameObject m_MiniMap = null;
    [SerializeField] private GameObject m_Tutorial = null;
    [SerializeField] private Transform m_FinishUI = null;
    [SerializeField] private GameObject m_StartFadeInUI = null;

    [SerializeField] private InputActionProperty m_ShowMenuButton;
    [SerializeField] private InputActionProperty m_ShowMiniMapButton;



    [SerializeField] private Transform m_Head = null;
    [SerializeField] private Transform m_MenuPos = null;
    [SerializeField] private Transform m_MiniMapPos = null;
    [SerializeField] private Transform m_TutorialMiniMapPos = null;

    [SerializeField] private SkinnedMeshRenderer m_LHandRenderer = null;
    [SerializeField] private SkinnedMeshRenderer m_RHandRenderer = null;

    private bool mb_IsMinimapOpen = false;
    public bool IsMinimapOpen
    {
        set { mb_IsMinimapOpen = value; }
    }

    private float spawnDistance = 1f;

    private void Start()
    {
        m_StartFadeInUI.transform.position = m_TutorialMiniMapPos.position;
        m_StartFadeInUI.transform.LookAt(new Vector3(m_Head.position.x, m_Head.position.y, m_Head.position.z));
        m_StartFadeInUI.transform.forward *= -1;


        m_StartFadeInUI.GetComponentInChildren<Image>().CrossFadeAlpha(0f, 2f, false);
    }

    private void Update()
    {
        if (m_ShowMenuButton.action.WasPressedThisFrame())
        {
            m_Menu.SetActive(!m_Menu.activeSelf);
            if (m_LHandRenderer.enabled)
            {
                m_LHandRenderer.enabled = false;
            }
            else
            {
                m_LHandRenderer.enabled = true;
            }
        }
        m_Menu.transform.position = m_MenuPos.position;
        m_Menu.transform.LookAt(new Vector3(m_Head.position.x, m_Head.position.y, m_Head.position.z));
        m_Menu.transform.forward *= -1;



        if (mb_IsMinimapOpen && m_ShowMiniMapButton.action.WasPressedThisFrame())
        {
            m_MiniMap.SetActive(!m_MiniMap.activeSelf);
            if (m_RHandRenderer.enabled)
            {
                m_RHandRenderer.enabled = false;
            }
            else
            {
                m_RHandRenderer.enabled = true;
            }
        }
        m_MiniMap.transform.position = m_MiniMapPos.position;
        m_MiniMap.transform.LookAt(new Vector3(m_Head.position.x, m_Head.position.y, m_Head.position.z));
        m_MiniMap.transform.forward *= -1;

        //221208 ±èÁØ¿ì
        m_Tutorial.transform.position = m_TutorialMiniMapPos.position;
        m_Tutorial.transform.LookAt(new Vector3(m_Head.position.x, m_Head.position.y, m_Head.position.z));
        m_Tutorial.transform.forward *= -1;

        m_FinishUI.position = m_TutorialMiniMapPos.position;
        m_FinishUI.LookAt(new Vector3(m_Head.position.x, m_Head.position.y, m_Head.position.z));
        m_FinishUI.forward *= -1f;



    }

    

    
}
