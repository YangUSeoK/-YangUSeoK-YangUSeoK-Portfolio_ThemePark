using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    // 20221104 양우석 Range범위 생각해보고 추가할 것
    public float NomalSpeed
    {
        get
        {
            return m_NomalSpeed;
        }
    }
    [SerializeField] protected float m_NomalSpeed;

    public float SoundDetectRange
    {
        get
        {
            return m_SoundDetectRange;
        }
    }
    [SerializeField] protected float m_SoundDetectRange;

    public float LookPlayerRange
    {
        get
        {
            return m_LookPlayerRange;
        }
    }
    [SerializeField] protected float m_LookPlayerRange;

    public float AttackRange
    {
        get
        {
            return m_AttackRange;
        }
    }
    [SerializeField] protected float m_AttackRange;

    public float CurSpeed
    {
        get
        {
            return mCurSpeed;
        }
    }
    protected float mCurSpeed;



   

    public void SetSpeed(float _curSpeed)
    {
        mCurSpeed = _curSpeed;
    }

    public void Attack()
    {
        // 게임오버    20221104 아직 미구현
        Debug.Log("GameOver");
    }

    protected abstract void Action();
}
