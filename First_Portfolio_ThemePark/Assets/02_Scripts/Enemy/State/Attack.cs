using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : EnemyState
{
    public Attack(Enemy _enemy) : base("Attack", _enemy) { }

    public override void EnterState()
    {
        Debug.Log("Attack ����!");
        m_Enemy.Anim.SetTrigger("IsAttack");
        // ���� ��������Ʈ ȣ��
        m_Enemy.IsAttack();
        m_Enemy.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.blue;
    }

    public override void ExitState()
    {
        Debug.Log("Attack ����!");
    }

    public override void Action()
    {
    }

    public override void CheckState()
    {
    }
}
