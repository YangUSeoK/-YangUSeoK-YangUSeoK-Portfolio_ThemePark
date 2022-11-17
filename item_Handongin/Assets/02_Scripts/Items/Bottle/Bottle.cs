using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : Item
{
    EItemType eItemtype = EItemType.Bottle;
    [SerializeField] GameObject m_CrashBottlePrefab = null;
    [SerializeField] float m_Power = 0f;
    [SerializeField] LayerMask m_ListenerLayer;
    private Vector3 m_HitPos;

    protected override void ActionInterface()
    {
        // 병이 충돌했을때 실행할 내용
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        m_Rigid.isKinematic = true;
        GameObject go = Instantiate(m_CrashBottlePrefab, transform.position, transform.rotation);
        Rigidbody[] rbs = go.GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < rbs.Length; i++)
        {
            Debug.Log(m_Speed);
            if (rbs[i] != null)
            {
                Debug.Log(i + " 실행");
                rbs[i].AddExplosionForce(m_Speed * m_Power, m_HitPos, 10f);
            }
        }
        Destroy(gameObject, 3f);
        Destroy(go, 3f);
    }

    private void OnCollisionEnter(Collision coll)
    {
        HitObject(coll);
        if (m_Speed > 5f)
        {
            m_HitPos = coll.GetContact(0).point;
            m_ItemAudio[0].Play();
            inform();
            Action();
        }
    }

    void inform()
    {
        Collider[] Listeners = Physics.OverlapSphere(transform.position, m_ItemAudio[0].maxDistance, m_ListenerLayer);
        
        foreach(Collider listener in Listeners)
        {
            listener.GetComponent<TraceTest>().targetPos = transform.position;
            listener.GetComponent<TraceTest>().onSound = true;
        }
    }
}
