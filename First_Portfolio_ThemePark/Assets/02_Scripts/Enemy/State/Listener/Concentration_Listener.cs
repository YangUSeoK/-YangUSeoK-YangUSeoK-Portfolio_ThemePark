using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class Concentration_Listener : EnemyState
{
    public Concentration_Listener(Enemy _enemy) : base("Concentration", _enemy) { }
=======
<<<<<<<< HEAD:First_Portfolio_ThemePark/Assets/02_Scripts/Enemy/State/Listener/Alert_Listener.cs
public class Alert_Listener : EnemyState
{
    public Alert_Listener(Enemy _enemy) : base("Alert", _enemy) { }
========
public class Concentration_Listener : EnemyState
{
    public Concentration_Listener(Enemy _enemy) : base("Alert", _enemy) { }
>>>>>>>> 34d6d22bc61a5f9b25fd283e99d3e323aa7749ca:First_Portfolio_ThemePark/Assets/02_Scripts/Enemy/State/Listener/Concentration_Listener.cs
>>>>>>> 816907dcb86a33c36b3874d1c706bfa6a6bc7f3f
 
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
