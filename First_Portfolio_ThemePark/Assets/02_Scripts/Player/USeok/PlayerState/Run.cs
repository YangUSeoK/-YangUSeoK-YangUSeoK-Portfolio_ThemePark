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
        // ¾É±â => SetState(Squat)
        if (m_Player.GetComponent<CharacterController>().height <= m_Player.SitDownHeight)
        {
            Debug.Log(m_Player.GetComponent<CharacterController>().height);
            m_Player.SetState(m_Player.Squat);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Walk Enter");
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
