using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPoint : MonoBehaviour
{
    [SerializeField] Transform m_TargetTr;
    [SerializeField] Vector3 m_TargetPos;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = m_TargetTr.position + m_TargetPos;
    }
}
