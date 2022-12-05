using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorGrab : XRGrabInteractable
{
    [SerializeField] private Transform m_Handle = null;
    [SerializeField] private Transform m_Door = null;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Drop()
    {
        base.Drop();

        transform.position = m_Handle.position;
        transform.rotation = m_Handle.rotation;
        transform.localScale = Vector3.one;

        Rigidbody handleRb = m_Handle.GetComponent<Rigidbody>();
        handleRb.velocity = Vector3.zero;
        handleRb.angularVelocity = Vector3.zero;

        Rigidbody doorRb = m_Door.GetComponent<Rigidbody>();
        doorRb.velocity = Vector3.zero;
        doorRb.angularVelocity = Vector3.zero;
    }
}
