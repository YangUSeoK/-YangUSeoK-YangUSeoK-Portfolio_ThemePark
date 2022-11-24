using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaughterFactory : MonoBehaviour
{
    public delegate void VoidSlaughterListDelegate(List<Enemy_Slaughter> _slaughterList);
    private VoidSlaughterListDelegate initSlaughterListDelegate = null;

    // 게임매니저가 플레이어 먹여줘야함
    private List<Enemy_Slaughter> mSlaughterList = null;  
    public List<Enemy_Slaughter> SlaughterList
    {
        get { return mSlaughterList; }
    }
    private Flag[] m_Flags = null;
    private SpawnPoint[] m_SpawnPoints = null;
    private bool mIsActive = false;
    
    [SerializeField] private Transform m_PlayerTr = null;
    [SerializeField] private Enemy_Slaughter m_Enemy = null;    // 소환할 프리팹
    [SerializeField] private float m_SpawnRange = 30f;
    [SerializeField] private float m_UnspawnRange = 60f;



    private void Start()
    {
        mSlaughterList = new List<Enemy_Slaughter>();
        m_Flags = GetComponentInChildren<FlagManager>().Flags;
        m_SpawnPoints = GetComponentInChildren<SpawnPointManager>().SpawnPoints;

        for (int i = 0; i < m_SpawnPoints.Length; ++i)
        {
            Debug.Log(i);
            InstantiateZombie(m_SpawnPoints[i].transform.position);
        }
        initSlaughterListDelegate?.Invoke(mSlaughterList);
    }

    private void Update()
    {
        // 현재 스폰이 안 되있는 상태고 플레이어가 일정 거리안으로 들어오면 SetActive(true)
        if (!mIsActive && Vector3.Distance(m_PlayerTr.position, transform.position) <= m_SpawnRange)
        {
            for (int i = 0; i < mSlaughterList.Count; ++i)
            {
                mSlaughterList[i].gameObject.SetActive(true);
                mIsActive = true;
            }
        }

        // 현재 스폰 되있는 상태고 플레이어가 일정 거리 이상으로 멀어지면 SetActive(false)
        if(mIsActive && Vector3.Distance(m_PlayerTr.position, transform.position) >= m_UnspawnRange)
        {
            for(int i = 0; i < mSlaughterList.Count; ++i)
            {
                mSlaughterList[i].gameObject.SetActive(false);
                mIsActive = false;
            }
        }
    }

    private void InstantiateZombie(Vector3 _spawnPoint)
    {
        Enemy_Slaughter enemy = Instantiate(m_Enemy, _spawnPoint, Quaternion.identity);
        mSlaughterList.Add(enemy);

        enemy.transform.SetParent(transform);
        enemy.PlayerTr = m_PlayerTr;
        enemy.Flags = m_Flags;
        enemy.SetDelegate(SetTracePlayerToNearZombie);
        enemy.gameObject.SetActive(false);
    }

    private void SetTracePlayerToNearZombie(Enemy_Slaughter _caller)
    {
        StartCoroutine(SetTracePlayerToNearZombieCoroutine(_caller));
    }

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

    public void SetDelegate(VoidSlaughterListDelegate _initSlaughterListCallback)
    {
        initSlaughterListDelegate = _initSlaughterListCallback;
    }

}
