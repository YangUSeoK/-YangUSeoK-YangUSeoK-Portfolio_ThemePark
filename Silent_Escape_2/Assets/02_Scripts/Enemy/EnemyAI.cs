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
    // 디버그용
    [SerializeField] protected EState mState;

    // 20221104 양우석 델리게이트로 연결해야 함.
    [SerializeField] protected Transform m_PlayerTr = null;

    protected Enemy_SightMan m_Enemy = null;


    protected EnemyFOV m_EnemyFOV = null;
    protected EnemyMove m_MoveAgent = null;

    //protected float m_PlayerLookRange = 0f;
    //protected float m_SoundDetectRange = 0f;
    //protected float m_AttackRange = 0f;


    protected Vector3 oriPos = Vector3.zero;
    protected WaitForSeconds ws = new WaitForSeconds(0.1f);

    // 디버그용
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
        // 현재상황 체크 코루틴 (무한반복)
        StartCoroutine(CheckStateCoroutine());

        // 현재상황에 맞는 액션 코루틴 (무한반복)
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
    // 소리듣기맨
    //            if (mMyType == EType.SoundMan)
    //            {
    //                // if( 소리를 10초안에 1초간격으로 3번 들으면  ) {mState = EState.Tracer}

    //                // else if로 바꿔야 함. 소리를 들으면

    //                // 20221104 양우석  PlayerMove가 없어서 받아올 수 없음.
    //                //if (dist <= mSoundDetectRange + m_PlayerTr.GetComponent<PlayerMove>().GetNoise())
    //                //{
    //                //    mState = EState.Alert;
    //                //}

    //                //else
    //                //{
    //                //    mState = EState.Patrol;
    //                //}
    //            }

    //            // 멍멍이
    //            if (mMyType == EType.ZombieDog)
    //            {
    //                // if(눈으로 플레이어를 발견하면) { mState = EState.Tracer }

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
    //                Debug.Log("뭔소리야");

    //                break;

    //            case EState.Trace:
    //                mbIsTrace = true;   // 한번 추적상태 들어오면 절대 안멈춘다. (숨거나, 죽거나 둘중하나)
    //                m_MoveAgent.TraceTarget(m_PlayerTr.position);
    //                break;



    //            // 멍멍이용 코드
    //            //if (mbIsTracer && !mbCanTraceSet && moveAgent.Agent.remainingDistance <= 2f)
    //            //{
    //            //    mbIsTracer = false;
    //            //    mbCanTraceSet = true;  << 지웠음
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
