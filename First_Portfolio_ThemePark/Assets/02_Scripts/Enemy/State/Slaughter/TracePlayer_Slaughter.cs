using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class TracePlayer_Slaughter : EnemyState
{
    public TracePlayer_Slaughter(Enemy _enemy) : base("TracePlayer", _enemy) { }



    private Vector3 m_PlayerPos = Vector3.zero; // 마지막으로 본 플레이어 위치 저장변수
    private bool mbIsLookPlayer = false;
    private float m_Timer = 0f;

    public override void EnterState()
    {
        Debug.Log($"{m_Enemy.name} TracePlayer 입장!");
        Debug.Log("주변 좀비를 부릅니다!");
        m_Enemy.Agent.speed = m_Enemy.TraceSpeed;
        m_PlayerPos = m_Enemy.PlayerTr.position;
        m_Enemy.Agent.destination = m_PlayerPos;

        m_Timer = 0f;
        m_Enemy.Anim.SetBool("IsTracePlayer", true);

        // 주변 좀비 부르는 함수
        (m_Enemy as Enemy_Slaughter).CallNearZombie();

        // BGM 바꾸기
        m_Enemy.Audio[3].Play();
        (m_Enemy as Enemy_Slaughter).EnterTracePlayerCallback();

        m_Enemy.Mr.material.color = Color.red;
    }

    public override void ExitState()
    {
        Debug.Log($"{m_Enemy.name} TracePlayer 퇴장!");
        m_Enemy.Anim.SetBool("IsTracePlayer", false);

        // BGM 바꾸기
        m_Enemy.Audio[2].Play();
        (m_Enemy as Enemy_Slaughter).ExitTracePlayerCallback();
    }

    public override void Action()
    {
        // 플레이어에게 레이를 쏴서 위치정보 업데이트 받음
        RaycastHit hitInfo;
        int layerMask = ((m_Enemy as Enemy_Slaughter).FOV.PlayerLayer | (m_Enemy as Enemy_Slaughter).FOV.ObstacleLayer);

        Debug.DrawLine(m_Enemy.transform.position, m_Enemy.PlayerTr.position, Color.blue);

        if (Physics.Raycast(m_Enemy.transform.position + (Vector3.up * 0.7f), m_Enemy.PlayerTr.position - m_Enemy.transform.position,
            out hitInfo, (m_Enemy as Enemy_Slaughter).TraceDetectRange + 30f, layerMask) 
            && hitInfo.collider.CompareTag("PLAYER"))
        {
            Debug.Log($" TracePlayer 무릎레이져 맞은놈 : {m_Enemy.transform.name} / {hitInfo.transform.name}");

            mbIsLookPlayer = true;
            m_PlayerPos = hitInfo.transform.position;
        }
        else if (Physics.Raycast(m_Enemy.transform.position + (Vector3.up * 1.4f), m_Enemy.PlayerTr.position - m_Enemy.transform.position,
                out hitInfo, (m_Enemy as Enemy_Slaughter).TraceDetectRange + 30f, layerMask) 
                && hitInfo.collider.CompareTag("PLAYER"))
        {
            Debug.Log($" TracePlayer 눈레이져 맞은놈 : {m_Enemy.transform.name} / {hitInfo.transform.name}");
                mbIsLookPlayer = true;
                m_PlayerPos = hitInfo.transform.position;
        }
        else
        {
            mbIsLookPlayer = false;
        }
        Debug.Log(mbIsLookPlayer);
        m_Enemy.Agent.destination = m_PlayerPos;
    }

    public override void CheckState()
    {
        if (mbIsLookPlayer)
        {
            float distance = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);
            m_Timer = 0;
            if (distance <= (m_Enemy as Enemy_Slaughter).AttackRange)
            {
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Attack);
                return;
            }
        }
        else
        {
            m_Timer += Time.deltaTime;
            //Debug.Log($"Trace Player : {m_Timer}");

            if (m_Timer >= 5f)
            {
                Debug.Log("어디갔지?");
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Concentration);
            }
        }
    }
}
