using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    [SerializeField] Collider[] m_Colliders;
    [HideInInspector] public Transform Player;

    private void Awake()
    {
        m_Colliders = GetComponents<Collider>();
    }

    private void Update()
    {
        //TrunColl();
    }

    void TrunColl()
    {
        if (Player != null)
        {
            if (Vector3.Distance(transform.position, Player.position) < 3f)
                ActivateColl();
            else
                DeactivateColl();
        }
    }

    void ActivateColl()
    { 
        foreach(var coll in m_Colliders)
        {
            if (coll.enabled == false)
                coll.enabled = true;
        }
    }

    void DeactivateColl()
    {
        foreach(var coll in m_Colliders)
        {
            if (coll.enabled == true)
                coll.enabled = false;
        }
    }

    //private void OnCollisionEnter(Collision coll)
    //{
    //    if (coll.gameObject.TryGetComponent<Rigidbody>(out var rigid) == true)
    //    {
    //        coll.transform.SetParent(transform);
    //        //coll.transform.localPosition = coll.transform.position;
    //        //coll.rigidbody.isKinematic = true;
    //    }
    //}

    //private void OnCollisionExit(Collision coll)
    //{
    //    if (coll.gameObject.TryGetComponent<Rigidbody>(out var rigid) == true)
    //    {
    //        //coll.transform.parent = null;
    //        coll.transform.SetParent(null);
    //        //coll.rigidbody.isKinematic = false;
    //    }
    //}
}
