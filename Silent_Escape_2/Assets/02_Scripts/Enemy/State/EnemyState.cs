using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected EnemyAI m_EnemyAI = null;

    protected virtual void Awake()
    {
        m_EnemyAI = GetComponent<EnemyAI>();
    }

    public abstract void EnterState(EnemyAI _enemyAI);
    public abstract void UpdateLogic(EnemyAI _enemyAI);
    public abstract void FixedUpdateLogic(EnemyAI _enemyAI);
    public abstract void ExitState(EnemyAI _enemyAI);
}
