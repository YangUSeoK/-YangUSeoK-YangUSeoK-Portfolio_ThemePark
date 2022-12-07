using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WheelButton : MonoBehaviour
{
    public bool mbIsPressed = false;

    [SerializeField] float m_RestorDamping = 10f;
    private Vector3 m_InitPos;
    private bool mbIsPressing = false;
    private AudioSource m_ButtonAudio;
    private Vector3 m_targetPos;

    private void Start()
    {
        m_InitPos = transform.position;
        m_ButtonAudio = GetComponent<AudioSource>();
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
            m_ButtonAudio.PlayOneShot(m_ButtonAudio.clip);
            mbIsPressed = !mbIsPressed;
            Debug.Log("Button Pressed!");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "BUTTON")
        {
            Vector3 otherPos = other.transform.position;
            otherPos.x = transform.position.x;
            otherPos.z = transform.position.z;
            transform.position = /*transform.position + */otherPos;
            //mbIsPressing = true;
            // �ݶ��̴��� ���� ������ Ÿ���� ��ġ�� �ڽ��� ��ġ�� ����ŭ ���ϸ� �ɵ�
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "BUTTON")
        {
            transform.position = m_InitPos;
            mbIsPressing = false;
        }
    }

    private void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.tag != "BUTTON")
            mbIsPressing = false;
    }
}
