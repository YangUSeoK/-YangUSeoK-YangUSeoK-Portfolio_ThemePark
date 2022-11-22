using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace_Listener : EnemyState
{
    public Trace_Listener(Enemy _enemy) : base("Trace", _enemy) { }
   
    public override void EnterState()
    {
        Debug.Log("Trace ����!");
        // �Ҹ�������

        m_Enemy.Agent.destination = (m_Enemy as Enemy_Listener).SoundPos;
    }

    public override void ExitState()
    {
        Debug.Log("Trace ����!");
        // �Ҹ������� �ֺ� �ѷ����� �ִϸ��̼�
    }

    public override void Action()
    {
    }

    public override void CheckState()
    {

        if (m_Enemy.PlayerTr != null && Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position) <= m_Enemy.AttackRange)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Listener).Attack);
        }

        else if(m_Enemy.Agent.remainingDistance <= Mathf.Epsilon)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Listener).Idle);
        }
    }
}
