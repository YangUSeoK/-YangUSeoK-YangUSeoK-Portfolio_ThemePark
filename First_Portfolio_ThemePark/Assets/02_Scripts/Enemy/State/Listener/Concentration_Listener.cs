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
        Debug.Log("Concentration ÀÔÀå!");
        m_Enemy.Audio[1].Stop();
        m_Enemy.Anim.SetTrigger("IsTraceSound");
        m_Enemy.Audio[0].Play();
        timer = 0f;
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
        
        timer += Time.deltaTime;

        if (timer > m_Enemy.Audio[0].clip.length)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Listener).TraceTarget);
        }
    }
}
