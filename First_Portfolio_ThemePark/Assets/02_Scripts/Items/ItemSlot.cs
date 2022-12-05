using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private GameObject m_Player = null;


    [SerializeField]
    private GameObject m_Item = null; // 디버그용
    public GameObject Item
    {
        get { return m_Item; }
        set { m_Item = value; }
    }

    [SerializeField]
    private GameObject m_HandItem = null; // 디버그용
    public GameObject HandItem
    {
        get { return HandItem; }
        set { m_HandItem = value; }
    }

    [SerializeField]
    private GameObject m_Hand = null; // 디버그용



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnger : " + other.name);

        if (other.CompareTag("ITEM"))
        {
            if (m_HandItem == null)
            {
                m_HandItem = other.gameObject;
            }
        }
        if (other.CompareTag("RHAND") || other.CompareTag("LHAND"))
        {
            GetComponent<MeshRenderer>().enabled = true;
            if (m_Hand == null)
            {
                m_Hand = other.gameObject;
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ITEM"))
        {
            if (m_HandItem != null)
            {
                m_HandItem = null;
            }
        }
        if (other.CompareTag("RHAND") || other.CompareTag("LHAND"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            if (m_Hand != null)
            {
                m_Hand = null;
            }
        }
    }

    public void PopItem()
    {
        if (m_Item != null)
        {
            Debug.Log("PoP");
            m_Item.SetActive(true);
            m_Item.transform.position = m_Hand.transform.position;
            //m_Item.transform.SetParent(null, false);
            m_Item = null;
        }
        else
        {
            Debug.Log("아이템이 없습니다");
        }
    }

    public void PushItem()
    {
        Debug.Log("Push");
        if (m_Item == null)
        {
            m_Item = m_HandItem;
            //m_Item.transform.SetParent(transform, false);
            m_Item.SetActive(false);
        }
        else
        {
            Debug.Log("아이템이 다찼습니다.");
        }
    }



    public void Enter()
    {
        GetComponent<MeshRenderer>().material.color = Color.black;
    }
    public void Exit()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
