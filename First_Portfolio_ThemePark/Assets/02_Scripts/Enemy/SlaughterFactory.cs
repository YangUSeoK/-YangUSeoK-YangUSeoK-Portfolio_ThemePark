using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaughterFactory : MonoBehaviour
{
    // 게임매니저가 플레이어 먹여줘야함
    [SerializeField] private Transform m_PlayerTr = null;
    [SerializeField] private List<Enemy_Slaughter> mEnemyList = null;  // 디버그용
    private Flag[] m_Flags = null;
    private bool mIsActive = false;
    
    [SerializeField] private Enemy_Slaughter m_Enemy = null;
    [SerializeField] private int m_SpawnCnt = 3;
    [SerializeField] private float m_SpawnRange = 30f;



    private void Awake()
    {
        
        
    }
    private void Start()
    {
        m_Flags = GetComponentInChildren<FlagManager>().Flags;
        Debug.Log(m_Flags.Length);
        for (int i = 0; i < m_SpawnCnt; ++i)
        {
            InstantiateZombie(m_Flags[i].transform.position);
        }
    }

    private void Update()
    {
        // 현재 스폰이 안 되있는 상태고 플레이어가 일정 거리안으로 들어오면 SetActive(true)
        if (!mIsActive && Vector3.Distance(m_PlayerTr.position, transform.position) <= m_SpawnRange)
        {
            for (int i = 0; i < mEnemyList.Count; ++i)
            {
                mEnemyList[i].gameObject.SetActive(true);
                mIsActive = true;
            }
        }
    }

    private void InstantiateZombie(Vector3 _spawnPoint)
    {
        Enemy_Slaughter enemy = Instantiate(m_Enemy, _spawnPoint, Quaternion.identity);
        mEnemyList.Add(enemy);
        enemy.PlayerTr = m_PlayerTr;
        enemy.Flags = m_Flags;
        enemy.SetDelegate(SetTracePlayerNearZombie);
        //enemy.gameObject.SetActive(false);
    }

    private void SetTracePlayerNearZombie(Enemy_Slaughter _caller)
    {
        StartCoroutine(SetTracePlayerNearZombieCoroutine(_caller));
    }

    private IEnumerator SetTracePlayerNearZombieCoroutine(Enemy_Slaughter _caller)
    {
        List<Enemy_Slaughter> calledZombiesList = new List<Enemy_Slaughter>();
        for (int i = 0; i < mEnemyList.Count; ++i)
        {
            // 부른놈이 아니고 추격중이 아닌놈
            if (mEnemyList[i] != _caller && mEnemyList[i].CurState != mEnemyList[i].TracePlayer)
            {
                calledZombiesList.Add(mEnemyList[i]);
            }
        }

        if (calledZombiesList.Count != 0)
        {
            Enemy_Slaughter nearZombie = calledZombiesList[0];
            for (int i = 0; i < calledZombiesList.Count - 1; ++i)
            {
                if ((_caller.transform.position - calledZombiesList[i].transform.position).sqrMagnitude
                    >= (_caller.transform.position - calledZombiesList[i + 1].transform.position).sqrMagnitude)
                {
                    nearZombie = calledZombiesList[i + 1];
                }
            }
            nearZombie.SetState(nearZombie.TracePlayer);
        }
        yield return null;
    }
}
