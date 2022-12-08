using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaughterFactory : MonoBehaviour
{
    public delegate void VoidSlaughterListDelegate(List<Enemy_Slaughter> _slaughterList);
    public delegate void VoidVoidDelegate();
    private VoidSlaughterListDelegate initDelegate = null;
    private VoidVoidDelegate checkAllZombieEnterPatrolDelegate = null;
    private VoidVoidDelegate enterTracePlayerDelegate = null;
    private VoidVoidDelegate checkAllZombieExitTracePlayerDelegate = null;

    // 게임매니저가 플레이어 먹여줘야함
    private List<Enemy_Slaughter> mSlaughterList = null;
    public List<Enemy_Slaughter> SlaughterList
    {
        get { return mSlaughterList; }
    }
    [SerializeField] private Flag[] m_Flags = null;
    [SerializeField] private SpawnPoint[] m_SpawnPoints = null;
    private bool mIsActive = false;

    private Transform m_PlayerTr = null;   // 디버그
    public Transform PlayerTr
    {
        set { m_PlayerTr = value; }
    }
    [SerializeField] private Enemy_Slaughter m_Enemy = null;    // 소환할 프리팹
    [SerializeField] private float m_SpawnRange = 30f;
    [SerializeField] private float m_UnspawnRange = 60f;

    private bool mb_IsUnlocking = false; // 디버그
    public bool IsUnlcking
    {
        set { mb_IsUnlocking = value; }
    }

    private void Awake()
    {
        mSlaughterList = new List<Enemy_Slaughter>();
        m_Flags = GetComponentsInChildren<Flag>();
        m_SpawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

    private void Start()
    {
        for(int i = 0; i < m_SpawnPoints.Length; ++i)
        {
            InstantiateZombie(m_SpawnPoints[i].transform.position);
        }
        initDelegate?.Invoke(mSlaughterList);
    }


    private void Update()
    {
        if (!mb_IsUnlocking)
        {
            // 현재 스폰이 안 되있는 상태고 플레이어가 일정 거리안으로 들어오면 SetActive(true)
            if (!mIsActive && Vector3.Distance(m_PlayerTr.position, transform.position) <= m_SpawnRange)
            {
                SetActiveZombies();
            }

            // 현재 스폰 되있는 상태고 플레이어가 일정 거리 이상으로 멀어지면 SetActive(false)
            if (mIsActive && Vector3.Distance(m_PlayerTr.position, transform.position) >= m_UnspawnRange)
            {
                SetUnActiveZombies();
            }
        }
    }

    public void SetActiveZombies()
    {
        for (int i = 0; i < mSlaughterList.Count; ++i)
        {
            mSlaughterList[i].gameObject.SetActive(true);
            mIsActive = true;
        }
    }

    public void SetUnActiveZombies()
    {
        for (int i = 0; i < mSlaughterList.Count; ++i)
        {
            mSlaughterList[i].SetState(mSlaughterList[i].Patrol);
            mSlaughterList[i].gameObject.SetActive(false);
            mIsActive = false;
        }
    }

    private void InstantiateZombie(Vector3 _spawnPoint)
    {
        Enemy_Slaughter enemy = Instantiate(m_Enemy, _spawnPoint, Quaternion.identity);
        mSlaughterList.Add(enemy);

        enemy.transform.SetParent(transform);
        enemy.PlayerTr = m_PlayerTr;
        enemy.Flags = m_Flags;
        enemy.SetDelegate(AllZombieEnterPatrol, SetTracePlayerToNearZombie, EnterTracePlayerCallback, AllZombieExitTracePlayer);
        enemy.gameObject.SetActive(false);
    }


    private void SetTracePlayerToNearZombie(Enemy_Slaughter _caller)
    {
        StartCoroutine(SetTracePlayerToNearZombieCoroutine(_caller));
    }

    // 가까운 좀비 찾아서 SetState(TracePlayer)
    private IEnumerator SetTracePlayerToNearZombieCoroutine(Enemy_Slaughter _caller)
    {
        List<Enemy_Slaughter> calledZombiesList = new List<Enemy_Slaughter>();
        for (int i = 0; i < mSlaughterList.Count; ++i)
        {
            // 부른놈이 아니고 추격중이 아닌놈을 리스트에 추가한다.
            if (mSlaughterList[i] != _caller && mSlaughterList[i].CurState != mSlaughterList[i].TracePlayer)
            {
                calledZombiesList.Add(mSlaughterList[i]);
            }
        }

        // 부를 좀비가 0마리가 아니면
        if (calledZombiesList.Count != 0)
        {
            // 좀비 리스트 중 제일 가까운 좀비를 구한다.
            Enemy_Slaughter nearZombie = calledZombiesList[0];
            for (int i = 0; i < calledZombiesList.Count - 1; ++i)
            {
                if ((_caller.transform.position - calledZombiesList[i].transform.position).sqrMagnitude
                    >= (_caller.transform.position - calledZombiesList[i + 1].transform.position).sqrMagnitude)
                {
                    nearZombie = calledZombiesList[i + 1];
                }
            }
            // 제일 가까운 좀비의 상태를 TracePlayer로 수정.
            nearZombie.SetState(nearZombie.TracePlayer);
        }
        yield return null;
    }

    #region Delegate_Callback

    // 모든 좀비가 패트롤이라면
    private void AllZombieEnterPatrol()
    {
        Debug.Log("좀비 Enter Patrol");
        int cnt = 0;
        for (int i = 0; i < mSlaughterList.Count; ++i)
        {
            if (mSlaughterList[i].CurState != mSlaughterList[i].Patrol)
            {
                ++cnt;
            }

        }

        // 패트롤브금 
        if (cnt == 0)
        {
            Debug.Log("모든좀비 Patrol");
            checkAllZombieEnterPatrolDelegate?.Invoke();
        }
    }

    private void EnterTracePlayerCallback()
    {
        enterTracePlayerDelegate?.Invoke();
    }

    private void AllZombieExitTracePlayer()
    {
        Debug.Log("좀비 Exit TracePlayer");
        int cnt = 0;

        for (int i = 0; i < mSlaughterList.Count; ++i)
        {
            if (mSlaughterList[i].CurState == mSlaughterList[i].TracePlayer)
            {
                ++cnt;
                Debug.Log("cnt up");
            }
        }

        Debug.Log($"Trace 좀비 수 : {cnt}");
        if (cnt == 0)
        {
            Debug.Log("모든좀비 Exit TracePlayer");
            // 전투브금 볼륨을 FadeOut
            checkAllZombieExitTracePlayerDelegate?.Invoke();
        }
    }

    public void SetDelegate(VoidSlaughterListDelegate _initCallback,  VoidVoidDelegate _checkAllZombieEnterPatrolCallback,
                            VoidVoidDelegate _enterTracePlayerCallback, VoidVoidDelegate _checkAllZombieExitTracePlayerCallback)
    {
        Debug.Log("팩토리 셋델리");
        initDelegate = _initCallback;
        checkAllZombieEnterPatrolDelegate = _checkAllZombieEnterPatrolCallback;
        enterTracePlayerDelegate = _enterTracePlayerCallback;
        checkAllZombieExitTracePlayerDelegate = _checkAllZombieExitTracePlayerCallback;
    }
    #endregion

}
