using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public delegate void VoidVoidDelegate();
    private VoidVoidDelegate allZombieEnterPatrolDelegate = null;
    private VoidVoidDelegate enterTracePlayerDelegate = null;
    private VoidVoidDelegate allZombieExitTracePlayerDelegate = null;
    private VoidVoidDelegate attackDelegate = null;

    [SerializeField] private Transform m_PlayerTr = null;  // 디버그
    public Transform PlayerTr
    {
        get { return m_PlayerTr; }
        set { m_PlayerTr = value; }
    }
    private Transform m_NearZombie = null;
    public Transform NearZombie
    {
        get { return m_NearZombie; }
    }

    private SlaughterFactory[] m_Factorys = null;
    private Enemy[] m_Enemys = null;
    private List<Enemy_Slaughter> m_SlaughterList = null;
    private Enemy_Listener[] m_Listeners = null;
    private Enemy_Stalker[] m_Stalkers = null;
    private CCTVManager m_CCTVManager = null;

    private void Awake()
    {
        m_SlaughterList = new List<Enemy_Slaughter>();
        m_Factorys = GetComponentsInChildren<SlaughterFactory>();
        m_Listeners = GetComponentsInChildren<Enemy_Listener>();
        m_Stalkers = GetComponentsInChildren<Enemy_Stalker>();
        m_CCTVManager = GetComponentInChildren<CCTVManager>();
        m_Enemys = GetComponentsInChildren<Enemy>();

        // CCTV가 플레이어 발견했을 때 콜백 설정
        //m_CCTVManager.SetDelegate(CCTVDetectCallback);
        SetFactorys();

        
    }
    private void Start()
    {
        Invoke("SetEnemys", 1f);
    }

    private void Update()
    {
        FindNearZombie();
    }

    private void SetEnemys()
    {
        for (int i = 0; i < m_Factorys.Length; ++i)
        {
            m_Factorys[i].SetActiveZombies();
        }

        // 모든 좀비들 IsAttack 델리게이트 콜백 설정
        for (int i = 0; i < m_Enemys.Length; ++i)
        {
            m_Enemys[i].SetDelegate(IsAttack);
        }
    }
    
    public void SetFactorys()
    {
        for (int i = 0; i < m_Factorys.Length; ++i)
        {
            m_Factorys[i].PlayerTr = m_PlayerTr;
            m_Factorys[i].SetDelegate(InitSlaughterListCallback, AllZombieEnterPatrolCallback, EnterTracePlayerCallback, AllZombieExitTracePlayerCallback);
        }
    }

    public void IsGameOver()
    {
        // 모든 슬러터 정지
        for (int i = 0; i < m_SlaughterList.Count; ++i)
        {
            if (m_SlaughterList[i].gameObject.activeSelf)
            {
                m_SlaughterList[i].Agent.isStopped = true;
            }
        }

        // CCTV매니저에서 IsGameOver 호출
        m_CCTVManager.IsGameOver();
    }

    public void CCTVDetectCallback(Transform _targetTr)
    {
        float callRange = 50f;
        for (int i = 0; i < m_Factorys.Length; ++i)
        {
            for (int j = 0; j < m_Factorys[i].SlaughterList.Count; ++j)
            {
                if (Vector3.Distance(_targetTr.position, m_Factorys[i].SlaughterList[j].transform.position) <= callRange)
                {
                    if (m_Factorys[i].SlaughterList[j].gameObject.activeSelf)
                    {
                        m_Factorys[i].SlaughterList[j].SetState(m_SlaughterList[i].TracePlayer);
                    }
                }
            }
        }
    }

    public void CallNearZombie(Transform _targetTr, float _callRange)
    {
        CCTVDetectCallback(_targetTr);

        Collider[] listener = Physics.OverlapSphere(transform.position, _callRange, 1 << LayerMask.NameToLayer("LISTENER"));

        if (listener.Length != 0)
        {
            for (int i = 0; i < listener.Length; ++i)
            {
                listener[i].transform.GetComponent<Enemy_Listener>().Listen(_targetTr, _targetTr.position, 100f);
            }
        }
    }


    private void FindNearZombie()
    {
        m_NearZombie = m_Enemys[0].transform;

        if (m_Enemys.Length == 1) return;

        for(int i = 0; i < m_Enemys.Length -1; ++i)
        {
            if (m_Enemys[i].gameObject.activeSelf)
            {
                if (Vector3.Distance(m_NearZombie.transform.position, m_PlayerTr.position) >=
                    Vector3.Distance(m_Enemys[i + 1].transform.position, m_PlayerTr.position))
                {
                    m_NearZombie = m_Enemys[i + 1].transform;
                }
            }
        }
    }

#region Delegate_Callback

    // Factory에서 좀비들 다 완성되면 리스트를 받음.
    private void InitSlaughterListCallback(List<Enemy_Slaughter> _slaughterList)
    {
        m_SlaughterList.AddRange(_slaughterList);
    }

    private void AllZombieEnterPatrolCallback()
    {

    }

    private void EnterTracePlayerCallback()
    {
        enterTracePlayerDelegate?.Invoke();
    }

    private void AllZombieExitTracePlayerCallback()
    {
        allZombieExitTracePlayerDelegate?.Invoke();
    }

    private void IsAttack()
    {
        attackDelegate?.Invoke();
    }

    public void SetDelegate(VoidVoidDelegate _allZombieEnterPatrolCallback, VoidVoidDelegate _enterTracePlayerCallback,
                            VoidVoidDelegate _allZombieExitTracePlayerCallback, VoidVoidDelegate _attackCallback)
    {

        allZombieEnterPatrolDelegate = _allZombieEnterPatrolCallback;
        enterTracePlayerDelegate = _enterTracePlayerCallback;
        allZombieExitTracePlayerDelegate = _allZombieExitTracePlayerCallback;
        attackDelegate = _attackCallback;
    }
    #endregion




}
