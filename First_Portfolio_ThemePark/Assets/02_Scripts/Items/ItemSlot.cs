using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private GameObject m_InSlotItem = null; // ����׿�
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
                Debug.Log("��â ������");
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
            Debug.Log("������ ����");
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
