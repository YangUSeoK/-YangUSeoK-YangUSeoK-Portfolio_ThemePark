using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SlowWalk : PlayerState
{
    public SlowWalk(Player _player) : base("SlowWalk", _player) { }

    public override void Action()
    {
        // ¸ØÃçÀÖÀ» ¶§ ¹ß¼Ò¸® Äð ¹«ÇÑ, ¾Æ´Ò¶§´Â SlowWalkStepInterval
        if(Mathf.Abs(m_Player.MoveAxis.action.ReadValue<Vector2>().y) <= Mathf.Epsilon)
        {
            m_Player.CurStepInterval = float.MaxValue;
        }
        else
        {
            m_Player.CurStepInterval = m_Player.SlowWalkStepInterval;
        }
    }

    public override void CheckState()
    {
        // °È±â
        if (m_Player.MoveAxis.action.ReadValue<Vector2>().y >= 0.95f)
        {
            m_Player.SetState(m_Player.Walk);
        }

        //¾É±â
        if (m_Player.GetComponent<CharacterController>().height <= m_Player.SitDownHeight)
        {
            m_Player.SetState(m_Player.Squat);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Slow Walk Enter");

        m_Player.CurSpeed = m_Player.SlowSpeed;
        m_Player.CurStepInterval = m_Player.SlowWalkStepInterval;
        //m_Player.CurStepIntervalWs = new WaitForSeconds(m_Player.CurStepInterval);
        m_Player.CurStepSoundRange = m_Player.SlowWalkSoundRange;
        m_Player.CurStepSoundLevel = m_Player.SlowWalkSoundLevel;
        m_Player.SetStepSound(0.3f, 0.5f);
    }

    public override void ExitState()
    {
        Debug.Log("Slow Walk Enter");
    }
}
