using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Listener : Enemy
{
   
    #region EnemyState
    private Idle_Listener m_Idle;
    [SerializeField] private AudioSource idleListenerSE;
    public Idle_Listener Idle
    {
        //idleListenerSE.Play();
        get { return m_Idle; }
        
    }

    private Trace_Listener m_TraceTarget;
    public Trace_Listener TraceTarget
    {
        get { return m_TraceTarget; }
    }

    private Concentration_Listener m_Concentration;
    public Concentration_Listener Concentration
    {
        get { return m_Concentration; }
    }
    
    private LookAround_Listener m_LookAround;
    public LookAround_Listener LookAround
    {
        get { return m_LookAround; }
    }

    private Attack m_Attack;
    public Attack Attack
    {
        get { return m_Attack; }
    }
    #endregion

    #region Inspector
    [Space]
    [Header("Range")]
    [SerializeField] protected float m_AttackRange = 1f;
    public float AttackRange
    {
        get { return m_AttackRange; }
    }
    #endregion

    #region Member_variable
    private Vector3 m_SoundPos;
    public Vector3 SoundPos
    {
        get { return m_SoundPos; }
        set { m_SoundPos = value; }
    }

    private float m_CurVolumeLv = 0f;
    public float CurVolumeLv
    {
        set { m_CurVolumeLv = value; }
    }
    #endregion


    protected override void Awake()
    {
        base.Awake();
        m_Idle = new Idle_Listener(this);
        m_TraceTarget = new Trace_Listener(this);
        m_Concentration = new Concentration_Listener(this);
        m_LookAround = new LookAround_Listener(this);
        m_Attack = new Attack(this);
    }

    protected override EnemyState GetInitialState()
    {
        return m_Idle;
    }

    public void Listen(Transform _targetTr, Vector3 _soundPos, float _volumeLevel)
    {
        if (m_CurVolumeLv <= _volumeLevel)
        {
            m_SoundPos = _soundPos;

            if (_targetTr.CompareTag("PLAYER"))
            {
                m_PlayerTr = _targetTr;
            }

            if (m_CurState == m_Idle)
            {
                SetState(m_Concentration);
            }

            //else if (m_CurState == m_TraceTarget || m_CurState == m_Concentration )
            else
            {
                SetState(m_TraceTarget);
            }
        }
    }
}
