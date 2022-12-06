using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Item : ICatch
{
    [Flags]
    public enum EItemType
    {
        Flashlight = 1,
        Bottle = 2,
        Can = 4,
        Rock = 8
    }
    protected Rigidbody m_Rigid;
    protected AudioSource[] m_ItemAudio;
    [SerializeField] protected bool mbIsHanded = false;
    protected bool mbIsInPocket = false;
    protected float m_Speed;
    [HideInInspector] public bool mbUICatch = false;

    void Awake()
    {
        m_Rigid = GetComponent<Rigidbody>();
        m_ItemAudio = GetComponents<AudioSource>();
    }

    void Update()
    {
        GetAcceleration();
    }

    public abstract void Action();

    private void GetAcceleration()
    {
        if (m_Rigid.velocity.magnitude > 1f)
            m_Speed = m_Rigid.velocity.magnitude;
    }

    // 반사각 구하는 함수
    protected void HitObject(Collision coll)
    {
        Vector3 inVector = transform.position - coll.transform.position;
        Vector3 collVector = coll.transform.position;

        float collAngle = Mathf.Atan2(collVector.y, collVector.x);
        float inAngle = Vector3.SignedAngle(collVector, inVector, -Vector3.forward);

        float refAngle = inAngle - 180 + collAngle;
        float refRadian = refAngle * Mathf.Rad2Deg;

        Vector3 refVector = new Vector3(Mathf.Cos(refRadian), Mathf.Sin(refRadian));

        m_Rigid.AddForce(refVector * m_Speed, ForceMode.Impulse);
    }

    public void SetIsGrabed(bool _isGrabed)
    {
        mbIsHanded = _isGrabed;
    }
   
    public void SetIsInPocket(bool _isInPocket)
    {
        mbIsInPocket = _isInPocket;
    }
}
