using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Idle_Listener : EnemyState
{
    public Idle_Listener(Enemy _enemy) : base("Idle", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log("Idle ¿‘¿Â!");
        m_Enemy.Agent.speed = 0f;
        //m_Enemy.Anim.SetTrigger("IsIdle");
        m_Enemy.Audio[0].Play();
    }

    public override void ExitState()
    {
        Debug.Log("Idle ≈¿Â!");
        m_Enemy.Audio[0].Stop();
        m_Enemy.Anim.SetTrigger("IsTraceSound");
        m_Enemy.Audio[1].Play();
    }

    public override void Action()
    {
    }

    public override void CheckState()
    {
    }
}
