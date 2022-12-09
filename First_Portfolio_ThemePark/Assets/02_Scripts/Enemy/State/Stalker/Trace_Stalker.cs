using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace_Stalker : EnemyState
{
    public Trace_Stalker(Enemy _enemy) : base("Trace", _enemy) { }

    public override void Action()
    {
        // 걷는 소리 재생
        // 그로울링 소리 재생
    }

    public override void CheckState()
    {
        if(Vector3.Distance(m_Enemy.transform.position, m_Enemy.PlayerTr.position) <= (m_Enemy as Enemy_Stalker).TraceRange)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Stalker).Find);
        }
        
        else if (m_Enemy.Agent.remainingDistance <= 0.5f)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Stalker).Sniff);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Dog : Trace Enter!");
        m_Enemy.Agent.destination = m_Enemy.PlayerTr.position;
        m_Enemy.Agent.speed = (m_Enemy as Enemy_Stalker).TraceSpeed;

        m_Enemy.Anim.SetBool("IsTrace", true);

        // 걷는 소리 설정
        // 그로울링 소리 설정
    }

    public override void ExitState()
    {
        Debug.Log("Dog : Trace Exit!");
        m_Enemy.Anim.SetBool("IsTrace", false);
    }
}
