using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : PlayerState
{
    public Run(Player _player) : base("Run", _player) { }

    public override void Action()
    {
    }

    public override void CheckState()
    {
        if (m_Player.RunButton.action.WasPressedThisFrame())
        {
            m_Player.SetState(m_Player.Walk);
        }

        // ¾É±â => SetState(Squat)
        if (m_Player.GetComponent<CharacterController>().height <= m_Player.SitDownHeight)
        {
            m_Player.SetState(m_Player.Squat);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Run Enter");
        m_Player.CurSpeed = m_Player.RunSpeed;
        m_Player.CurStepInterval = m_Player.RunStepInterval;
        m_Player.CurStepIntervalWs = new WaitForSeconds(m_Player.CurStepInterval);
        m_Player.CurStepSoundRange = m_Player.RunSoundRange;
        m_Player.CurStepSoundLevel = m_Player.RunSoundLevel;

        m_Player.StepSound(); new System.NotImplementedException();
    }

    public override void ExitState()
    {
        Debug.Log("Run Out");
    }
}
