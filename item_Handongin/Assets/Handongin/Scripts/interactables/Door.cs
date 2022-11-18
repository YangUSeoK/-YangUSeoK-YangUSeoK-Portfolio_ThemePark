using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Rigidbody doorRigid;
    Quaternion initRot;
    HingeJoint hingeJoint;
    bool isLock = false;
    void Start()
    {
        initRot = transform.rotation;
        doorRigid = GetComponent<Rigidbody>();
        hingeJoint = GetComponent<HingeJoint>();
    }

    void Update()
    {
        //if (transform.rotation == initRot)
        //    isLock = true;
        //else
        //    isLock = false;

        if (isLock == true)
        {
            doorRigid.velocity = Vector3.zero;
            doorRigid.angularVelocity = Vector3.zero;
            doorRigid.isKinematic = true;
        }
        else
            doorRigid.isKinematic = false;

        if (hingeJoint.angle <= hingeJoint.limits.min)
        {
            isLock = true;
        }
        else
            isLock = false;
        
    }

    void LimitAngle()
    {
        if (transform.rotation.y <= 0f)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if (transform.rotation.y >= 120f)
            transform.rotation = Quaternion.Euler(0f, 120f, 0f);


    }
}
