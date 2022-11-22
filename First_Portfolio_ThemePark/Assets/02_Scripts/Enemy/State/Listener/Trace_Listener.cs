using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace_Listener : EnemyState
{
    public Trace_Listener(Enemy _enemy) : base("Trace", _enemy) { }
   
    public override void EnterState()
    {
        Debug.Log("Trace 입장!");
        // 소리지를 때 이동 멈춰있어야 함

        m_Enemy.Agent.speed = m_Enemy.TraceSpeed;
        m_Enemy.Agent.destination = (m_Enemy as Enemy_Listener).SoundPos;
        m_Enemy.Anim.SetTrigger("IsTrace");
    }

    public override void ExitState()
    {
        Debug.Log("Trace 퇴장!");
        // 소리지르고 주변 둘러보기 애니메이션
    }

    public override void Action()
    {
    }

    public override void CheckState()
    {
        // 플레이어의 거리가 공격사거리보다 짧으면 SetState(Attack)
        if ((m_Enemy.PlayerTr != null) && 
            (Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position) <= (m_Enemy as Enemy_Listener).AttackRange))
        {
            m_Enemy.SetState((m_Enemy as Enemy_Listener).Attack);
            return;
        }

        // 소리가 난 위치까지 도착하면 SetState(Idle)
        else if(m_Enemy.Agent.remainingDistance <= Mathf.Epsilon)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Listener).Idle);
            return;
        }
    }
}
