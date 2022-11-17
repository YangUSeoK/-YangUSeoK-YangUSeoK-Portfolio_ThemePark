using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDoor : MonoBehaviour
{
    public Transform targetPos;
    Rigidbody rb;
    void Start()
    {
        //targetPos = GetComponentInChildren<Transform>();
        rb = GetComponent<Rigidbody>();
    }    

    void Update()
    {
        rb.MovePosition(targetPos.position);
    }
}
