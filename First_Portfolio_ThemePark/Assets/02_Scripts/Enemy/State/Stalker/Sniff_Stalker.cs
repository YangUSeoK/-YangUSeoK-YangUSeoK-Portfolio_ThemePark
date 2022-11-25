using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniff_Stalker : EnemyState
{
    public Sniff_Stalker(Enemy _enemy) : base("Sniff", _enemy) { }

    public override void Action()
    {
        
    }

    public override void CheckState()
    {
        float timer = 0f;
        timer += Time.deltaTime;
        if(timer >= (m_Enemy as Enemy_Stalker).SniffTime)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Stalker).Trace);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Dog : Å¯Å¯..³¿»õ ¸Ã´Â Áß");
        m_Enemy.Anim.SetTrigger("IsSniff");
        
        // Å¯Å¯ ¿Àµð¿À Ãâ·Â
    }

    public override void ExitState()
    {
        Debug.Log("Dog : À§Ä¡ È®ÀÎ");
    }
}
