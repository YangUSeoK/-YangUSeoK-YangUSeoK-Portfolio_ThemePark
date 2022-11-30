using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SlowWalk : PlayerState
{
    public SlowWalk(Player _player) : base("SlowWalk", _player) { }

    public override void Action()
    {
        Debug.Log("Slow Walk Action");
    }

    public override void CheckState()
    {
        Debug.Log("Slow Walk CheckState");
        
        // °È±â
        InputManager.Instance.LeftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 secondary2DAxisValue);
        Debug.Log(secondary2DAxisValue);
        if (secondary2DAxisValue.y >= 0.95f)
        {
            m_Player.SetState(m_Player.Walk);
        }

        //¾É±â
        if (m_Player.GetComponent<CharacterController>().height <= m_Player.SitDownHeight)
        {
            Debug.Log(m_Player.GetComponent<CharacterController>().height);
            m_Player.SetState(m_Player.Squat);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Slow Walk Enter");

        m_Player.CurSpeed = m_Player.SlowSpeed;
        m_Player.CurStepInterval = m_Player.SlowWalkStepInterval;
        m_Player.CurStepIntervalWs = new WaitForSeconds(m_Player.CurStepInterval);
        m_Player.CurStepSoundRange = m_Player.SlowWalkSoundRange;
        m_Player.CurStepSoundLevel = m_Player.SlowWalkSoundLevel;

        m_Player.StepSound();
    }

    public override void ExitState()
    {
        Debug.Log("Slow Walk Enter");
        m_Player.StopSound();
    }
}
