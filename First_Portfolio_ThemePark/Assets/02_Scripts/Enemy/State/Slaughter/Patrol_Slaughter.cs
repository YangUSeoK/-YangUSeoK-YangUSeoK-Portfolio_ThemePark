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
        Debug.Log("Patrol 입장!");
        m_Enemy.Agent.speed = m_Enemy.PatrolSpeed;
        m_Enemy.Anim.SetTrigger("IsPatrol");
    }

    public override void ExitState()
    {
        Debug.Log("Patrol 퇴장!");
    }

    public override void Action()
    {
        // 다음 갈 곳 계산
        if (m_Enemy.Agent.remainingDistance <= 0.5f)
        {
            mNextIdx = Random.Range(0,(m_Enemy as Enemy_Slaughter).Flags.Length);
            PatrollFlags();
        }
    }

    public override void CheckState()
    {
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);

        // 플레이어가 범위안에 들어왔으면
        if ((m_Enemy as Enemy_Slaughter).FOV.IsInFOV((m_Enemy as Enemy_Slaughter).PatrolPlayerDetectRange, (m_Enemy as Enemy_Slaughter).PatrolDetectAngle, LayerMask.NameToLayer("PLAYER"))
            && (m_Enemy as Enemy_Slaughter).FOV.IsLookDirect(m_Enemy.PlayerTr, (m_Enemy as Enemy_Slaughter).PatrolPlayerDetectRange, LayerMask.NameToLayer("PLAYER")))
        {
            Debug.Log("인간이다!!");
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
            return;
        }

        // 빛이 범위안에 들어왔으면
        // 어차피 두번 검사해야하는거니까 else if 안써도 같다.
        if ((m_Enemy as Enemy_Slaughter).FOV.IsInFovWithRayCheckDirect((m_Enemy as Enemy_Slaughter).PatrolDetectRange, (m_Enemy as Enemy_Slaughter).PatrolDetectAngle,
            "LIGHT", (m_Enemy as Enemy_Slaughter).FOV.mLayerMask, ref m_LightPos, ref m_FlashTr)) 
        {
            Debug.Log("빛이다!! 누가 있나?");

            // 라이트포즈, 플래시Tr을 Enemy_Slaughter에 넘겨줌. TraceLight에서 사용할 것
            (m_Enemy as Enemy_Slaughter).LightPos = m_LightPos;
            (m_Enemy as Enemy_Slaughter).FlashTr = m_FlashTr;
            
            (m_Enemy as Enemy_Slaughter).SetState((m_Enemy as Enemy_Slaughter).TraceLight);
            return;
        }
    }

    public void PatrollFlags()
    {
        // 경로 계산중일 때는 리턴
        if (m_Enemy.Agent.isPathStale)
        {
            return;
        }
        m_Enemy.Agent.destination = (m_Enemy as Enemy_Slaughter).Flags[mNextIdx].transform.position;
    }
}
