using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concentration_Listener : EnemyState
{
    public Concentration_Listener(Enemy _enemy) : base("Concentration", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log("Concentration ÀÔÀå!");
    }

    public override void ExitState()
    {
        Debug.Log("Concentration ÅðÀå!");
    }

    public override void Action()
    {
        Debug.Log("Concentration ¾×¼Ç!");
    }

    public override void CheckState()
    {
        Debug.Log("Concentration ¾÷µ«!");
    }
}
