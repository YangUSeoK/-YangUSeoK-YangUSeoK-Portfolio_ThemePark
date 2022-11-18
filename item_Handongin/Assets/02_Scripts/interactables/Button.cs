using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] float m_RestorDamping = 10f;
    public bool mbIsPressed = false;
    private Vector3 m_InitPos;
    private bool mbIsPressing = false;

    private void Start()
    {
        m_InitPos = transform.position;
    }

    private void Update()
    {
        if (transform.position.y < m_InitPos.y && mbIsPressing == false)
        {
            transform.position = Vector3.Lerp(transform.position, m_InitPos, m_RestorDamping * Time.deltaTime);
        }
    }
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag != "BUTTON")
            mbIsPressing = true;

        if (coll.gameObject.tag == "TRIGGER")
            mbIsPressed = !mbIsPressed;
    }

    private void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.tag != "BUTTON")
            mbIsPressing = false;
    }
}
