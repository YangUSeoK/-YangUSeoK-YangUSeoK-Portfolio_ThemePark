using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    

    public void RotateWheel(float _rotSpeed)
    {   
        transform.Rotate(Vector3.right * _rotSpeed * Time.deltaTime);
    }
}
