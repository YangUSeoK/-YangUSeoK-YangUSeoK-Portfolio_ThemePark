using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Idle_Listener : EnemyState
{
    public Idle_Listener(Enemy _enemy) : base("Idle", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log($"{m_Enemy.name} Idle ¿‘¿Â!");
        m_Enemy.Agent.speed = 0f;
        m_Enemy.Anim.SetBool("IsIdle", true);
        m_Enemy.Audio[0].Play();
        (m_Enemy as Enemy_Listener).CurVolumeLv = 0f;

        m_Enemy.Mr.material.color = Color.white;

    }

    public override void ExitState()
    {
        Debug.Log($"{m_Enemy.name} Idle ≈¿Â!");
        m_Enemy.Anim.SetBool("IsIdle", false);
        m_Enemy.Audio[0].Stop();
    }

    public override void Action()
    {
    }

    public override void CheckState()
    {
    }
}
