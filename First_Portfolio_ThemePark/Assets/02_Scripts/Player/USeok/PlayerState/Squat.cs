using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squat : PlayerState
{
    public Squat(Player _player) : base("Squat", _player) { }

    public override void Action()
    {
    }

    public override void CheckState()
    {
        //Debug.Log(m_Player.GetComponent<CharacterController>().height);
        // ¼­±â
        if (m_Player.GetComponent<CharacterController>().height >= m_Player.WakeUpHeight)
        {
            m_Player.SetState(m_Player.Walk);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Squat Enter");

        m_Player.CurSpeed = m_Player.SquatSpeed;
        m_Player.CurStepInterval = m_Player.SquatStepInterval;
        //m_Player.CurStepIntervalWs = new WaitForSeconds(m_Player.CurStepInterval);
        m_Player.CurStepSoundRange = m_Player.SquatSoundRange;
        m_Player.CurStepSoundLevel = m_Player.SquatSoundLevel;

    }

    public override void ExitState()
    {
        Debug.Log("Squat Enter");
    }
}
