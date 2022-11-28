using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static GameManager instance;

    private EnemyManager m_EnemyManager = null;
    private SoundManager m_SoundManager = null;
    private UIManager m_UIManager = null;
    private Transform m_PlayerTr = null;


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
        m_EnemyManager.PlayerTr = m_PlayerTr;

        m_SoundManager = GetComponentInChildren<SoundManager>();
        m_UIManager = GetComponentInChildren<UIManager>();
        m_PlayerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;


    }

    public bool mbIsGameOver = false;

    private void GameOver()
    {
        mbIsGameOver = true;
        m_EnemyManager.IsGameOver();
        m_UIManager.IsGameOver();
        m_SoundManager.IsGameOver();
        //m_PlayerTr.GetComponent<PlayerCtrl>().IsGameOver();

        // 씬전환 -> 메인타이틀로
    }

}
