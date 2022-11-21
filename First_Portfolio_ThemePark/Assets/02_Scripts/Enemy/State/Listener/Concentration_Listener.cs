using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concentration_Listener : EnemyState
{
    public Concentration_Listener(Enemy _enemy) : base("Alert", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log("Alert ����!");
    }

    public override void ExitState()
    {
        Debug.Log("Alert ����!");
    }

    public override void Action()
    {
        Debug.Log("Alert ��������!");
    }

    public override void CheckState()
    {
        Debug.Log("Alert ����!");
    }
}
