using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] float power = 10f;
    void Start()
    {
        
    }

    void Update()
    {
        MouseAction();
    }

    void MouseAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawLine(ray.origin, ray.direction);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject + "°¨Áö" );
                if (hit.rigidbody != null)
                    hit.rigidbody.AddForce((hit.transform.position - Input.mousePosition).normalized * power);
            }
        }
    }
}
