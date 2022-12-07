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
        Debug.Log($"{m_Enemy.name} TraceLight ����!");

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
        // ���� ���̴��� üũ -> ���� ���̸� �� ��ġ�� ����
        // ������ ��ġ�� �̵�
        (m_Enemy as Enemy_Slaughter).FOV.IsInFovWithRayCheckDirect((m_Enemy as Enemy_Slaughter).TraceDetectRange, (m_Enemy as Enemy_Slaughter).TraceDetectAngle,
                                        "LIGHT", (m_Enemy as Enemy_Slaughter).FOV.mLayerMask, ref m_LightPos, ref m_FlashTr);
        m_Enemy.Agent.destination = new Vector3(m_LightPos.x, m_Enemy.transform.position.y, m_LightPos.z);
    }

    public override void CheckState()
    {
        // ������ ��ġ�� ��� ���̸� ���.
        RaycastHit hitInfo;
        int layerMask = ((m_Enemy as Enemy_Slaughter).FOV.ObstacleLayer) | ((m_Enemy as Enemy_Slaughter).FOV.PlayerLayer);
        //| ~(1 << (m_Enemy as Enemy_Slaughter).FOV.LightLayer);
        

        // �÷��̾ ���� ���ٸ�
        if (Physics.Raycast(m_Enemy.transform.position + (Vector3.up) * 1.4f, m_Enemy.PlayerTr.position - m_Enemy.transform.position,
            out hitInfo, 100f, layerMask))
        {
            Debug.Log(hitInfo.transform.name);
            // ���θ��°� ���� �÷��̾ �������� ����ִٸ� => TracePlayer
            // 20221116 ��켮:  �÷��̾�� �Ÿ� ������ ���纸�� �����ؾ� ��.
            // 20221128 ��켮 : ���� �� ���� �ٸ���ġ�� �÷��̾ �ٷ� �Ѿƿ��°� �����ؾ� ��
            if (hitInfo.collider.CompareTag("PLAYER"))
            {
                Debug.Log("���θ��°� ��� �Ų���!!");
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
                return;
            }
        }
        else if (Physics.Raycast(m_Enemy.transform.position + (Vector3.up) * 0.7f, m_Enemy.PlayerTr.position - m_Enemy.transform.position,
                out hitInfo, 100f, layerMask))
        {
            Debug.Log($"TraceLight ���������� : {hitInfo.transform.name}");
            if (hitInfo.collider.CompareTag("PLAYER"))
            {
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
                return;
            }
        }

        Debug.DrawLine(m_Enemy.transform.position, m_LightPos, Color.blue);

        // ���̰� �ȸ¾Ұ� ������ �� ��ġ�� ���� ��ġ�� ������  => Concentration
        if (Vector3.Distance(new Vector3(m_LightPos.x, m_Enemy.transform.position.y, m_LightPos.z), m_Enemy.transform.position) <= 0.3f)
        {
            Debug.Log("�ƹ��͵� ����?");
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Concentration);
        }
    }



    public override void ExitState()
    {
        Debug.Log($"{m_Enemy.name} TraceLight ����!");
    }
}
