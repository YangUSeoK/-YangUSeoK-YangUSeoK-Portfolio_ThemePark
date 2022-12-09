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

    public void Grabbed()
    {
        Destroy(joint);
        GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[1];
    }

    public void UnGrabbed()
    {
        Debug.Log(GetComponent<MeshRenderer>().materials[0].name);
        GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[0];
    }
}
