using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inven : MonoBehaviour
{
    private ItemSlot[] m_Slots = null;

    private void Awake()
    {
        m_Slots = GetComponentsInChildren<ItemSlot>();
    }
}
