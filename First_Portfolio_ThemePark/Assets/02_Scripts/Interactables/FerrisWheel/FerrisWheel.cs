using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheel : MonoBehaviour
{
    [SerializeField] float m_RotSpeed = 5f;
    [SerializeField] private WheelButton m_WheelButton;
    private Rotate m_WheelRotate;
    private Cylinder[] m_Cylinders;
    private Transform m_Player;

    void Start()
    {
        m_WheelRotate = GetComponentInChildren<Rotate>();
        m_Cylinders = GetComponentsInChildren<Cylinder>();
        m_Player = GameObject.FindGameObjectWithTag("PLAYER").transform;

        if (m_Player != null)
        {
            foreach (var cylinder in m_Cylinders)
            {
                cylinder.Player = m_Player;
            }
        }
    }

    void Update()
    {
        m_WheelRotate.RotateWheel(m_RotSpeed);

        if (m_WheelButton.mbIsPressed == true)
            m_RotSpeed = 0f;
        else
            m_RotSpeed = 5f;
    }
}
