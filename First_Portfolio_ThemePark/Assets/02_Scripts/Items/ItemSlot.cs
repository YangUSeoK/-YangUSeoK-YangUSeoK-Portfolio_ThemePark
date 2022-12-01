using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Item = null; // 디버그용
    public GameObject Item
    {
        get { return m_Item; }
        set { m_Item = value; }
    }
    private bool m_IsTurnOn = false;
    private bool m_HaveItem = false;


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
