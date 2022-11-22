using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Enemy_Slaughter : Enemy
{

    public delegate void VoidVoidDelegate(Enemy_Slaughter _caller);
    VoidVoidDelegate callZombiesAroundDelegate = null;


    // EnemyManager가 먹여줘야 함 => 20221122 양우석 : 완
    protected Flag[] m_Flags;
    public Flag[] Flags
    {
        set { m_Flags = value; }
    }

    // EnemyState 프로퍼티
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
    public Concentration_Slaughter Alert
    {
        get { return m_Concentration; }
    }
    private Attack m_Attack = null;
    public Attack Attack
    {
        get { return m_Attack; }
    }
    #endregion

    private Transform m_FlashTr = null;
    public Transform FlashTr
    {
        get { return m_FlashTr; }
        set { m_FlashTr = value; }
    }

    // FOV
    private FOV m_FOV = null;
    private Vector3 m_LightPos = Vector3.zero;

   

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

    public void SetPatrol()
    {
        m_Patrol.FOV = m_FOV;
        m_Patrol.Agent = m_Agent;
        m_Patrol.MoveSpeed = m_PatrolSpeed;
        m_Patrol.Flags = m_Flags;
    }

    public void SetTraceLight()
    {
        m_TraceLight.FOV = m_FOV;
        m_TraceLight.Agent = m_Agent;
        m_TraceLight.MoveSpeed = m_PatrolSpeed;
        m_TraceLight.LightPos = m_LightPos;
        m_TraceLight.FlashTr = m_FlashTr;
    }

    public void SetTracePlayer()
    {
        m_TracePlayer.FOV = m_FOV;
        m_TracePlayer.Agent = m_Agent;
        m_TracePlayer.MoveSpeed = m_TracePlayerSpeed;
        m_TracePlayer.PlayerPos = m_PlayerTr.position;
    }

    public void SetConcentration()
    {
        m_Concentration.FOV = m_FOV;
        m_Concentration.Agent = m_Agent;
        m_Concentration.MoveSpeed = m_ConcentrationSpeed;
    }

    public void SetAttack()
    {
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
