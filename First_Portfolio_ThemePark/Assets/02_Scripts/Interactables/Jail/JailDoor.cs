using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailDoor : MonoBehaviour
{
    [SerializeField] bool mbIsStart = false;
    Rigidbody m_DoorRigid;

    void Start()
    {
        m_DoorRigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        m_DoorRigid.isKinematic = !mbIsStart;
    }
}
