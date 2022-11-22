using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<<< HEAD:First_Portfolio_ThemePark/Assets/02_Scripts/Enemy/State/Listener/Alert_Listener.cs
public class Alert_Listener : EnemyState
{
    public Alert_Listener(Enemy _enemy) : base("Alert", _enemy) { }
========
public class Concentration_Listener : EnemyState
{
    public Concentration_Listener(Enemy _enemy) : base("Alert", _enemy) { }
>>>>>>>> 34d6d22bc61a5f9b25fd283e99d3e323aa7749ca:First_Portfolio_ThemePark/Assets/02_Scripts/Enemy/State/Listener/Concentration_Listener.cs
 
    public override void EnterState()
    {
        Debug.Log("Alert ÀÔÀå!");
    }

    public override void ExitState()
    {
        Debug.Log("Alert ÅðÀå!");
    }

    public override void Action()
    {
        Debug.Log("Alert ¹°¸®¾÷µ«!");
    }

    public override void CheckState()
    {
        Debug.Log("Alert ¾÷µ«!");
    }
}
