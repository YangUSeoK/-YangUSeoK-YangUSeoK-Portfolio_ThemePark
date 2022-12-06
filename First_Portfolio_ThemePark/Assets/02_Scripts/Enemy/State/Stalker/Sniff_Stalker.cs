using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniff_Stalker : EnemyState
{
    public Sniff_Stalker(Enemy _enemy) : base("Sniff", _enemy) { }

    float timer = 0f;
    public override void Action()
    {

    }

    public override void CheckState()
    {
        
        timer += Time.deltaTime;
        if(timer >= (m_Enemy as Enemy_Stalker).SniffTime)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Stalker).Trace);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Dog : Å¯Å¯..³¿»õ ¸Ã´Â Áß");
        m_Enemy.Anim.SetBool("IsSniff", true);


        // Å¯Å¯ ¿Àµð¿À Ãâ·Â
    }

    public override void ExitState()
    {
        Debug.Log("Dog : À§Ä¡ È®ÀÎ");
        m_Enemy.Anim.SetBool("IsSniff", false);
    }
}
