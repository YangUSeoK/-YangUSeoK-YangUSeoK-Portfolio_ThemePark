using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LookAround_Listener : EnemyState
{
    public LookAround_Listener(Enemy _enemy) : base("LookAround", _enemy) { }

    private float timer = 0f;
    public override void EnterState()
    {
        Debug.Log($"{m_Enemy.name} LookAround ÀÔÀå!");
        m_Enemy.Anim.SetBool("IsLookAround", true);
        timer = 0f;


        m_Enemy.Mr.material.color = Color.blue;
    }

    public override void ExitState()
    {
        Debug.Log($"{m_Enemy.name} LookAround ÅğÀå!");
        m_Enemy.Anim.SetBool("IsLookAround", false);
    }

    public override void Action()
    {
    }

    public override void CheckState()
    {
        timer += Time.deltaTime;
        if (timer >= 3f)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Listener).Idle);
        }
    }
}
