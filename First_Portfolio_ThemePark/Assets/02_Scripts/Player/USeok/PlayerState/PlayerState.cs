using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    public PlayerState(string _name, Player _player) 
    {
        m_Name = _name;
        m_Player = _player;
    }

    protected string m_Name;
    public string Name
    {
        get { return m_Name; }
    }

    protected Player m_Player;

    public abstract void EnterState();
    public abstract void CheckState();
    public abstract void Action();
    public abstract void ExitState();

}
