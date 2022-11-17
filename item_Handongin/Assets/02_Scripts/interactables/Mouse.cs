using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    [SerializeField] float power = 10f;
    [SerializeField] Vector3 offset;
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
            Debug.DrawLine(ray.origin, ray.direction, Color.red, 2f);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject + "감지");
                if (hit.rigidbody != null)
                {
                    Debug.Log("rigidbody 감지");
                    Debug.Log(hit.rigidbody);
                    hit.rigidbody.AddForce(ray.direction * power, ForceMode.Impulse);
                }
                else
                {
                    Debug.Log("rigidbody 없음");
                }
            }
        }
    }
}
