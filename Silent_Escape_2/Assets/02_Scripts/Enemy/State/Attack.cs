using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : EnemyState
{
    public override void EnterState(EnemyAI _enemyAI)
    {
        Debug.Log("Attack ����!");
    }

    public override void ExitState(EnemyAI _enemyAI)
    {
        Debug.Log("Attack ����!");
    }

    public override void FixedUpdateLogic(EnemyAI _enemyAI)
    {
        Debug.Log("Attack ��������!");
    }

    public override void UpdateLogic(EnemyAI _enemyAI)
    {
        Debug.Log("Attack ����!");
    }
}
