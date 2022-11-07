using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    // 20221104 ��������Ʈ�� �����ؾ� ��
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
        // ���� �� �� ���
        if (m_Agent.remainingDistance <= 0.5f)
        {
            // �ε����� ����ȣ���� ���� �ٽ� ó������. %�� ������
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
    //        default: // ������
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
        // ��� ������� ���� ����
        if (m_Agent.isPathStale)
        {
            return;
        }
        m_Agent.destination = m_Flags[mNextIdx].transform.position;
        m_Agent.isStopped = false;
    }
}

