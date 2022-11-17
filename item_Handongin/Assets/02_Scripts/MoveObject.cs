using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    float moveSpeed = 5f;
    Vector3 enterPos;
    bool isStop = false;
    void Start()
    {
        
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(-h, 0f, -v).normalized;

        if (isStop == false)
            moveSpeed = 5f;
        else
            moveSpeed = 1f;
            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        isStop = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isStop = false;
    }
}
