using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool mbIsPressed = false;

    [SerializeField] float m_RestorDamping = 10f;
    private Vector3 m_InitPos;
    private bool mbIsPressing = false;
    private AudioSource buttonAudio;

    private void Start()
    {
        m_InitPos = transform.position;
        buttonAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (transform.position.y < m_InitPos.y && mbIsPressing == false)
        {
            transform.position = Vector3.Lerp(transform.position, m_InitPos, m_RestorDamping * Time.deltaTime);
        }

        if (transform.position.y > m_InitPos.y)
            transform.position = m_InitPos;
    }
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag != "BUTTON")
            mbIsPressing = true;

        if (coll.gameObject.tag == "TRIGGER")
        {
            buttonAudio.PlayOneShot(buttonAudio.clip);
            mbIsPressed = !mbIsPressed;
        }
    }

    private void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.tag != "BUTTON")
            mbIsPressing = false;
    }
}
