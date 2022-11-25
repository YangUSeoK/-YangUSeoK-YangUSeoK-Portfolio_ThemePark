using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVManager : MonoBehaviour
{
    public delegate void VoidTransformDelegate(Transform _tr);
    VoidTransformDelegate cctvDetectCallbackDelegate = null;

    [SerializeField] private CCTV[] m_CCTVs = null;
    public CCTV[] CCTVs
    {
        get { return m_CCTVs; }
    }
    [SerializeField] private float m_ChangeTime = 10f;
    [SerializeField] private float m_DetectWait = 0.5f;

    private WaitForSeconds mChangeTime;
    private WaitForSeconds mDetectWait;
    private int mTurnOnIdx = 0;


    private void Awake()
    {
        m_CCTVs = GetComponentsInChildren<CCTV>();
        mChangeTime = new WaitForSeconds(m_ChangeTime);
        mDetectWait = new WaitForSeconds(m_DetectWait);
    }

    private void Start()
    {
        for(int i = 0; i < m_CCTVs.Length; ++i)
        {
            m_CCTVs[i].SetDelegate(DetectCallback);
        }

        StartCoroutine(NextCCTVCoroutine());
    }

    private void DetectCallback(Transform _tr)
    {
        cctvDetectCallbackDelegate?.Invoke(_tr);
    }

    private void TurnOnNextCCTV()
    {
        // 현재 켜져있는 CCTV를 끔
        m_CCTVs[mTurnOnIdx].IsTurnOn = false;
        m_CCTVs[mTurnOnIdx].RedLight.SetActive(false);
        Debug.Log($"꺼지는 CCTV : {mTurnOnIdx}");
        // 인덱스 1 증가. 만약 전체개수만큼 된다면 0으로 초기화
        ++mTurnOnIdx;
        if (mTurnOnIdx == m_CCTVs.Length)
        {
            mTurnOnIdx = 0;
        }

        // 다음 CCTV를 켬
        m_CCTVs[mTurnOnIdx].IsTurnOn = true;
        m_CCTVs[mTurnOnIdx].RedLight.SetActive(true);
        Debug.Log($"켜지는 CCTV : {mTurnOnIdx}");
    }

    private IEnumerator NextCCTVCoroutine()
    {
        while (true)
        {
            TurnOnNextCCTV();
            yield return mChangeTime;

            // 디텍트를 하면 다음으로 안넘어감. 
            while (m_CCTVs[mTurnOnIdx].IsDetect)
            {
                yield return mDetectWait;
            }
        }
    }

    // CCTV.Detect => EnemyManager
    public void SetDelegate(VoidTransformDelegate _cctvDetectCallback)
    {
        cctvDetectCallbackDelegate = _cctvDetectCallback;
    }
}
