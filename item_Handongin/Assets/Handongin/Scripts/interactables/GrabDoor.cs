using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabDoor : MonoBehaviour
{
    Rigidbody rb;
    Vector3 initPos;
    Quaternion initRot;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initPos = transform.localPosition;
        initRot = transform.localRotation;
    }

    private void Update()
    {
        if (transform.position != initPos)
        {
            Invoke("Init", 4f);
        }
    }

    void Init()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.localPosition = initPos;
        transform.localRotation = initRot;
    }
}
