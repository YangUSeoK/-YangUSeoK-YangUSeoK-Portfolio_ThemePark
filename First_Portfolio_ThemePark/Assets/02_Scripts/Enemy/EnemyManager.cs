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
        m_Enemys = GetComponentsInChildren<Enemy>();
        m_Factorys = GetComponentsInChildren<SlaughterFactory>();
        m_Listeners = GetComponentsInChildren<Enemy_Listener>();
        //m_Stalkers = GetComponentsInChildren<Enemy_Stalker>();
        //m_CCTVManager = GetComponentInChildren<CCTVManager>();


        // CCTV가 플레이어 발견했을 때 콜백 설정
        //m_CCTVManager.SetDelegate(CCTVDetectCallback);


        // 모든 좀비들 IsAttack 델리게이트 콜백 설정
        for (int i = 0; i < m_Enemys.Length; ++i)
        {
            m_Enemys[i].SetDelegate(IsAttack);
        }
    }

    private void Update()
    {
        FindNearZombie();
    }

    // Factory에서 Slaughter 받아오는 델리게이트.
    // 슬러터 생성 후에 리스트 받아와야해서 콜백으로 받아옴
    public void SetFactorys()
    {
        Debug.Log(PlayerTr.name);
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
            m_SlaughterList[i].Agent.isStopped = true;
        }

        // CCTV매니저에서 IsGameOver 호출
        m_CCTVManager.IsGameOver();
    }

    private void CCTVDetectCallback(Transform _targetTr)
    {
        float callRange = 50f;
        for (int i = 0; i < m_SlaughterList.Count; ++i)
        {
            if (Vector3.Distance(_targetTr.position, m_SlaughterList[i].transform.position) <= callRange)
            {
                m_SlaughterList[i].SetState(m_SlaughterList[i].TracePlayer);
            }
        }
    }

    private void FindNearZombie()
    {
        m_NearZombie = m_Enemys[0].transform;
        for(int i = 0; i < m_Enemys.Length-1; ++i)
        {
            if(Vector3.Distance(m_NearZombie.transform.position, m_PlayerTr.position) >=
                Vector3.Distance(m_Enemys[i+1].transform.position, m_PlayerTr.position))
            {
                m_NearZombie = m_Enemys[i + 1].transform;
            }
        }
    }

#region Delegate_Callback

    // Factory에서 좀비들 다 완성되면 리스트를 받음.
    private void InitSlaughterListCallback(List<Enemy_Slaughter> _slaughterList)
    {
        m_SlaughterList = _slaughterList;
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
        Debug.Log("EnemyManager : Player DIe");
        attackDelegate?.Invoke();
        for(int i = 0; i < m_Enemys.Length; ++i)
        {
            m_Enemys[i].Agent.isStopped = true;
            m_Enemys[i].Anim.SetBool("IsAttack", true);
        }
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
