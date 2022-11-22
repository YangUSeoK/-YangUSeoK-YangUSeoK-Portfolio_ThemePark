using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSound : MonoBehaviour
{
    private float stepSound = 10f;

    private void Update()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, stepSound, 1 << LayerMask.NameToLayer("LISTENER"));
        for (int i = 0; i < colls.Length; ++i)
        {
            colls[i].gameObject.GetComponent<Enemy_Listener>().Listen(transform.position, transform);
        }
    }
}
