using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stalker : Enemy
{
    #region EnemyState
    private Sniff_Stalker m_Sniff = null;
    public Sniff_Stalker Sniff
    {
        get { return m_Sniff; }
    }
    
    private Trace_Stalker m_Trace = null;
    public Trace_Stalker Trace
    {
        get { return m_Trace; }
    }

    private Find_Stalker m_Find = null;
    public Find_Stalker Find
    {
        get { return m_Find; }
    }

    private Alert_Stalker m_Alert = null;
    public Alert_Stalker Alert
    {
        get { return m_Alert; }
    }
    #endregion


    [SerializeField] private float m_FindSpeed = 0f;
    public float FindSpeed
    {
        get { return m_FindSpeed; }
    }

    [SerializeField] private float m_TraceRange = 10f;
    public float TraceRange
    {
        get { return m_TraceRange; }
    }    

    [SerializeField] private float m_FindRange = 4f;
    public float FindRange
    {
        get { return m_FindRange; }
    }

    [SerializeField] private float m_AlertRange = 50f;
    public float AlertRange
    {
        get { return m_AlertRange; }
    }

    // 으르렁 소리 거리 재생할 범위
    [SerializeField] private float m_GrowlingRange = 30f;
    public float GrowlingRange
    {
        get { return m_GrowlingRange; }
    }

    [SerializeField] private float m_SniffTime = 2.5f;
    public float SniffTime
    {
        get { return m_SniffTime; }
    }

    [SerializeField] private float m_AlertTime = 10f;
    public float AlertTime
    {
        get { return m_AlertTime; }
    }


    protected override void Awake()
    {
        base.Awake();
        m_Sniff = new Sniff_Stalker(this);
        m_Trace = new Trace_Stalker(this);
        m_Find = new Find_Stalker(this);
        m_Alert = new Alert_Stalker(this);

    }


    protected override EnemyState GetInitialState()
    {
        return m_Sniff;
    }



}
