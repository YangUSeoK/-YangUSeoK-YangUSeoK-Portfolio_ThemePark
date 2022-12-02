using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Menu = null;
    [SerializeField] private GameObject m_MiniMap = null;
    
    [SerializeField] private InputActionProperty m_ShowMenuButton;
    [SerializeField] private InputActionProperty m_ShowMiniMapButton;



    [SerializeField] private Transform m_Head = null;
    [SerializeField] private Transform m_MenuPos = null;

    private float spawnDistance = 1f;

    private void Update()
    {
        if (m_ShowMenuButton.action.WasPressedThisFrame())
        {
            m_Menu.SetActive(!m_Menu.activeSelf); 
        }
        m_Menu.transform.position = m_MenuPos.position;
        m_Menu.transform.LookAt(new Vector3(m_Head.position.x, m_Head.position.y, m_Head.position.z));
        m_Menu.transform.forward *= -1;



        if (m_ShowMiniMapButton.action.WasPressedThisFrame())
        {
            m_MiniMap.SetActive(!m_MiniMap.activeSelf);
        }
        m_MiniMap.transform.position = m_Head.position + (new Vector3(m_Head.forward.x, 0f, m_Head.forward.z).normalized * spawnDistance);
        m_MiniMap.transform.LookAt(new Vector3(m_Head.position.x, m_Menu.transform.position.y, m_Head.position.z));
        //m_MiniMap.transform.forward *= -1;
    }

    
}
