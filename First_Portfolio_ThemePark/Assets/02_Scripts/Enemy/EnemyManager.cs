using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private SlaughterFactory[] m_Factorys = null;
    private List<Enemy_Slaughter> m_SlaughterList = null;
    private Enemy_Listener[] m_Listeners = null;
    private CCTVManager m_CCTVManager = null;

    private void Awake()
    {
        m_Factorys = GetComponentsInChildren<SlaughterFactory>();
        m_Listeners = GetComponentsInChildren<Enemy_Listener>();
        m_CCTVManager = GetComponentInChildren<CCTVManager>();

        m_CCTVManager.SetDelegate(CCTVDetectCallback);
    }

    private void CCTVDetectCallback(Transform _targetTr)
    {
        for(int i = 0; i < m_SlaughterList.Count; ++i)
        {

        }
    }



    public void GameOver()
    {
       
    }
}
