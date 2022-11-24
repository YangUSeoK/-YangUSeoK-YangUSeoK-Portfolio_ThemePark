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
        Debug.Log("Concentration ����!");
        Debug.Log("���� ������ ���̴ϴ�.");
        m_Enemy.Agent.speed = m_Enemy.ConcentrationSpeed;

        m_Timer = 0f;
        m_Enemy.Anim.SetTrigger("IsConcentration");
    }

    public override void ExitState()
    {
        Debug.Log("Concentration ����!");
    }

    public override void Action()
    {

    }

    public override void CheckState()
    {
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);

        // �÷��̾ �����ȿ� ��������
        if ((m_Enemy as Enemy_Slaughter).FOV.IsInFOV((m_Enemy as Enemy_Slaughter).ConcentrationDetectRange, (m_Enemy as Enemy_Slaughter).AlertDetectAngle, LayerMask.NameToLayer("PLAYER"))
            && (m_Enemy as Enemy_Slaughter).FOV.IsLookDirect(m_Enemy.PlayerTr, (m_Enemy as Enemy_Slaughter).PatrolPlayerDetectRange, LayerMask.NameToLayer("PLAYER")))
        {
            Debug.Log("�ű� �־�����!");

            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
            m_Timer = 0f;
            return;
        }

        // ���� �����ȿ� ��������
        if ((m_Enemy as Enemy_Slaughter).FOV.IsInFovWithRayCheckDirect((m_Enemy as Enemy_Slaughter).ConcentrationDetectRange, (m_Enemy as Enemy_Slaughter).AlertDetectAngle, 
            "LIGHT", (m_Enemy as Enemy_Slaughter).FOV.mLayerMask, ref m_LightPos, ref m_FlashTr))
        {
            Debug.Log("���� ���󰣴�..");
            (m_Enemy as Enemy_Slaughter).SetToTraceLight(m_FlashTr, m_LightPos);
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