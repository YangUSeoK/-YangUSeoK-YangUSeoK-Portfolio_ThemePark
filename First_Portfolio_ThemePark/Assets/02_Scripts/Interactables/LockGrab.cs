using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LockGrab : XRGrabInteractableTwoAttach
{
    [SerializeField] private Transform m_Lock = null;
    private Transform m_HandTr = null;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Grab()
    {
        base.Grab();
    }
    protected override void Drop()
    {
        base.Drop();
    }
}
