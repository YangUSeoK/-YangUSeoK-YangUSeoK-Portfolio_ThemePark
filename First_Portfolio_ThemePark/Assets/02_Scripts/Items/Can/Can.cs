using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : Item
{
    EItemType eItemtype = EItemType.Can;
    [SerializeField] LayerMask m_ListenerLayer;
    
    public override void Action()
    {
        // 캔이 충돌했을때 실행할 함수내용
        // 소리를 재생하거나, 이펙트를 실행하거나
        // 효과음의 볼륨을 속도(받은충격) 에 따라 변경해서 강하게 던졌을때와 약하게 던졌을때 소리가 다르게 나게 함
        m_ItemAudio[0].volume = m_Speed * 0.05f;
        m_ItemAudio[0].PlayOneShot(m_ItemAudio[0].clip);
    }

    // 충돌할시 소리를 재생하고, 위치를 알리고, 반사각으로 물체를 튕겨나게함
    // 나중에 손으로 잡았을때 콜라이더가 겹치게되면 문제가 생길수 있음
    // 문제 생길시 잡았을땐 아이템의 콜라이더를 비활성화 하거나 손의 콜라이더를 비활성화 할것
    private void OnCollisionEnter(Collision coll)
    {
        if (m_Speed > 1f)
        {
            Action();
            inform();
            HitObject(coll);
        }
    }

    // 충돌했을때 위치를 알리기 위한 함수(임시로 만들어놓은거라 나중에 수정필요)
    void inform()
    {
        Collider[] Listeners = Physics.OverlapSphere(transform.position, m_ItemAudio[0].maxDistance * m_ItemAudio[0].volume, m_ListenerLayer);

        foreach (Collider listener in Listeners)
        {
            listener.GetComponent<Enemy_Listener>().Listen(transform, transform.position, m_ItemAudio[0].maxDistance * m_ItemAudio[0].volume);
        }
    }
}
