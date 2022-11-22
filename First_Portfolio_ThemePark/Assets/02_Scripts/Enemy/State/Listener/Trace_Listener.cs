using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace_Listener : EnemyState
{
    public Trace_Listener(Enemy _enemy) : base("Trace", _enemy) { }
   
    public override void EnterState()
    {
        Debug.Log("Trace ����!");
        // �Ҹ����� �� �̵� �����־�� ��

        m_Enemy.Agent.speed = m_Enemy.TraceSpeed;
        m_Enemy.Agent.destination = (m_Enemy as Enemy_Listener).SoundPos;
        m_Enemy.Anim.SetTrigger("IsTrace");
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
        // �÷��̾��� �Ÿ��� ���ݻ�Ÿ����� ª���� SetState(Attack)
        if ((m_Enemy.PlayerTr != null) && 
            (Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position) <= (m_Enemy as Enemy_Listener).AttackRange))
        {
            m_Enemy.SetState((m_Enemy as Enemy_Listener).Attack);
            return;
        }

        // �Ҹ��� �� ��ġ���� �����ϸ� SetState(Idle)
        else if(m_Enemy.Agent.remainingDistance <= Mathf.Epsilon)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Listener).Idle);
            return;
        }
    }
}
