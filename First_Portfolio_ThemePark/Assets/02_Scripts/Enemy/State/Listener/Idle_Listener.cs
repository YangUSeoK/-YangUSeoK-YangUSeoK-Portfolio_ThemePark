using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle_Listener : EnemyState
{
    public Idle_Listener(Enemy _enemy) : base("Idle", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log("Idle ÀÔÀå!");
    }

    public override void ExitState()
    {
        Debug.Log("Idle ÅğÀå!");
    }

    public override void Action()
    {
        Debug.Log("Idle ¾×¼Ç!");
    }

    public override void CheckState()
    {
        Debug.Log("Idle ¾÷µ«!");
    }
}
