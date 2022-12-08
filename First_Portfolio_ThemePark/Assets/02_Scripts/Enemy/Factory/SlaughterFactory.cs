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

    // ���ӸŴ����� �÷��̾� �Կ������
    private List<Enemy_Slaughter> mSlaughterList = null;
    public List<Enemy_Slaughter> SlaughterList
    {
        get { return mSlaughterList; }
    }
    [SerializeField] private Flag[] m_Flags = null;
    [SerializeField] private SpawnPoint[] m_SpawnPoints = null;
    private bool mIsActive = false;

    private Transform m_PlayerTr = null;   // �����
    public Transform PlayerTr
    {
        set { m_PlayerTr = value; }
    }
    [SerializeField] private Enemy_Slaughter m_Enemy = null;    // ��ȯ�� ������
    [SerializeField] private float m_SpawnRange = 30f;
    [SerializeField] private float m_UnspawnRange = 60f;

    private bool mb_IsUnlocking = false; // �����
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
            // ���� ������ �� ���ִ� ���°� �÷��̾ ���� �Ÿ������� ������ SetActive(true)
            if (!mIsActive && Vector3.Distance(m_PlayerTr.position, transform.position) <= m_SpawnRange)
            {
                SetActiveZombies();
            }

            // ���� ���� ���ִ� ���°� �÷��̾ ���� �Ÿ� �̻����� �־����� SetActive(false)
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

    // ����� ���� ã�Ƽ� SetState(TracePlayer)
    private IEnumerator SetTracePlayerToNearZombieCoroutine(Enemy_Slaughter _caller)
    {
        List<Enemy_Slaughter> calledZombiesList = new List<Enemy_Slaughter>();
        for (int i = 0; i < mSlaughterList.Count; ++i)
        {
            // �θ����� �ƴϰ� �߰����� �ƴѳ��� ����Ʈ�� �߰��Ѵ�.
            if (mSlaughterList[i] != _caller && mSlaughterList[i].CurState != mSlaughterList[i].TracePlayer)
            {
                calledZombiesList.Add(mSlaughterList[i]);
            }
        }

        // �θ� ���� 0������ �ƴϸ�
        if (calledZombiesList.Count != 0)
        {
            // ���� ����Ʈ �� ���� ����� ���� ���Ѵ�.
            Enemy_Slaughter nearZombie = calledZombiesList[0];
            for (int i = 0; i < calledZombiesList.Count - 1; ++i)
            {
                if ((_caller.transform.position - calledZombiesList[i].transform.position).sqrMagnitude
                    >= (_caller.transform.position - calledZombiesList[i + 1].transform.position).sqrMagnitude)
                {
                    nearZombie = calledZombiesList[i + 1];
                }
            }
            // ���� ����� ������ ���¸� TracePlayer�� ����.
            nearZombie.SetState(nearZombie.TracePlayer);
        }
        yield return null;
    }

    #region Delegate_Callback

    // ��� ���� ��Ʈ���̶��
    private void AllZombieEnterPatrol()
    {
        Debug.Log("���� Enter Patrol");
        int cnt = 0;
        for (int i = 0; i < mSlaughterList.Count; ++i)
        {
            if (mSlaughterList[i].CurState != mSlaughterList[i].Patrol)
            {
                ++cnt;
            }

        }

        // ��Ʈ�Ѻ�� 
        if (cnt == 0)
        {
            Debug.Log("������� Patrol");
            checkAllZombieEnterPatrolDelegate?.Invoke();
        }
    }

    private void EnterTracePlayerCallback()
    {
        enterTracePlayerDelegate?.Invoke();
    }

    private void AllZombieExitTracePlayer()
    {
        Debug.Log("���� Exit TracePlayer");
        int cnt = 0;

        for (int i = 0; i < mSlaughterList.Count; ++i)
        {
            if (mSlaughterList[i].CurState == mSlaughterList[i].TracePlayer)
            {
                ++cnt;
                Debug.Log("cnt up");
            }
        }

        Debug.Log($"Trace ���� �� : {cnt}");
        if (cnt == 0)
        {
            Debug.Log("������� Exit TracePlayer");
            // ������� ������ FadeOut
            checkAllZombieExitTracePlayerDelegate?.Invoke();
        }
    }

    public void SetDelegate(VoidSlaughterListDelegate _initCallback,  VoidVoidDelegate _checkAllZombieEnterPatrolCallback,
                            VoidVoidDelegate _enterTracePlayerCallback, VoidVoidDelegate _checkAllZombieExitTracePlayerCallback)
    {
        Debug.Log("���丮 �µ���");
        initDelegate = _initCallback;
        checkAllZombieEnterPatrolDelegate = _checkAllZombieEnterPatrolCallback;
        enterTracePlayerDelegate = _enterTracePlayerCallback;
        checkAllZombieExitTracePlayerDelegate = _checkAllZombieExitTracePlayerCallback;
    }
    #endregion

}
