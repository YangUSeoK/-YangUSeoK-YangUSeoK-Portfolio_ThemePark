using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Concentration_Listener : EnemyState
{
    public Concentration_Listener(Enemy _enemy) : base("Concentration", _enemy) { }

    private float timer = 0f;
    public override void EnterState()
    {
        Debug.Log($"{m_Enemy.name} Concentration ÀÔÀå!");
        m_Enemy.Anim.SetBool("IsConcentration",true);
        m_Enemy.Audio[1].Play();
        timer = 0f;


        m_Enemy.Mr.material.color = Color.green;

    }

    public override void ExitState()
    {
        Debug.Log($"{m_Enemy.name} Concentration ÅğÀå!");
        m_Enemy.Anim.SetBool("IsConcentration", false);
    }

    public override void Action()
    {
    }

    public override void CheckState()
    {
        timer += Time.deltaTime;

        if (timer >= 2.3f)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Listener).TraceTarget);
        }
    }
}
