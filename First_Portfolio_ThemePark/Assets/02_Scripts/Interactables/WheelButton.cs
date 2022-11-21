using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelButton : MonoBehaviour
{
    public bool mbIsPressed = false;

    [SerializeField] float m_RestorDamping = 10f;
    private Vector3 m_InitPos;
    private bool mbIsPressing = false;
<<<<<<< HEAD
    private AudioSource buttonAudio;
=======
    private AudioSource m_ButtonAudio;
>>>>>>> 34d6d22bc61a5f9b25fd283e99d3e323aa7749ca

    private void Start()
    {
        m_InitPos = transform.position;
<<<<<<< HEAD
        buttonAudio = GetComponent<AudioSource>();
=======
        m_ButtonAudio = GetComponent<AudioSource>();
>>>>>>> 34d6d22bc61a5f9b25fd283e99d3e323aa7749ca
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
<<<<<<< HEAD
            buttonAudio.PlayOneShot(buttonAudio.clip);
=======
            m_ButtonAudio.PlayOneShot(m_ButtonAudio.clip);
>>>>>>> 34d6d22bc61a5f9b25fd283e99d3e323aa7749ca
            mbIsPressed = !mbIsPressed;
        }
    }

    private void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.tag != "BUTTON")
            mbIsPressing = false;
    }
}
