using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public delegate void VoidVoidDelegate();
    private VoidVoidDelegate attackDelegate = null;


    #region Inspector
    [Header("Speed")]
    [SerializeField] protected float m_PatrolSpeed = 5f;
    public float PatrolSpeed
    {
        get { return m_PatrolSpeed; }
    }

    [SerializeField] protected float m_ConcentrationSpeed = 1f;
    public float ConcentrationSpeed
    {
        get { return m_ConcentrationSpeed; }
    }

    [SerializeField] protected float m_TraceSpeed = 8f;
    public float TraceSpeed
    {
        get { return m_TraceSpeed; }
    }
    #endregion

    #region Member_variable
    [SerializeField] protected Transform m_PlayerTr = null;  //디버그
    public Transform PlayerTr
    {
        get { return m_PlayerTr; }
        set { m_PlayerTr = value; }
    }

    protected NavMeshAgent m_Agent = null;
    public NavMeshAgent Agent
    {
        get { return m_Agent; }
    }

    protected EnemyState m_CurState = null;
    public EnemyState CurState
    {
        get { return m_CurState; }
    }

    protected Animator m_Anim = null;
    public Animator Anim
    {
        get { return m_Anim; }
    }

    protected AudioSource[] m_Audio = null;
    public AudioSource[] Audio
    {
        get { return m_Audio; }
    }
    #endregion

    protected virtual void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Anim = GetComponent<Animator>();
        m_Audio = GetComponents<AudioSource>();
    }

    protected void OnEnable()
    {
        m_CurState = GetInitialState();
        if (m_CurState != null)
        {
            m_CurState.EnterState();
        }
    }

    protected void Update()
    {
        m_CurState.CheckState();
        m_CurState.Action();
    }

    // State��
    public void SetState(EnemyState _state)
    {
        if (m_CurState != null)
        {
            m_CurState.ExitState();
        }
        m_CurState = _state;
        m_CurState.EnterState();
    }

    #region Delegate_Callback
    public void IsAttack()
    {
        attackDelegate?.Invoke();
    }

    public void SetDelegate(VoidVoidDelegate _attackCallback)
    {
        attackDelegate = _attackCallback;
    }
    #endregion

    protected abstract EnemyState GetInitialState();
    
}
