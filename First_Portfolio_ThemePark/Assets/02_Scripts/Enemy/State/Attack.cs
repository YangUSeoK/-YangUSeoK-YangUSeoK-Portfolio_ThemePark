using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : EnemyState
{
    public Attack(Enemy _enemy) : base("Attack", _enemy) { }

    public override void EnterState()
    {
        Debug.Log("Attack 입장!");
        m_Enemy.Anim.SetTrigger("IsAttack");
        // 공격 델리게이트 호출
        m_Enemy.IsAttack();
    }

    public override void ExitState()
    {
        Debug.Log("Attack 퇴장!");
    }

    public override void Action()
    {
    }

    public override void CheckState()
    {
    }
}
