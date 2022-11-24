using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSound : MonoBehaviour
{
    [SerializeField] private float stepSound = 50f;

    private void Update()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, stepSound, 1 << LayerMask.NameToLayer("LISTENER"));
        for (int i = 0; i < colls.Length; ++i)
        {
            colls[i].gameObject.GetComponent<Enemy_Listener>().Listen(transform, transform.position);
        }
    }
}
