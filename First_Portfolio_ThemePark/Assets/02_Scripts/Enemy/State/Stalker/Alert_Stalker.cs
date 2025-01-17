using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert_Stalker : EnemyState
{
    public Alert_Stalker(Enemy _enemy) : base("Alert", _enemy) { }

    float mTimer = 0f;

    public override void Action()
    {
        // �Ҹ��� 10�ʵ��� ¢�´�.
        mTimer = 0f;
        while (mTimer < 1f)
        {
            mTimer += Time.deltaTime;
            float delay = Random.Range(0, 3);

            m_Enemy.Anim.SetFloat("RandomBark", delay);
            if (delay != 0f)
            {
                m_Enemy.Audio[0].PlayOneShot(m_Enemy.Audio[0].clip);
            }
        }
        mTimer = 0f;
    }

    public override void CheckState()
    {
        // 10�� ���� ¢�� Set Sniff��
        float timer = 0f;
        timer += Time.deltaTime;
        if(timer >= (m_Enemy as Enemy_Stalker).AlertTime)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Stalker).Sniff);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Dog : �ΰ��̴� �۸�!");
        m_Enemy.Agent.destination = m_Enemy.transform.position;

        m_Enemy.Anim.SetTrigger("IsAlert");
        Howling();
        // �Ҹ� ¢�� ����
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    private void Howling()
    {
        Collider[] listeners = new Collider[10];
        int layer = 1 << LayerMask.NameToLayer("LISTENER");
        Physics.OverlapSphereNonAlloc(m_Enemy.transform.position, (m_Enemy as Enemy_Stalker).AlertRange, listeners, layer);

        for(int i = 0; i < listeners.Length; ++i)
        {
            if(listeners[i] != null)
            {
                listeners[i].GetComponent<Enemy_Listener>().Listen(m_Enemy.PlayerTr, m_Enemy.transform.position,
                                                                    (m_Enemy as Enemy_Stalker).AlertRange);
            }
        }
    }
}
