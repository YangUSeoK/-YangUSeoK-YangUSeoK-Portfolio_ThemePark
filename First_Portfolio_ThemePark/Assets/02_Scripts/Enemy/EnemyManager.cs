using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Transform m_PlayerTr = null;
    public Transform PlayerTr
    {
        get { return m_PlayerTr; }
        set { m_PlayerTr = value; }
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
        m_Stalkers = GetComponentsInChildren<Enemy_Stalker>();
        m_CCTVManager = GetComponentInChildren<CCTVManager>();


        for (int i = 0; i < m_Factorys.Length; ++i)
        {
            m_Factorys[i].SetDelegate(GetSlaughterList);
        }
        m_CCTVManager.SetDelegate(CCTVDetectCallback);
        

        // IsAttack ��������Ʈ �Է�
        for(int i = 0; i < m_Enemys.Length; ++i)
        {
            m_Enemys[i].SetDelegate(IsAttack);
        }
        
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

    private void IsAttack()
    {
        // ���ӸŴ������� ��������Ʈ �ݹ�
    }

    public void IsGameOver()
    {
        // ��� ������ ����
        for(int i = 0; i < m_SlaughterList.Count; ++i)
        {
            m_SlaughterList[i].Agent.isStopped = true;
        }

        // CCTV�Ŵ������� IsGameOver ȣ��
        m_CCTVManager.IsGameOver();
    }

    

}
