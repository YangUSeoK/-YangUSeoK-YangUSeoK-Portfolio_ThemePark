using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyState
{
    public EnemyState(string _name, Enemy _enemy)
    {
        m_Name = _name;
        m_Enemy = _enemy;
    }

    protected string m_Name;
    public string Name
    {
        get { return m_Name; }
    }

    protected Enemy m_Enemy;
    protected NavMeshAgent m_Agent;
    


    public abstract void EnterState();
    public abstract void CheckState();
    public abstract void Action();
    public abstract void ExitState();
}
