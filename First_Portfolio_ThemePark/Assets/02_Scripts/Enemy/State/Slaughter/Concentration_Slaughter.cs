using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concentration_Slaughter : EnemyState
{
    public Concentration_Slaughter(Enemy _enemy) : base("Concentration", _enemy) { }

    private Vector3 m_LightPos = Vector3.zero;
    private Transform m_FlashTr = null;
    private float m_Timer = 0f;

    public override void EnterState()
    {
        Debug.Log("Alert ����!");
        Debug.Log("���� ������ ���̴ϴ�.");
        m_Enemy.SetConcentration();
        m_Timer = 0f;
    }

    public override void ExitState()
    {
        Debug.Log("Alert ����!");
    }

    public override void Action()
    {

    }

    public override void CheckState()
    {
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);

        // �÷��̾ �����ȿ� ��������
        if (m_FOV.IsInFOV(m_Enemy.AlertDetectRange, m_Enemy.AlertDetectAngle, LayerMask.NameToLayer("PLAYER"))
            && m_FOV.IsLookDirect(m_Enemy.PlayerTr, m_Enemy.PatrolPlayerDetectRange, LayerMask.NameToLayer("PLAYER")))
        {
            Debug.Log("�ű� �־�����!");

            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
            m_Timer = 0f;
            return;
        }

        // ���� �����ȿ� ��������
        if (m_FOV.IsInFovWithRayCheckDirect(m_Enemy.AlertDetectRange, m_Enemy.AlertDetectAngle, 
            "LIGHT", m_FOV.mLayerMask, ref m_LightPos, ref m_FlashTr))
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
        Debug.Log($"Alert : {m_Timer}");
    }
}