using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        for (int i = 0; i < m_Factorys.Length; ++i)
        {
            m_Factorys[i].SetDelegate(GetSlaughterList);
        }
        m_CCTVManager.SetDelegate(CCTVDetectCallback);


    }

    private void CCTVDetectCallback(Transform _targetTr)
    {
        float callRange = 50f;
        for(int i = 0; i < m_SlaughterList.Count; ++i)
        {
            if(Vector3.Distance(_targetTr.position, m_SlaughterList[i].transform.position) <= callRange)
            {
                m_SlaughterList[i].SetState(m_SlaughterList[i].TracePlayer);
            }
        }
    }

    private void GetSlaughterList(List<Enemy_Slaughter> _slaughterList)
    {
        m_SlaughterList = _slaughterList;
    }

    public void GameOver()
    {
       
    }

    

}
