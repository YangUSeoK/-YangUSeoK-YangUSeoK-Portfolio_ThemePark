using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle_Listener : EnemyState
{
    public Idle_Listener(Enemy _enemy) : base("Idle", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log("Idle ¿‘¿Â!");
        m_Enemy.Agent.speed = 0f;
        m_Enemy.Anim.SetTrigger("IsIdle");
    }

    public override void ExitState()
    {
        Debug.Log("Idle ≈¿Â!");
    }

    public override void Action()
    {
    }

    public override void CheckState()
    {
    }
}
