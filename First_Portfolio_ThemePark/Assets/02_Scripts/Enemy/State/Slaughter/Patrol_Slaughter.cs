using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Patrol_Slaughter : EnemyState
{
    // 생성자
    public Patrol_Slaughter(Enemy _enemy) : base("Patrol", _enemy) { }

    
    private Transform m_FlashTr = null;
    private Vector3 m_LightPos = Vector3.zero;
    private int mNextIdx = 0;


    public override void EnterState()
    {
        Debug.Log($"{m_Enemy.name} Patrol Enter!");
        m_Enemy.Agent.speed = m_Enemy.PatrolSpeed;
        m_Enemy.Anim.SetBool("IsPatrol",true);
        m_Enemy.Audio[0].Play();
        (m_Enemy as Enemy_Slaughter).EnterPatrolCallback();
        m_Enemy.Mr.material.color = Color.white;
    }

    public override void ExitState()
    {
        Debug.Log($"{m_Enemy.name} Patrol Exit!");
        m_Enemy.Audio[0].Stop();
        m_Enemy.Anim.SetBool("IsPatrol", false);
    }

    public override void Action()
    {
        // 플래그 순찰
        if (m_Enemy.Agent.remainingDistance <= 0.5f)
        {
            mNextIdx = Random.Range(0,(m_Enemy as Enemy_Slaughter).Flags.Length);
            PatrollFlags();
        }
    }

    public override void CheckState()
    {
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);

        // 플레이어가 FOV 안에 있고 가로막는게 없다면
        if ((m_Enemy as Enemy_Slaughter).FOV.IsInFOV((m_Enemy as Enemy_Slaughter).PatrolPlayerDetectRange, (m_Enemy as Enemy_Slaughter).PatrolDetectAngle, LayerMask.NameToLayer("PLAYER"))
            && (m_Enemy as Enemy_Slaughter).FOV.IsLookDirect(m_Enemy.PlayerTr, (m_Enemy as Enemy_Slaughter).PatrolPlayerDetectRange, LayerMask.NameToLayer("PLAYER")))
        {
           // Debug.Log("Slaughter : 인간이다!!");
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
            return;
        }

        // 위에서 걸리면 return 이라서 if 써도됨
        // 부채꼴로 레이를 쏴서 빛을 감지한다면
        if ((m_Enemy as Enemy_Slaughter).FOV.IsInFovWithRayCheckDirect((m_Enemy as Enemy_Slaughter).PatrolDetectRange, (m_Enemy as Enemy_Slaughter).PatrolDetectAngle,
            "LIGHT", (m_Enemy as Enemy_Slaughter).FOV.mLayerMask, ref m_LightPos, ref m_FlashTr)) 
        {
           // Debug.Log("���̴�!! ���� �ֳ�?");

            // 빛을 본 정보를 m_Enemy에게 전달
            (m_Enemy as Enemy_Slaughter).LightPos = m_LightPos;
            (m_Enemy as Enemy_Slaughter).FlashTr = m_FlashTr;
            
            (m_Enemy as Enemy_Slaughter).SetState((m_Enemy as Enemy_Slaughter).TraceLight);
            return;
        }
    }

    public void PatrollFlags()
    {
        // 위치 정한대로 순찰
        if (m_Enemy.Agent.isPathStale)
        {
            return;
        }
        m_Enemy.Agent.destination = (m_Enemy as Enemy_Slaughter).Flags[mNextIdx].transform.position;
    }
}
