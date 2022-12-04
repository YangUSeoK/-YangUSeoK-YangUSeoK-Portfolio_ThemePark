using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

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
    [SerializeField] private float m_WalkSoundRange = 3f;
    public float WalkSoundRange
    {
        get { return m_WalkSoundRange; }
    }
    [SerializeField] private float m_RunSoundRange = 5f;
    public float RunSoundRange
    {
        get { return m_RunSoundRange; }
    }
    [SerializeField] private float m_SlowWalkSoundRange = 1f;
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
    [SerializeField] private float m_CurStepInterval = 5f;
    public float CurStepInterval
    {
        get { return m_CurStepInterval; }
        set { m_CurStepInterval = value; }
    }
    [SerializeField] private float m_WalkStepInterval = 3f;
    public float WalkStepInterval
    {
        get { return m_WalkStepInterval; }
    }
    [SerializeField] private float m_RunStepInterval = 5f;
    public float RunStepInterval
    {
        get { return m_RunStepInterval; }
    }
    [SerializeField] private float m_SlowWalkStepInterval = 1f;
    public float SlowWalkStepInterval
    {
        get { return m_SlowWalkStepInterval; }
    }
    [SerializeField] private float m_SquatStepInterval = 0f;
    public float SquatStepInterval
    {
        get { return m_SquatStepInterval; }
    }


    #endregion

    #region Member_variable
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

    private AudioSource m_Audio = null;
    public AudioSource Audio
    {
        get { return m_Audio; }
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

    private float m_SitDownHeight = 0.8f;
    public float SitDownHeight
    {
        get { return m_SitDownHeight; }
    }

    private float m_WakeUpHeight = 1.2f;
    public float WakeUpHeight
    {
        get { return m_WakeUpHeight; }
    }

    
    #endregion

    private void Awake()
    {
        m_Audio = GetComponent<AudioSource>();
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
        }

    }

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
        if(m_CurState != m_Squat)
        {
            SetState(m_Run);
        }
    }

    public void OutRun()
    {
        if(m_CurState == m_Run)
        {
            SetState(m_Walk);
        }

    }

    public void StepSound()
    {
        StartCoroutine(StepSoundCoroutine());
    }

    public void StopSound()
    {
        StopCoroutine(StepSoundCoroutine());
    }

    private IEnumerator StepSoundCoroutine()
    {
        yield return m_CurStepIntervalWs;
        // 매 상황 입장마다 만들어주기
        Collider[] colls = Physics.OverlapSphere(transform.position, m_CurStepSoundRange, 1 << LayerMask.NameToLayer("LISTENER"));
        for (int i = 0; i < colls.Length; ++i)
        {
            colls[i].gameObject.GetComponent<Enemy_Listener>().Listen(transform, transform.position, m_CurStepSoundLevel);
        }

        // 스텝 소리 출력
    }

    private void HeartBeat()
    {
        float beatSound = 1f / Vector3.Distance(m_EnemyManager.NearZombie.transform.position, transform.position);
    }
}
