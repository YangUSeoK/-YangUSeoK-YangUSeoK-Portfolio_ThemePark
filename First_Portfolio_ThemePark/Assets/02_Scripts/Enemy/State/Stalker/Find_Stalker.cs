using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Find_Stalker : EnemyState
{
    public Find_Stalker(Enemy _enemy) : base("Find", _enemy) { }

    public override void Action()
    {
        // �� ������ �÷��̾��� ��ġ�� ������ �Ѿư���.
        m_Enemy.Agent.destination = m_Enemy.PlayerTr.position;

        // �߼Ҹ� ���
        // �׷ο︵ �Ҹ� ���
    }

    public override void CheckState()
    {
        RaycastHit hitInfo;
        int layerMask = 1 << LayerMask.NameToLayer("PLAYER") | 1 << LayerMask.NameToLayer("OBSTACLE");

        // �÷��̾ ���̸� �ν��� ���� ���������� SetState(Alert);
        Physics.Raycast(m_Enemy.transform.position, m_Enemy.PlayerTr.position - m_Enemy.transform.position, out hitInfo,
                        (m_Enemy as Enemy_Stalker).FindRange, layerMask);
        if (hitInfo.transform.CompareTag("PLAYER"))
        {
            m_Enemy.SetState((m_Enemy as Enemy_Stalker).Alert);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Dog : ������ ����!");
        m_Enemy.Agent.speed = (m_Enemy as Enemy_Stalker).FindSpeed;
        m_Enemy.Anim.SetBool("IsFind", true);

        // �޸��� �Ҹ� ����
        // �׷ο︵ �Ҹ� ����(�Ҹ� ������ ���)
    }

    public override void ExitState()
    {
        Debug.Log("Find Exit");
        m_Enemy.Anim.SetBool("IsFind", false);
    }
}
