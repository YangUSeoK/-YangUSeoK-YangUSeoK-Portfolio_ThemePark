using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceTest : MonoBehaviour
{
    public Vector3 targetPos;
    public bool onSound = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (onSound == true)
        {
            targetPos.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime);

            if (transform.position == targetPos)
                onSound = false;
        }
    }
}
