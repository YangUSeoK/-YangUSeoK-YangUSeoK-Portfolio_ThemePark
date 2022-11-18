using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    [SerializeField] Collider[] m_Colliders;
    [SerializeField] Transform m_player;
    Vector3 m_CenterPos;
    private void Awake()
    {
        m_Colliders = GetComponents<Collider>();
    }

    private void Start()
    {
        m_CenterPos = transform.position + new Vector3(0f, 27f, 0f);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, m_player.position) < 3f)
            ActivateColl();
        else
            DeactivateColl();
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

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.TryGetComponent<Rigidbody>(out var rigid) == true)
        {
            coll.transform.SetParent(transform);
            //coll.transform.localPosition = coll.transform.position;
            //coll.rigidbody.isKinematic = true;
        }
    }

    private void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.TryGetComponent<Rigidbody>(out var rigid) == true)
        {
            //coll.transform.parent = null;
            coll.transform.SetParent(null);
            //coll.rigidbody.isKinematic = false;
        }
    }
}
