using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{
    #region Player_State
    private Walk m_Walk = null;
    public Walk Walk
    {
        get { return m_Walk; }
    }

    private Run m_Run = null;
    public Run Run
    {
        get { return m_Run; }
    }

    private SlowWalk m_SlowWalk = null;
    public SlowWalk SlowWalk
    {
        get { return m_SlowWalk; }
    }

    private Squat m_Squat = null;
    public Squat Squat
    {
        get { return m_Squat; }
    }
    #endregion
    #region Inspector
    [Space]
    [Header("Speed")]


    [SerializeField] private float m_WalkSpeed = 3f;
    public float WalkSpeed
    {
        get { return m_WalkSpeed; }
    }

    [SerializeField] private float m_RunSpeed = 5f;
    public float RunSpeed
    {
        get { return m_RunSpeed; }
    }

    [SerializeField] private float m_SlowSpeed = 2f;
    public float SlowSpeed
    {
        get { return m_SlowSpeed; }
    }

    [SerializeField] private float m_SquatSpeed = 1f;
    public float SquatSpeed
    {
        get { return m_SquatSpeed; }
    }
    public float CurSpeed
    {
        set { m_MoveProvider.moveSpeed = value; }
    }

    [Space]
    [Header("Sound Range")]

    [SerializeField] private float m_CurStepSoundRange = 5f;
    public float CurStepSoundRange
    {
        get { return m_CurStepSoundRange; }
        set { m_CurStepSoundRange = value; }
    }
    [SerializeField] private float m_WalkSoundRange = 6.5f;
    public float WalkSoundRange
    {
        get { return m_WalkSoundRange; }
    }
    [SerializeField] private float m_RunSoundRange = 10f;
    public float RunSoundRange
    {
        get { return m_RunSoundRange; }
    }
    [SerializeField] private float m_SlowWalkSoundRange = 4f;
    public float SlowWalkSoundRange
    {
        get { return m_SlowWalkSoundRange; }
    }
    [SerializeField] private float m_SquatSoundRange = 0f;
    public float SquatSoundRange
    {
        get { return m_SquatSoundRange; }
    }

    [Space]
    [Header("Sound Level")]
    [SerializeField] private float m_CurStepSoundLevel = 5f;
    public float CurStepSoundLevel
    {
        get { return m_CurStepSoundLevel; }
        set { m_CurStepSoundLevel = value; }
    }
    [SerializeField] private float m_WalkSoundLevel = 3f;
    public float WalkSoundLevel
    {
        get { return m_WalkSoundLevel; }
    }
    [SerializeField] private float m_RunSoundLevel = 5f;
    public float RunSoundLevel
    {
        get { return m_RunSoundLevel; }
    }
    [SerializeField] private float m_SlowWalkSoundLevel = 1f;
    public float SlowWalkSoundLevel
    {
        get { return m_SlowWalkSoundLevel; }
    }
    [SerializeField] private float m_SquatSoundLevel = 0f;
    public float SquatSoundLevel
    {
        get { return m_SquatSoundLevel; }
    }

    [Space]
    [Header("Step Interval")]
    [SerializeField] private float m_CurStepInterval = 0.6f;
    public float CurStepInterval
    {
        get { return m_CurStepInterval; }
        set { m_CurStepInterval = value; }
    }
    [SerializeField] private float m_WalkStepInterval = 0.6f;
    public float WalkStepInterval
    {
        get { return m_WalkStepInterval; }
    }
    [SerializeField] private float m_RunStepInterval = 0.3f;
    public float RunStepInterval
    {
        get { return m_RunStepInterval; }
    }
    [SerializeField] private float m_SlowWalkStepInterval = 1f;
    public float SlowWalkStepInterval
    {
        get { return m_SlowWalkStepInterval; }
    }
    [SerializeField] private float m_SquatStepInterval = float.MaxValue;
    public float SquatStepInterval
    {
        get { return m_SquatStepInterval; }
    }


    #endregion
    #region Member_variable
    [Space]
    [Header("Member_Variable")]
    [SerializeField] private InputActionProperty m_MoveAxis;
    public InputActionProperty MoveAxis
    {
        get { return m_MoveAxis; }
    }
    [SerializeField] private InputActionProperty m_RunButton;
    public InputActionProperty RunButton
    {
        get { return m_RunButton; }
    }
    // 20221203 양우석 : 델리게이트로 받아와야 함
    [SerializeField] private EnemyManager m_EnemyManager = null;


    private bool mb_IsGameOver = false;

    private AudioSource[] m_Audios = null;
    public AudioSource[] Audios
    {
        get { return m_Audios; }
    }

    [SerializeField] private AudioSource m_HeartBeat = null;

    private enum EAudioClipName
    {
        RightStepSound = 0,
        LeftStepSound,
        HeartBeat,

        Length
    }


    private PlayerState m_CurState = null;
    public PlayerState CurState
    {
        get { return m_CurState; }
    }

    private Inven m_Inven = null;
    public Inven Inven
    {
        get { return m_Inven; }
    }

    private ActionBasedContinuousMoveProvider m_MoveProvider = null;
    public ActionBasedContinuousMoveProvider MoveProvider
    {
        get { return m_MoveProvider; }
    }



    private WaitForSeconds m_CurStepIntervalWs;
    public WaitForSeconds CurStepIntervalWs
    {
        set { m_CurStepIntervalWs = value; }
    }

    private float m_SitDownHeight = 1.2f;
    public float SitDownHeight
    {
        get { return m_SitDownHeight; }
    }

    private float m_WakeUpHeight = 1.4f;
    public float WakeUpHeight
    {
        get { return m_WakeUpHeight; }
    }

    private int mStepIdx = 0;
    private float mTimer = 0f;


    #endregion

    private void Awake()
    {
        m_Audios = GetComponents<AudioSource>();
        m_Inven = GetComponent<Inven>();
        m_MoveProvider = GetComponent<ActionBasedContinuousMoveProvider>();

        m_Walk = new Walk(this);
        m_Run = new Run(this);
        m_SlowWalk = new SlowWalk(this);
        m_Squat = new Squat(this);
    }

    private void Start()
    {
        m_CurState = m_Walk;
        SetState(m_Walk);
    }

    private void Update()
    {
        if (!mb_IsGameOver)
        {
            m_CurState.CheckState();
            m_CurState.Action();

            // 20221205  발소리 코루틴 대신 Update에서 하는걸로 수정
            // if (m_CurState != m_Squat)
            {
                mTimer += Time.deltaTime;
                if (mTimer >= m_CurStepInterval)
                {
                    StepSound();
                    mTimer = 0f;
                }
            }
            SetReverb();
            SetHeartBeat();
        }
        else
        {
            StopAllSound();
        }
    }
    #region State
    public void SetState(PlayerState _state)
    {
        if (m_CurState != null)
        {
            m_CurState.ExitState();
        }
        m_CurState = _state;
        m_CurState.EnterState();
    }

    public void InputRun()
    {
        if (m_CurState != m_Squat)
        {
            SetState(m_Run);
        }
    }

    public void OutRun()
    {
        if (m_CurState == m_Run)
        {
            SetState(m_Walk);
        }
    }
    #endregion

    #region Sound
    public void StepSound()
    {
        Collider[] nearListener = Physics.OverlapSphere(transform.position, m_CurStepSoundRange, 1 << LayerMask.NameToLayer("LISTENER"));
        for (int i = 0; i < nearListener.Length; ++i)
        {
            nearListener[i].gameObject.GetComponent<Enemy_Listener>().Listen(transform, transform.position, m_CurStepSoundLevel);
        }

        // 스텝 소리 출력
        if (mStepIdx == 0)
        {
            Debug.Log("왼쪽");
            // 왼쪽소리
            m_Audios[mStepIdx].PlayOneShot(m_Audios[mStepIdx].clip);
            mStepIdx = 1;
        }
        else if (mStepIdx == 1)
        {
            Debug.Log("오른쪽");
            // 오른쪽 소리
            m_Audios[mStepIdx].PlayOneShot(m_Audios[mStepIdx].clip);
            mStepIdx = 0;
        }
    }

    public void SetStepSound(float _volume, float _pitch)
    {
        for (int i = 0; i < 2; ++i)
        {
            m_Audios[i].volume = _volume;
            m_Audios[i].pitch = _pitch;
        }
    }

    private void SetHeartBeat()
    {
        float distance = Vector3.Distance(m_EnemyManager.NearZombie.transform.position, transform.position);
        float beatSound = 0f;
        float pitch = 0f;

        //Debug.Log(distance);
        if (distance <= 8f)
        {
            beatSound = Mathf.Clamp(3f / distance, 0f, 1f);
        }
        
        pitch = Mathf.Clamp(0.7f + (2f / distance), 0.7f, 1.4f);
        m_HeartBeat.volume = beatSound;
        m_HeartBeat.pitch = pitch;
    }

    private void SetReverb()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hitInfo, 1f))
        {
            if (hitInfo.transform.CompareTag("REVERBZONE"))
            {
                GetComponent<AudioReverbFilter>().enabled = true;
            }
            else
            {
                GetComponent<AudioReverbFilter>().enabled = false;
            }
        }
    }

    private void StopAllSound()
    {
        // 발소리 정지
        for (int i = 0; i < m_Audios.Length; ++i)
        {
            m_Audios[i].Stop();
        }

        // 심장소리 정지
        m_HeartBeat.Stop();
    }
    #endregion
}
