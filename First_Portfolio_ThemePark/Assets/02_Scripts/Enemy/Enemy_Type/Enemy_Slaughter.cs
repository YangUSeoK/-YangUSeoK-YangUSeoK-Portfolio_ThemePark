using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Enemy_Slaughter : Enemy
{
    public delegate void VoidVoidDelegate(Enemy_Slaughter _caller);
    VoidVoidDelegate callZombiesAroundDelegate = null;

    #region EnemyState
    private Patrol_Slaughter m_Patrol = null;
    public Patrol_Slaughter Patrol
    {
        get { return m_Patrol; }
    }

    private TraceLight_Slaughter m_TraceLight = null;
    public TraceLight_Slaughter TraceLight
    {
        get { return m_TraceLight; }
    }

    private TracePlayer_Slaughter m_TracePlayer = null;
    public TracePlayer_Slaughter TracePlayer
    {
        get { return m_TracePlayer; }
    }

    private Concentration_Slaughter m_Concentration = null;
    public Concentration_Slaughter Concentration
    {
        get { return m_Concentration; }
    }

    private Attack m_Attack = null;
    public Attack Attack
    {
        get { return m_Attack; }
    }
    #endregion

    #region Inspector
    [Space]
    [Header("Range")]
    [SerializeField] protected float m_PatrolDetectRange = 20f;
    public float PatrolDetectRange
    {
        get { return m_PatrolDetectRange; }
    }

    [SerializeField] protected float m_PatrolPlayerDetectRange = 10f;
    public float PatrolPlayerDetectRange
    {
        get { return m_PatrolPlayerDetectRange; }
    }

    [SerializeField] protected float m_ConcentrationDetectRange = 15f;
    public float ConcentrationDetectRange
    {
        get { return m_ConcentrationDetectRange; }
    }

    [SerializeField] protected float m_TraceDetectRange = 17f;
    public float TraceDetectRange
    {
        get { return m_TraceDetectRange; }
    }

    [SerializeField] protected float m_AttackRange = 1f;
    public float AttackRange
    {
        get { return m_AttackRange; }
    }

    [Space]
    [Header("Detect Angle")]
    [SerializeField] protected float m_PatrolDetectAngle = 120f;
    public float PatrolDetectAngle
    {
        get { return m_PatrolDetectAngle; }
    }

    [SerializeField] protected float m_AlertDetectAngle = 270f;
    public float AlertDetectAngle
    {
        get { return m_AlertDetectAngle; }
    }

    [SerializeField] protected float m_TraceDetectAngle = 180f;
    public float TraceDetectAngle
    {
        get { return m_TraceDetectAngle; }
    }
    #endregion

    #region Member_variable
    private FOV m_FOV = null;
    public FOV FOV
    {
        get { return m_FOV; }
    }

    private Transform m_FlashTr = null;
    public Transform FlashTr
    {
        get { return m_FlashTr; }
        set { m_FlashTr = value; }
    }

    protected Flag[] m_Flags;
    public Flag[] Flags
    {
        get { return m_Flags; }
        set { m_Flags = value; }
    }
    
    private Vector3 m_LightPos = Vector3.zero;
    public Vector3 LightPos
    {
        get { return m_LightPos; }
        set { m_LightPos = value; }
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        m_Patrol = new Patrol_Slaughter(this);
        m_TraceLight = new TraceLight_Slaughter(this);
        m_TracePlayer = new TracePlayer_Slaughter(this);
        m_Concentration = new Concentration_Slaughter(this);
        m_Attack = new Attack(this);

        m_FOV = GetComponent<FOV>();
    }

    protected override EnemyState GetInitialState()
    {
        return m_Patrol;
    }

    public void SetToTraceLight(Transform _flashTr, Vector3 _lightPos)
    {
        m_FlashTr = _flashTr;
        m_LightPos = _lightPos;
    }
    
    public void SetTraceLightToTracePlayer(Vector3 _playerPos)
    {
    }

    public void CallNearZombie()
    {
        float waitTime = 1f;
        StartCoroutine(CallNearZombieWaitSecondCoroutine(waitTime));
    }
    
    private IEnumerator CallNearZombieWaitSecondCoroutine(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);
        callZombiesAroundDelegate?.Invoke(this);
    }

    public void SetDelegate(VoidVoidDelegate _callZombiesAroundCallback)
    {
        //ZombieFactory.SetTracePlayer
        callZombiesAroundDelegate = _callZombiesAroundCallback;
    }
}
