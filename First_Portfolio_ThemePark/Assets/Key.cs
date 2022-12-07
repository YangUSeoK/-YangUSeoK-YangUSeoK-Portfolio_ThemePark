using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    FixedJoint joint;

    private void Start()
    {
        joint = GetComponent<FixedJoint>();
    }

    public void DeleteJoint()
    {
        Destroy(joint);
    }

}
