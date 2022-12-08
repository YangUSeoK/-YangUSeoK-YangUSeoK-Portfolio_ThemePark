using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private bool mbIsGameClear = false;
    public bool IsGameClear
    {
        get { return mbIsGameClear; }
    }

    private UIManager m_UIManager = null;
    private OnClickButton m_ClickButton = null;
    [SerializeField] private GameMenuManager m_GameMenuManager = null;

    [SerializeField] private EnemyManager m_EnemyManager = null;
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

        m_UIManager = GetComponentInChildren<UIManager>();
        m_PlayerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
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
        SoundManager.instance.SetPatrolBGM();
    }
    private void EnterTracePlayerCallback()
    {
        SoundManager.instance.SetTracePlayerBGM();
    }
    private void AllZombieExitTracePlayerCallback()
    {
        SoundManager.instance.FadeOutTracePlayerBGM();
    }
    private void GameOver()
    {
        mbIsGameOver = true;
        m_EnemyManager.AllStop();
        m_UIManager.IsGameOver();
        SoundManager.instance.IsGameOver();
        m_PlayerTr.GetComponent<PlayerCtrl>().IsGameOver();

        // 타임스케일 0으로 
        Time.timeScale= 0f;
        // 씬전환 -> 메인타이틀로
        m_ClickButton.GetOutGameScene();
        // 타임스케일 정상화
        //Time.timeScale = 0f;
    }

    public void GameClear()//221201 김준우
    {
        //조건식
        mbIsGameClear = true;
        m_EnemyManager.AllStop();
        m_UIManager.IsGameClear();
        SoundManager.instance.IsGameClear();
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
