using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Find_Stalker : EnemyState
{
    public Find_Stalker(Enemy _enemy) : base("Find", _enemy) { }

    public override void Action()
    {
        // 매 프레임 플레이어의 위치를 갱신해 쫓아간다.
        m_Enemy.Agent.destination = m_Enemy.PlayerTr.position;

        // 발소리 재생
        // 그로울링 소리 재생
    }

    public override void CheckState()
    {
        RaycastHit hitInfo;
        int layerMask = 1 << LayerMask.NameToLayer("PLAYER") | 1 << LayerMask.NameToLayer("OBSTACLE");

        // 플레이어에 레이를 싸쏴서 직접 보고있으면 SetState(Alert);
        Physics.Raycast(m_Enemy.transform.position, m_Enemy.PlayerTr.position - m_Enemy.transform.position, out hitInfo,
                        (m_Enemy as Enemy_Stalker).FindRange, layerMask);
        if (hitInfo.transform.CompareTag("PLAYER"))
        {
            m_Enemy.SetState((m_Enemy as Enemy_Stalker).Alert);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Dog : 냄새가 난다!");
        m_Enemy.Agent.speed = (m_Enemy as Enemy_Stalker).FindSpeed;
        m_Enemy.Anim.SetBool("IsFind", true);

        // 달리는 소리 설정
        // 그로울링 소리 설정(소리 빠르게 등등)
    }

    public override void ExitState()
    {
        Debug.Log("Find Exit");
        m_Enemy.Anim.SetBool("IsFind", false);
    }
}
