using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Listener : Enemy
{
    private Idle_Listener m_Idle;
    public Idle_Listener Idle
    {
        get { return m_Idle; }
    }
    private Trace_Listener m_TarcePlayer;
    public Trace_Listener TracePlayer
    {
        get { return m_TarcePlayer; }
    }
<<<<<<< HEAD
    private Concentration_Listener m_Concentration;
    public Concentration_Listener Alert
=======
<<<<<<< HEAD
    private Alert_Listener m_Alert;
    public Alert_Listener Alert
=======
    private Concentration_Listener m_Alert;
    public Concentration_Listener Alert
>>>>>>> 34d6d22bc61a5f9b25fd283e99d3e323aa7749ca
>>>>>>> 816907dcb86a33c36b3874d1c706bfa6a6bc7f3f
    {
        get { return m_Concentration; }
    }
    private Attack m_Attack;
    public Attack Attack
    {
        get { return m_Attack; }
    }


    protected override void Awake()
    {
        base.Awake();
        m_Idle = new Idle_Listener(this);
        m_TarcePlayer = new Trace_Listener(this);
<<<<<<< HEAD
        m_Concentration = new Concentration_Listener(this);
=======
<<<<<<< HEAD
        m_Alert = new Alert_Listener(this);
=======
        m_Alert = new Concentration_Listener(this);
>>>>>>> 34d6d22bc61a5f9b25fd283e99d3e323aa7749ca
>>>>>>> 816907dcb86a33c36b3874d1c706bfa6a6bc7f3f
        m_Attack = new Attack(this);
    }

    public override void SetPatrol()
    {

    }

    public override void SetTracePlayer()
    {
    }

<<<<<<< HEAD
    public override void SetConcentration()
=======
<<<<<<< HEAD
    public override void SetAlert()
=======
    public override void SetConcentration()
>>>>>>> 34d6d22bc61a5f9b25fd283e99d3e323aa7749ca
>>>>>>> 816907dcb86a33c36b3874d1c706bfa6a6bc7f3f
    {
    }

    public override void SetAttack()
    {
    }

    

    
}
