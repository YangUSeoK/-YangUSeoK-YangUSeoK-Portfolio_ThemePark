using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tir : MonoBehaviour
{
    [SerializeField] Transform m_BottlePoint;
    [SerializeField] Transform m_CanPoint;
    [SerializeField] GameObject m_BottlePrefab;
    [SerializeField] GameObject m_CanPrefab;
    WheelButton[] m_Buttons;
    WheelButton mBlueButton;
    WheelButton mRedButton;
    List<GameObject> mSummonObjects = new List<GameObject>();

    void Start()
    {
        m_Buttons = GetComponentsInChildren<WheelButton>();
        mBlueButton = m_Buttons[0];
        mRedButton = m_Buttons[1];
    }

    void Update()
    {
        SummonAll();
        DestroyAll();
    }

    void SummonAll()
    {
        if (mBlueButton.mbIsPressed == true)
        {
            SummonBottle();
            SummonCan();
            mBlueButton.mbIsPressed = false;
        }
    }

    void SummonBottle()
    {
        GameObject go = Instantiate(m_BottlePrefab, m_BottlePoint.position, m_BottlePoint.rotation);
        mSummonObjects.Add(go);
    }

    void SummonCan()
    {
        GameObject go = Instantiate(m_CanPrefab, m_CanPoint.position, m_CanPoint.rotation);
        mSummonObjects.Add(go);
    }

    void DestroyAll()
    {
        if (mRedButton.mbIsPressed == true)
        {
            foreach(var obj in mSummonObjects)
            {
                Destroy(obj);
            }
            mRedButton.mbIsPressed = false;
        }
    }
}
