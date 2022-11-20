using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle_Listener : EnemyState
{
    public Idle_Listener(Enemy _enemy) : base("Idle", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log("Idle ����!");
    }

    public override void ExitState()
    {
        Debug.Log("Idle ����!");
    }

    public override void Action()
    {
        Debug.Log("Idle �׼�!");
    }

    public override void CheckState()
    {
        Debug.Log("Idle ����!");
    }
}
