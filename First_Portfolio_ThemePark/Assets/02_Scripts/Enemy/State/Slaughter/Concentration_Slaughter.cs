using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concentration_Slaughter : EnemyState
{
    public Concentration_Slaughter(Enemy _enemy) : base("Concentration", _enemy) { }

    private Transform m_FlashTr = null;
    private Vector3 m_LightPos = Vector3.zero;
    private float m_Timer = 0f;

    public override void EnterState()
    {
        Debug.Log("Slaughter : Concentration Enter!");
        Debug.Log("주위를 살펴본다.");
        m_Enemy.Agent.speed = m_Enemy.ConcentrationSpeed;

        m_Timer = 0f;
        m_Enemy.Anim.SetTrigger("IsConcentration");

        // BGM 바꾸기
        m_Enemy.Audio[2].Play();
        (m_Enemy as Enemy_Slaughter).ExitTracePlayerCallback();
    }

    public override void ExitState()
    {
        Debug.Log("Concentration Exit!");
        m_Enemy.Audio[2].Stop();
    }

    public override void Action()
    {

    }

    public override void CheckState()
    {
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);

        // 플레이어를 발견하면
        if ((m_Enemy as Enemy_Slaughter).FOV.IsInFOV((m_Enemy as Enemy_Slaughter).ConcentrationDetectRange, (m_Enemy as Enemy_Slaughter).AlertDetectAngle, LayerMask.NameToLayer("PLAYER"))
            && (m_Enemy as Enemy_Slaughter).FOV.IsLookDirect(m_Enemy.PlayerTr, (m_Enemy as Enemy_Slaughter).PatrolPlayerDetectRange, LayerMask.NameToLayer("PLAYER")))
        {
            Debug.Log("Slaughter : 거기있었구나!");

            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
            m_Timer = 0f;
            return;
        }

        // 빛을 발견하면
        if ((m_Enemy as Enemy_Slaughter).FOV.IsInFovWithRayCheckDirect((m_Enemy as Enemy_Slaughter).ConcentrationDetectRange, (m_Enemy as Enemy_Slaughter).AlertDetectAngle, 
            "LIGHT", (m_Enemy as Enemy_Slaughter).FOV.mLayerMask, ref m_LightPos, ref m_FlashTr))
        {
            Debug.Log("빛인가..");
            (m_Enemy as Enemy_Slaughter).LightPos = m_LightPos;
            (m_Enemy as Enemy_Slaughter).FlashTr = m_FlashTr;
            (m_Enemy as Enemy_Slaughter).SetState((m_Enemy as Enemy_Slaughter).TraceLight);
            m_Timer = 0f;
            return;
        }
        
        AggroCheck();
    }

    private void AggroCheck()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer >= 10f)
        {
            (m_Enemy as Enemy_Slaughter).SetState((m_Enemy as Enemy_Slaughter).Patrol);
        }
        Debug.Log($"Concentration : {m_Timer}");
    }
}