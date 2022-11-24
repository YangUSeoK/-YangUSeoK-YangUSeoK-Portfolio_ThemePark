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
        Debug.Log("TraceLight ����!");

        if (m_FlashTr == null)
        {
            m_FlashTr = (m_Enemy as Enemy_Slaughter).FlashTr;
        }
        m_Enemy.Agent.speed = m_Enemy.PatrolSpeed;
        m_Enemy.Agent.destination = (m_Enemy as Enemy_Slaughter).LightPos;
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
        int layerMask = (1 << (m_Enemy as Enemy_Slaughter).FOV.FlashLayer) | (1 << (m_Enemy as Enemy_Slaughter).FOV.ObstacleLayer)
                        | (1 << (m_Enemy as Enemy_Slaughter).FOV.PlayerLayer) | ~(1 << (m_Enemy as Enemy_Slaughter).FOV.LightLayer);

        // Ž������ ���̶��
        if (Physics.Raycast(m_Enemy.transform.position, m_FlashTr.position - m_Enemy.transform.position,
            out hitInfo, /*(m_Enemy as Enemy_Slaughter).TraceDetectRange + 30f*/ 100f, layerMask))
        {
            // ���θ��°� ���� �÷��̾ �������� ����ִٸ� => TracePlayer
            // 20221116 ��켮:  �÷��̾�� �Ÿ� ������ ���纸�� �����ؾ� ��.
            if (hitInfo.collider.CompareTag("PLAYER") ||
                (hitInfo.collider.CompareTag("FLASH") && Vector3.Distance(m_Enemy.PlayerTr.position, m_FlashTr.position) <= 1.5f))
            {
                Debug.Log("���θ��°� ��� �Ų���!!");
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
                return;
            }
        }

        Debug.DrawLine(m_Enemy.transform.position, m_LightPos, Color.blue);

        // ���̰� �ȸ¾Ұ� ������ �� ��ġ�� ���� ��ġ�� ������  => Alert
        if (Vector3.Distance(new Vector3(m_LightPos.x, m_Enemy.transform.position.y, m_LightPos.z), m_Enemy.transform.position) <= 0.3f)
        {
            Debug.Log("�ƹ��͵� ����?");
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Concentration);
        }
    }



    public override void ExitState()
    {
        Debug.Log("TraceLight ����!");
    }
}
