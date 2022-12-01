using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private GameObject m_InSlotItem = null; // 디버그용
    public GameObject InSlotItem
    {
        get { return m_InSlotItem; }
        set
        {
            if (m_InSlotItem == null)
            {
                m_InSlotItem = value;
                m_InSlotItem.gameObject.SetActive(false);
                m_InSlotItem.GetComponent<Item>().SetIsInPocket(true);
            }
            else
            {
                Debug.Log("템창 차있음");
            }
        }
    }

    public GameObject OutputItem(Transform _hand)
    {
        if (m_InSlotItem != null)
        {
            m_InSlotItem.transform.SetParent(_hand);
            m_InSlotItem.gameObject.SetActive(true);
            m_InSlotItem.GetComponent<Item>().SetIsInPocket(false);
            m_InSlotItem.transform.position = _hand.position;
            m_InSlotItem.transform.rotation = _hand.rotation;
            m_InSlotItem = null;

            return m_InSlotItem;
        }
        else
        {
            Debug.Log("아이템 없음");
            return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ITEM"))
        {

        }
    }

}
