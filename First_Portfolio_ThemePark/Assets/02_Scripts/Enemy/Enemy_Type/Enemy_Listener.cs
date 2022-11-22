using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Listener : Enemy
{
    // EnemyState 프로퍼티
    #region EnemyState
    private Idle_Listener m_Idle;
    public Idle_Listener Idle
    {
        get { return m_Idle; }
    }
    private Trace_Listener m_TraceTarget;
    public Trace_Listener TraceTarget
    {
        get { return m_TraceTarget; }
    }
    private Concentration_Listener m_Concentration;
    public Concentration_Listener Alert
    {
        get { return m_Concentration; }
    }
    private Attack m_Attack;
    public Attack Attack
    {
        get { return m_Attack; }
    }
    #endregion

    private Vector3 m_SoundPos;
    public Vector3 SoundPos
    {
        get { return m_SoundPos; }
        set { m_SoundPos = value; }
    }


    protected override void Awake()
    {
        base.Awake();
        m_Idle = new Idle_Listener(this);
        m_TraceTarget = new Trace_Listener(this);
        m_Concentration = new Concentration_Listener(this);
        m_Attack = new Attack(this);
    }

    protected override EnemyState GetInitialState()
    {
        return m_Idle;
    }

    public void SetIdle()
    {

    }

    public void SetTraceTarget()
    {
    }

    public void SetConcentration()
    {
    }

    public void SetAttack()
    {
    }

    public void Listen(Vector3 _soundPos)
    {
        m_SoundPos = _soundPos;
        SetState(m_TraceTarget);
    }




}
