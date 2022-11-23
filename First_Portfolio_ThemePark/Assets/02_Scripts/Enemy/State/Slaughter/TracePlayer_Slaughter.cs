using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class TracePlayer_Slaughter : EnemyState
{
    public TracePlayer_Slaughter(Enemy _enemy) : base("TracePlayer", _enemy) { }

    
    
    private Vector3 m_PlayerPos = Vector3.zero; // ���������� �� �÷��̾� ��ġ ���庯��
    private bool mbIsLookPlayer = false;
    private float m_Timer = 0f;

    public override void EnterState()
    {
        Debug.Log("TracePlayer ����!");
        Debug.Log("�ֺ� ���� �θ��ϴ�!");
        m_Enemy.Agent.speed = m_Enemy.TraceSpeed;
        m_Enemy.Agent.destination = m_Enemy.PlayerTr.position;

        m_Timer = 0f;
        m_Enemy.Anim.SetTrigger("IsTracePlayer");
        // �ֺ� ���� �θ��� �Լ�
        (m_Enemy as Enemy_Slaughter).CallNearZombie();
    }

    public override void ExitState()
    {
        Debug.Log("TracePlayer ����!");
    }

    public override void Action()
    {
        // �÷��̾�� ���̸� ���� ��ġ���� ������Ʈ ����
        RaycastHit hitInfo;       
        int layerMask = ((m_Enemy as Enemy_Slaughter).FOV.PlayerLayer | (m_Enemy as Enemy_Slaughter).FOV.ObstacleLayer);

        Debug.DrawLine(m_Enemy.transform.position, m_Enemy.PlayerTr.position, Color.blue);

        if (Physics.Raycast(m_Enemy.transform.position, m_Enemy.PlayerTr.position - m_Enemy.transform.position,
            out hitInfo, (m_Enemy as Enemy_Slaughter).TraceDetectRange+ 30f, layerMask))
        {
            Debug.Log(hitInfo.transform.name);
            if (hitInfo.collider.CompareTag("PLAYER"))
            {
                Debug.Log("�÷��̾� ����");
                mbIsLookPlayer = true;
                m_PlayerPos = hitInfo.transform.position;
            }
            else
            {
                mbIsLookPlayer = false;
            }
        }
        else
        {
            mbIsLookPlayer = false;
        }
        m_Enemy.Agent.destination = m_PlayerPos;
    }

    public override void CheckState()
    {
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);

        if (mbIsLookPlayer)
        {
            m_Timer = 0;
            if ((m_Enemy as Enemy_Slaughter).AttackRange >= dist)
            {
                Debug.Log("�׾�� �װ�!");
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Attack);
                return;
            }
        }
        else
        {
            m_Timer += Time.deltaTime;
            Debug.Log($"Trace Player : {m_Timer}");

            if (m_Timer >= 5f)
            {
                Debug.Log("�����?");
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Concentration);
            }
        }
    }
}
