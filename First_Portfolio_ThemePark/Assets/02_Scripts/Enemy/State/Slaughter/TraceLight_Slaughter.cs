using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TraceLight_Slaughter : EnemyState
{
    public TraceLight_Slaughter(Enemy _enemy) : base("TraceLight", _enemy) { }

    private Vector3 m_LightPos = Vector3.zero;
    private Transform m_FlashTr = null;




    public override void EnterState()
    {
        Debug.Log($"{m_Enemy.name} TraceLight 입장!");

        if (m_FlashTr == null)
        {
            m_FlashTr = (m_Enemy as Enemy_Slaughter).FlashTr;
        }
        m_Enemy.Agent.speed = m_Enemy.PatrolSpeed;
        m_Enemy.Agent.destination = (m_Enemy as Enemy_Slaughter).LightPos;
        m_Enemy.Audio[1].Play();
        m_Enemy.Mr.material.color = Color.yellow;
    }

    public override void Action()
    {
        // 빛이 보이는지 체크 -> 빛이 보이면 빛 위치를 저장
        // 저장한 위치로 이동
        (m_Enemy as Enemy_Slaughter).FOV.IsInFovWithRayCheckDirect((m_Enemy as Enemy_Slaughter).TraceDetectRange, (m_Enemy as Enemy_Slaughter).TraceDetectAngle,
                                        "LIGHT", (m_Enemy as Enemy_Slaughter).FOV.mLayerMask, ref m_LightPos, ref m_FlashTr);
        m_Enemy.Agent.destination = new Vector3(m_LightPos.x, m_Enemy.transform.position.y, m_LightPos.z);
    }

    public override void CheckState()
    {
        // 손전등 위치로 계속 레이를 쏜다.
        RaycastHit hitInfo;
        int layerMask = ((m_Enemy as Enemy_Slaughter).FOV.ObstacleLayer) | ((m_Enemy as Enemy_Slaughter).FOV.PlayerLayer);
        //| ~(1 << (m_Enemy as Enemy_Slaughter).FOV.LightLayer);
        

        // 플레이어를 직접 본다면
        if (Physics.Raycast(m_Enemy.transform.position + (Vector3.up) * 1.4f, m_Enemy.PlayerTr.position - m_Enemy.transform.position,
            out hitInfo, 100f, layerMask))
        {
            Debug.Log(hitInfo.transform.name);
            // 가로막는게 없고 플레이어가 손전등을 들고있다면 => TracePlayer
            // 20221116 양우석:  플레이어랑 거리 실제로 맞춰보고 수정해야 함.
            // 20221128 양우석 : 빛을 본 좀비가 다른위치의 플레이어를 바로 쫓아오는거 수정해야 함
            if (hitInfo.collider.CompareTag("PLAYER"))
            {
                Debug.Log("가로막는게 없어서 쩨꼈다!!");
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
                return;
            }
        }
        else if (Physics.Raycast(m_Enemy.transform.position + (Vector3.up) * 0.7f, m_Enemy.PlayerTr.position - m_Enemy.transform.position,
                out hitInfo, 100f, layerMask))
        {
            Debug.Log($"TraceLight 무릎레이져 : {hitInfo.transform.name}");
            if (hitInfo.collider.CompareTag("PLAYER"))
            {
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
                return;
            }
        }

        Debug.DrawLine(m_Enemy.transform.position, m_LightPos, Color.blue);

        // 레이가 안맞았고 마지막 빛 위치랑 현재 위치가 같으면  => Concentration
        if (Vector3.Distance(new Vector3(m_LightPos.x, m_Enemy.transform.position.y, m_LightPos.z), m_Enemy.transform.position) <= 0.3f)
        {
            Debug.Log("아무것도 없나?");
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Concentration);
        }
    }



    public override void ExitState()
    {
        Debug.Log($"{m_Enemy.name} TraceLight 퇴장!");
    }
}
