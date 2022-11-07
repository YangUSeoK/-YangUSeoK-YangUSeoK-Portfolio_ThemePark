using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    // 20221104 델리게이트로 연결해야 함
    [SerializeField] private FlagManager m_FlagManager = null;
    private Enemy m_Enemy = null;
    private Flag[] m_Flags;
    public NavMeshAgent m_Agent = null;

    private int mNextIdx = 0;
    


    private void Awake()
    {
        m_Enemy = GetComponent<Enemy>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.autoBraking = false;
        m_Agent.speed = m_Enemy.CurSpeed;
    }

    private void Start()
    {
        m_Flags = m_FlagManager.Flags;
        patrollFlags();
    }

    private void Update()
    {
        // 다음 갈 곳 계산
        if (m_Agent.remainingDistance <= 0.5f)
        {
            // 인덱스가 끝번호까지 가면 다시 처음으로. %랑 같은거
            ++mNextIdx;
            if (mNextIdx >= m_Flags.Length)
            {
                mNextIdx -= m_Flags.Length;
            }
            patrollFlags();
        }
    }

    public void Stop()
    {
        m_Agent.isStopped = true;
        m_Agent.velocity = Vector3.zero;
    }

    //public void SetSpeed(int _state)
    //{
    //    switch (_state)
    //    {
    //        case 0: // Patrol
    //            m_Enemy.SetSpeed(1.5f);
    //            break;
    //        case 1: // Alert
    //            m_Enemy.SetSpeed(1f);
    //            break;
    //        case 2: // Trace
    //            m_Enemy.SetSpeed(5f);
    //            break;
    //        case 3: // 
    //            m_Enemy.SetSpeed(0f);
    //            break;
    //        default: // 나머지
    //            m_Enemy.SetSpeed(0f); 
    //            break;
    //    }
    //}

    public void TraceTarget(Vector3 _pos)
    {
        if (m_Agent.isPathStale)
        {
            return;
        }
        m_Agent.destination = _pos;
        m_Agent.isStopped = false;
    }

    public void patrollFlags()
    {
        // 경로 계산중일 때는 리턴
        if (m_Agent.isPathStale)
        {
            return;
        }
        m_Agent.destination = m_Flags[mNextIdx].transform.position;
        m_Agent.isStopped = false;
    }
}

