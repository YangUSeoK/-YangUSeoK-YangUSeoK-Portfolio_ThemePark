using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyAI : MonoBehaviour
{
    protected enum EType
    {
        SightMan = 0,
        SoundMan,
        ZombieDog,

        Length
    }
    [SerializeField] protected EType m_MyType;

    protected enum EState
    {
        Patrol = 0,
        Alert,
        Trace,
        Attack,

        Length
    }
    // ����׿�
    [SerializeField] protected EState mState;

    // 20221104 ��켮 ��������Ʈ�� �����ؾ� ��.
    [SerializeField] protected Transform m_PlayerTr = null;

    protected Enemy_SightMan m_Enemy = null;


    protected EnemyFOV m_EnemyFOV = null;
    protected EnemyMove m_MoveAgent = null;

    //protected float m_PlayerLookRange = 0f;
    //protected float m_SoundDetectRange = 0f;
    //protected float m_AttackRange = 0f;


    protected Vector3 oriPos = Vector3.zero;
    protected WaitForSeconds ws = new WaitForSeconds(0.1f);

    // ����׿�
    [SerializeField] protected bool mbIsTrace = false;
    [SerializeField] protected bool mbIsAlert = false;

    protected virtual void Awake()
    {
        m_Enemy = GetComponent<Enemy_SightMan>();
        m_EnemyFOV = GetComponent<EnemyFOV>();
        m_MoveAgent = GetComponent<EnemyMove>();
    }
    
    protected void Start()
    {
        // �����Ȳ üũ �ڷ�ƾ (���ѹݺ�)
        StartCoroutine(CheckStateCoroutine());

        // �����Ȳ�� �´� �׼� �ڷ�ƾ (���ѹݺ�)
        StartCoroutine(ActionCoroutine());
    }

    //public void SetRange(float _lookDetectRange, float _soundDetectRange, float _attackRange)
    //{
    //    m_PlayerLookRange = _lookDetectRange;
    //    m_SoundDetectRange = _soundDetectRange;
    //    m_AttackRange = _attackRange;
    //}


    protected abstract IEnumerator CheckStateCoroutine();
    //{
    // �Ҹ�����
    //            if (mMyType == EType.SoundMan)
    //            {
    //                // if( �Ҹ��� 10�ʾȿ� 1�ʰ������� 3�� ������  ) {mState = EState.Tracer}

    //                // else if�� �ٲ�� ��. �Ҹ��� ������

    //                // 20221104 ��켮  PlayerMove�� ��� �޾ƿ� �� ����.
    //                //if (dist <= mSoundDetectRange + m_PlayerTr.GetComponent<PlayerMove>().GetNoise())
    //                //{
    //                //    mState = EState.Alert;
    //                //}

    //                //else
    //                //{
    //                //    mState = EState.Patrol;
    //                //}
    //            }

    //            // �۸���
    //            if (mMyType == EType.ZombieDog)
    //            {
    //                // if(������ �÷��̾ �߰��ϸ�) { mState = EState.Tracer }

    //                // else{ mState = EState.Patrol;}
    //            }
    //    
    //}
    protected abstract IEnumerator ActionCoroutine();
    //{
    //    while (true)
    //    {
    //        yield return ws;

    //        switch (mState)
    //        {
    //            case EState.Patrol:
    //                m_MoveAgent.patrollFlags();
    //                break;

    //            case EState.Alert:
    //                m_MoveAgent.Stop();
    //                m_MoveAgent.SetSpeed((int)mState);
    //                m_MoveAgent.TraceTarget(m_PlayerTr.position);
    //                Debug.Log("���Ҹ���");

    //                break;

    //            case EState.Trace:
    //                mbIsTrace = true;   // �ѹ� �������� ������ ���� �ȸ����. (���ų�, �װų� �����ϳ�)
    //                m_MoveAgent.TraceTarget(m_PlayerTr.position);
    //                break;



    //            // �۸��̿� �ڵ�
    //            //if (mbIsTracer && !mbCanTraceSet && moveAgent.Agent.remainingDistance <= 2f)
    //            //{
    //            //    mbIsTracer = false;
    //            //    mbCanTraceSet = true;  << ������
    //            //    mState = EState.Alert;
    //            //    yield return new WaitForSeconds(2f);
    //            //}


    //            case EState.Attack:
    //                m_MoveAgent.Stop();
    //                mbIsTrace = false;
    //                Debug.Log("Die");
    //                StopAllCoroutines();
    //                // PlayerDie();
    //                break;
    //        }
    //    }
    //}
    

    public void SetState(string _state)
    {
        mState = (EState)Enum.Parse(typeof(EState), _state);
    }


}
