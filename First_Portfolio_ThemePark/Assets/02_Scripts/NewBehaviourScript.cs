using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform rHand;
    public Transform lHand;

    public Transform rPocket;
    public Transform lPocket;

    private void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
       if (other.transform == rPocket)
        {
            // �տ� �������� �ִ��� �κ��� �������� �ִ��� �˻�
        }
       else if (other.transform == lPocket)
        {

        }
    }
}
