using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Item
{
    EItemType itemType = EItemType.Rock;
    [SerializeField] LayerMask m_ListenerLayer;

    public override void Action()
    {
        m_ItemAudio[0].volume = m_Speed * 0.05f;
        m_ItemAudio[0].PlayOneShot(m_ItemAudio[0].clip);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (m_Speed > 1f)
        {
            Action();
            inform();
            HitObject(coll);
        }
    }

    void inform()
    {
        Collider[] Listeners = Physics.OverlapSphere(transform.position, m_ItemAudio[0].maxDistance * m_ItemAudio[0].volume * 0.5f, m_ListenerLayer);

        foreach (Collider listener in Listeners)
        {
            listener.GetComponent<Enemy_Listener>().Listen(transform, transform.position, m_ItemAudio[0].volume);
        }
    }
}
