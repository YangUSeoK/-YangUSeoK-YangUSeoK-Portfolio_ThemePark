using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Patrol_Slaughter : EnemyState
{
    // ������
    public Patrol_Slaughter(Enemy _enemy) : base("Patrol", _enemy) { }

    
    private Transform m_FlashTr = null;
    private Vector3 m_LightPos = Vector3.zero;
    private int mNextIdx = 0;


    public override void EnterState()
    {
       // Debug.Log("Patrol ����!");
        m_Enemy.Agent.speed = m_Enemy.PatrolSpeed;
        m_Enemy.Anim.SetTrigger("IsPatrol");
    }

    public override void ExitState()
    {
       // Debug.Log("Patrol ����!");
    }

    public override void Action()
    {
        // ���� �� �� ���
        if (m_Enemy.Agent.remainingDistance <= 0.5f)
        {
            mNextIdx = Random.Range(0,(m_Enemy as Enemy_Slaughter).Flags.Length);
            PatrollFlags();
        }
    }

    public override void CheckState()
    {
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);

        // �÷��̾ �����ȿ� ��������
        if ((m_Enemy as Enemy_Slaughter).FOV.IsInFOV((m_Enemy as Enemy_Slaughter).PatrolPlayerDetectRange, (m_Enemy as Enemy_Slaughter).PatrolDetectAngle, LayerMask.NameToLayer("PLAYER"))
            && (m_Enemy as Enemy_Slaughter).FOV.IsLookDirect(m_Enemy.PlayerTr, (m_Enemy as Enemy_Slaughter).PatrolPlayerDetectRange, LayerMask.NameToLayer("PLAYER")))
        {
           // Debug.Log("�ΰ��̴�!!");
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
            return;
        }

        // ���� �����ȿ� ��������
        // ������ �ι� �˻��ؾ��ϴ°Ŵϱ� else if �Ƚᵵ ����.
        if ((m_Enemy as Enemy_Slaughter).FOV.IsInFovWithRayCheckDirect((m_Enemy as Enemy_Slaughter).PatrolDetectRange, (m_Enemy as Enemy_Slaughter).PatrolDetectAngle,
            "LIGHT", (m_Enemy as Enemy_Slaughter).FOV.mLayerMask, ref m_LightPos, ref m_FlashTr)) 
        {
           // Debug.Log("���̴�!! ���� �ֳ�?");

            // ����Ʈ����, �÷���Tr�� Enemy_Slaughter�� �Ѱ���. TraceLight���� ����� ��
            (m_Enemy as Enemy_Slaughter).LightPos = m_LightPos;
            (m_Enemy as Enemy_Slaughter).FlashTr = m_FlashTr;
            
            (m_Enemy as Enemy_Slaughter).SetState((m_Enemy as Enemy_Slaughter).TraceLight);
            return;
        }
    }

    public void PatrollFlags()
    {
        // ��� ������� ���� ����
        if (m_Enemy.Agent.isPathStale)
        {
            return;
        }
        m_Enemy.Agent.destination = (m_Enemy as Enemy_Slaughter).Flags[mNextIdx].transform.position;
    }
}
