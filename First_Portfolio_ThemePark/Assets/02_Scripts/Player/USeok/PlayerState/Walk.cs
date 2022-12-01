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
        // Axis 0.95 ÀÌÇÏ => SetState(SlowWalk)
        InputManager.Instance.LeftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 secondary2DAxisValue);
        
        //Debug.Log(secondary2DAxisValue);
        
        if(secondary2DAxisValue.y < 0.95f)
        {
            m_Player.SetState(m_Player.SlowWalk);
        }

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
