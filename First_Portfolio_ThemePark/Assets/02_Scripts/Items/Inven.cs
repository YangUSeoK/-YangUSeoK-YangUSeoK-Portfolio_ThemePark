using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inven : MonoBehaviour
{
    [SerializeField] private GameObject m_Player = null;
    private ItemSlot[] m_Slots = null;

    [SerializeField]
    private GameObject m_Item = null; // 디버그용
    public GameObject Item
    {
        get { return m_Item; }
        set { m_Item = value; }
    }
    private GameObject m_HandItem = null;
    public GameObject HandItem
    {
        get { return HandItem; }
        set { m_HandItem = value; }
    }
    private bool m_IsTurnOn = false;

    private GameObject m_LHand = null;
    private GameObject m_RHand = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LHAND"))
        {
            m_LHand = other.gameObject;
        }
        if (other.CompareTag("RHAND"))
        {
            m_RHand = other.gameObject;
        }
    }

    private void Awake()
    {
        m_Slots = GetComponentsInChildren<ItemSlot>();
    }

    public void PopItem()
    {
        if (m_IsTurnOn)
        {
            m_Item.SetActive(true);
            m_Item = null;
        }
    }

    public void PushItem()
    {
        if (m_IsTurnOn)
        {
            //m_Item = _item;
            m_Item.SetActive(false);
        }
    }

    public void TurnOn()
    {
        m_IsTurnOn = true;
    }

    public void TurnOff()
    {
        m_IsTurnOn = false;
    }
}
