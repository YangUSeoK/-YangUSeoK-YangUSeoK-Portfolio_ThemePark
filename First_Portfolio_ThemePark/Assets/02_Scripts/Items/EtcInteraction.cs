using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtcInteraction : Item
{
    public override void Action()
    {
        m_ItemAudio[0].volume = m_Speed * 0.05f;
        if (m_ItemAudio[0].volume > 0.5f)
            m_ItemAudio[0].volume = 0.5f;
        m_ItemAudio[0].PlayOneShot(m_ItemAudio[0].clip);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (m_Speed > 1f)
        {
            Action();
        }
    }
}
