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
        Debug.Log($"{m_Enemy.name} TracePlayer ����!");
        Debug.Log("�ֺ� ���� �θ��ϴ�!");
        m_Enemy.Agent.speed = m_Enemy.TraceSpeed;
        m_PlayerPos = m_Enemy.PlayerTr.position;
        m_Enemy.Agent.destination = m_PlayerPos;

        m_Timer = 0f;
        m_Enemy.Anim.SetBool("IsTracePlayer", true);

        // �ֺ� ���� �θ��� �Լ�
        (m_Enemy as Enemy_Slaughter).CallNearZombie();

        // BGM �ٲٱ�
        m_Enemy.Audio[3].Play();
        (m_Enemy as Enemy_Slaughter).EnterTracePlayerCallback();

        m_Enemy.Mr.material.color = Color.red;
    }

    public override void ExitState()
    {
        Debug.Log($"{m_Enemy.name} TracePlayer ����!");
        m_Enemy.Anim.SetBool("IsTracePlayer", false);

        // BGM �ٲٱ�
        m_Enemy.Audio[2].Play();
        (m_Enemy as Enemy_Slaughter).ExitTracePlayerCallback();
    }

    public override void Action()
    {
        // �÷��̾�� ���̸� ���� ��ġ���� ������Ʈ ����
        RaycastHit hitInfo;
        int layerMask = ((m_Enemy as Enemy_Slaughter).FOV.PlayerLayer | (m_Enemy as Enemy_Slaughter).FOV.ObstacleLayer);

        Debug.DrawLine(m_Enemy.transform.position, m_Enemy.PlayerTr.position, Color.blue);

        if (Physics.Raycast(m_Enemy.transform.position + (Vector3.up * 0.7f), m_Enemy.PlayerTr.position - m_Enemy.transform.position,
            out hitInfo, (m_Enemy as Enemy_Slaughter).TraceDetectRange + 30f, layerMask) 
            && hitInfo.collider.CompareTag("PLAYER"))
        {
            Debug.Log($" TracePlayer ���������� ������ : {m_Enemy.transform.name} / {hitInfo.transform.name}");

            mbIsLookPlayer = true;
            m_PlayerPos = hitInfo.transform.position;
        }
        else if (Physics.Raycast(m_Enemy.transform.position + (Vector3.up * 1.4f), m_Enemy.PlayerTr.position - m_Enemy.transform.position,
                out hitInfo, (m_Enemy as Enemy_Slaughter).TraceDetectRange + 30f, layerMask) 
                && hitInfo.collider.CompareTag("PLAYER"))
        {
            Debug.Log($" TracePlayer �������� ������ : {m_Enemy.transform.name} / {hitInfo.transform.name}");
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
                Debug.Log("�����?");
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Concentration);
            }
        }
    }
}
