using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheel : MonoBehaviour
{
    [SerializeField] float m_RotSpeed = 5f;
    Rotate m_WheelRotate;
    WheelButton m_WheelButton;

    void Start()
    {
        m_WheelRotate = GetComponentInChildren<Rotate>();
        m_WheelButton = GetComponentInChildren<WheelButton>();
    }

    void Update()
    {
        if (m_WheelButton.mbIsPressed == true)
            m_WheelRotate.RotateWheel(m_RotSpeed);
    }
}
