using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert_Stalker : EnemyState
{
    public Alert_Stalker(Enemy _enemy) : base("Alert", _enemy) { }

    public override void Action()
    {
        // 소리를 10초동안 짖는다.
    }

    public override void CheckState()
    {
        // 10초 동안 짖고 Set Sniff로
        float timer = 0f;
        timer += Time.deltaTime;
        if(timer >= (m_Enemy as Enemy_Stalker).AlertTime)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Stalker).Sniff);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Dog : 인간이다 멍멍!");
        m_Enemy.Agent.destination = m_Enemy.transform.position;

        m_Enemy.Anim.SetTrigger("IsAlert");
        Howling();
        // 소리 짖기 설정
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    private void Howling()
    {
        Collider[] listeners = new Collider[10];
        int layer = 1 << LayerMask.NameToLayer("LISTENER");
        Physics.OverlapSphereNonAlloc(m_Enemy.transform.position, (m_Enemy as Enemy_Stalker).AlertRange, listeners, layer);

        for(int i = 0; i < listeners.Length; ++i)
        {
            if(listeners[i] != null)
            {
                listeners[i].GetComponent<Enemy_Listener>().Listen(m_Enemy.PlayerTr, m_Enemy.transform.position,
                                                                    (m_Enemy as Enemy_Stalker).AlertRange);
            }
        }
    }
}
