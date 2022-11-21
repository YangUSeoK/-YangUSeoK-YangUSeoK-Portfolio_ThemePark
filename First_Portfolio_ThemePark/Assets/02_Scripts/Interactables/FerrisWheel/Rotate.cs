using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
<<<<<<< HEAD
    

=======
>>>>>>> 34d6d22bc61a5f9b25fd283e99d3e323aa7749ca
    public void RotateWheel(float _rotSpeed)
    {   
        transform.Rotate(Vector3.right * _rotSpeed * Time.deltaTime);
    }
}
