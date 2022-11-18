using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSummon : MonoBehaviour
{
    public GameObject prefab;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);
            go.GetComponent<Rigidbody>().AddForce(Vector3.forward * 500f);
        }
    }
}
