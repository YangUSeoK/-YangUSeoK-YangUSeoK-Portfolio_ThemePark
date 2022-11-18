using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float m_RotSpeed = 10f;
    [SerializeField] Button m_Button;

    void Update()
    {
        if (m_Button.mbIsPressed == true)
            RotateWheel();
    }

    void RotateWheel()
    {   
        transform.Rotate(Vector3.right * m_RotSpeed * Time.deltaTime);
    }
}
