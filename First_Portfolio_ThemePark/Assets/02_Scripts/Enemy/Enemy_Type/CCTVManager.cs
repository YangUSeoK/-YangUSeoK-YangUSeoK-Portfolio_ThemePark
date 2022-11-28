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
        // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ö´ï¿½ CCTVï¿½ï¿½ ï¿½ï¿½
        m_CCTVs[mTurnOnIdx].IsTurnOn = false;
        m_CCTVs[mTurnOnIdx].RedLight.SetActive(false);
        Debug.Log($"ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ CCTV : {mTurnOnIdx}");
        // ï¿½Îµï¿½ï¿½ï¿½ 1 ï¿½ï¿½ï¿½ï¿½. ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ã¼ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Å­ ï¿½È´Ù¸ï¿½ 0ï¿½ï¿½ï¿½ï¿½ ï¿½Ê±ï¿½È­
        ++mTurnOnIdx;
        if (mTurnOnIdx == m_CCTVs.Length)
        {
            mTurnOnIdx = 0;
        }

        // ï¿½ï¿½ï¿½ï¿½ CCTVï¿½ï¿½ ï¿½ï¿½
        m_CCTVs[mTurnOnIdx].IsTurnOn = true;
        m_CCTVs[mTurnOnIdx].RedLight.SetActive(true);
        Debug.Log($"ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ CCTV : {mTurnOnIdx}");
    }

    private IEnumerator NextCCTVCoroutine()
    {
        while (true)
        {
            TurnOnNextCCTV();
            yield return mChangeTime;

            // ï¿½ï¿½ï¿½ï¿½Æ®ï¿½ï¿½ ï¿½Ï¸ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½È³Ñ¾î°¨. 
            while (m_CCTVs[mTurnOnIdx].IsDetect)
            {
                yield return mDetectWait;
            }
        }
    }

    public void IsGameOver()
    {
        // ÄÚ·çÆ¾ Á¤Áö 
        StopCoroutine(NextCCTVCoroutine());
        
        // ¸ðµç CCTV TurnOff
        for(int i = 0; i <  
            m_CCTVs.Length; ++i)
        {
            m_CCTVs[i].IsTurnOn = false;
        }

    }
    

    // CCTV.Detect => EnemyManager
    public void SetDelegate(VoidTransformDelegate _cctvDetectCallback)
    {
        cctvDetectCallbackDelegate = _cctvDetectCallback;
    }
}
