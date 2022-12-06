using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글턴
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static GameManager instance = null;

    private bool mbIsGameOver = false;
    public bool IsGameOver
    {
        get { return mbIsGameOver; }
    }
    private EnemyManager m_EnemyManager = null;
    private SoundManager m_SoundManager = null;
    private UIManager m_UIManager = null;
    [SerializeField] private Transform m_PlayerTr = null;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);

        m_EnemyManager = GetComponentInChildren<EnemyManager>();
        m_SoundManager = GetComponentInChildren<SoundManager>();
        m_UIManager = GetComponentInChildren<UIManager>();
        //m_PlayerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
        SetEnemyManager();
        
        m_EnemyManager.SetDelegate(AllZombieEnterPatrolCallback, EnterTracePlayerCallback, AllZombieExitTracePlayerCallback, GameOver);

    }
   private void SetEnemyManager()
    {
        m_EnemyManager.PlayerTr = m_PlayerTr;
        m_EnemyManager.SetFactorys();
    }
    


    #region Enemy_State_Callback
    private void AllZombieEnterPatrolCallback()
    {
        m_SoundManager.SetPatrolBGM();
    }
    private void EnterTracePlayerCallback()
    {
        Debug.Log("4");
        m_SoundManager.SetTracePlayerBGM();
    }
    private void AllZombieExitTracePlayerCallback()
    {
        m_SoundManager.FadeOutTracePlayerBGM();
    }
    private void GameOver()
    {
        mbIsGameOver = true;
        m_EnemyManager.IsGameOver();
        m_UIManager.IsGameOver();
        m_SoundManager.IsGameOver();
        m_PlayerTr.GetComponent<PlayerCtrl>().IsGameOver();

        // 씬전환 -> 메인타이틀로
    }

    private void GameClear()//221201 김준우
    {
        //조건식

    }
    #endregion

    #region UI
    private void OpenInventory()
    {
        m_UIManager.OpenInventory();
    }

    private void CloseInventory()
    {
        m_UIManager.CloseInventory();
    }
    #endregion
}
