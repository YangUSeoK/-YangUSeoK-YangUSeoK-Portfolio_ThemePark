using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Walk : PlayerState
{
    public Walk(Player _player) : base("Walk", _player) { }

    public override void Action()
    {
    }

    public override void CheckState()
    {
        // Axis 0.95 이하 => SetState(SlowWalk)
        Debug.Log(m_Player.MoveAxis.action.ReadValue<Vector2>());

        if(m_Player.MoveAxis.action.ReadValue<Vector2>().y < 0.6f)
        {
            m_Player.SetState(m_Player.SlowWalk);
        }

        // Run 버튼 누르면 SetState(Run)
        if (m_Player.RunButton.action.WasPressedThisFrame())
        {
            m_Player.SetState(m_Player.Run);
        }

        // 앉기 => SetState(Squat)
        if (m_Player.GetComponent<CharacterController>().height <= m_Player.SitDownHeight)
        {
            //m_Player.SetState(m_Player.Squat);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Walk Enter");
        m_Player.CurSpeed = m_Player.WalkSpeed;
        m_Player.CurStepInterval = m_Player.WalkStepInterval;
        m_Player.CurStepIntervalWs = new WaitForSeconds(m_Player.CurStepInterval);
        m_Player.CurStepSoundRange = m_Player.WalkSoundRange;
        m_Player.CurStepSoundLevel = m_Player.WalkSoundLevel;

        m_Player.StepSound();
    }

    public override void ExitState()
    {
        Debug.Log("Walk Exit");
        m_Player.StopSound();
    }
}
