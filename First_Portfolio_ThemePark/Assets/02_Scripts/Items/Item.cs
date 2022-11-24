using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Item : MonoBehaviour, ICatch
{
    [Flags]
    public enum EItemType
    {
        Flashlight = 1,
        Bottle = 2,
        Can = 4
    }   
    protected Rigidbody m_Rigid;
    protected AudioSource[] m_ItemAudio;
    [SerializeField] protected bool mbIsHanded = false;
    protected bool mbIsInPocket = false;
    protected bool mbIsClose = false;
    protected float m_Speed;

    void Awake()
    {
        m_Rigid = GetComponent<Rigidbody>();
        m_ItemAudio = GetComponents<AudioSource>();
    }

    void Update()
    {
        GetAcceleration();
        //CheakHanded();
        CheakPocket();
    }

    public void Action()
    {
        ActionInterface();
    }

    protected virtual void ActionInterface()
    {
        Debug.Log("Item Action");
        return;
    }

    private void GetAcceleration()
    {
        if (m_Rigid.velocity.magnitude > 1f)
            m_Speed = m_Rigid.velocity.magnitude;
    }

    // 반사각 구하는 함수(가져온거라 이해후 보정필요)
    protected void HitObject(Collision coll)
    {
        //Vector3 dir = transform.position - coll.transform.position;

        Vector3 inVector = transform.position - coll.transform.position;
        Vector3 collVector = coll.transform.position;

        float collAngle = Mathf.Atan2(collVector.y, collVector.x);
        float inAngle = Vector3.SignedAngle(collVector, inVector, -Vector3.forward);

        float refAngle = inAngle - 180 + collAngle;
        float refRadian = refAngle * Mathf.Rad2Deg;

        Vector3 refVector = new Vector3(Mathf.Cos(refRadian), Mathf.Sin(refRadian));

        m_Rigid.AddForce(refVector * m_Speed, ForceMode.Impulse);
    }

    //void CheakHanded()
    //{
    //    if (transform.parent != null)
    //    {
    //        Debug.Log("부모있음");
    //        mbIsHanded = true;
    //    }
    //    else
    //    {
    //        Debug.Log("부모없음");
    //        mbIsHanded = false;
    //    }
    //}

    void CheakPocket()
    {
        if (transform.parent.tag == "POCKET")
            mbIsInPocket = true;
        else
            mbIsInPocket = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HAND")
        {
            mbIsClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "HAND")
        {
            mbIsClose = false;
        }
    }

    public void SetGrabed()
    {
        mbIsHanded = true;
    }

    public void SetUnGrabed()
    {
        mbIsHanded = false;
    }
}
