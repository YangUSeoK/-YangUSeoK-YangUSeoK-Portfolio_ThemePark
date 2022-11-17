using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float m_RotSpeed = 10f;

    void Update()
    {
        RotateWheel();
    }

    void RotateWheel()
    {
        transform.Rotate(Vector3.right * m_RotSpeed * Time.deltaTime);
    }
}
