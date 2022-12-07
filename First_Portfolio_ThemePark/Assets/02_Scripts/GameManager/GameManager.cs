using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // �̱���
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
    private SoundManager m_SoundManager = null;
    private UIManager m_UIManager = null;

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

        m_SoundManager = GetComponentInChildren<SoundManager>();
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

        // Ÿ�ӽ����� 0���� 
        // ����ȯ -> ����Ÿ��Ʋ��
        // Ÿ�ӽ����� ����ȭ
        //Time.timeScale = 0f;
    }

    private void GameClear()//221201 ���ؿ�
    {
        //���ǽ�
        mbIsGameOver = true;
        m_EnemyManager.IsGameOver();
        m_UIManager.IsGameClear();
        m_SoundManager.IsGameClear();
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
