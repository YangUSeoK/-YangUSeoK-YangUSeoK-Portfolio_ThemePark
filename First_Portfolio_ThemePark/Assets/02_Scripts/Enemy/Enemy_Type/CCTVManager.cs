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
        Debug.Log("CCTV 매니저 콜백받음");
        cctvDetectCallbackDelegate?.Invoke(_tr);

        // 전체 가로등 불빛 시뻘겋게


        // 경고음이 출력중이 아닐때 경고음 출력
        AudioSource audio = GetComponent<AudioSource>();
        if (!audio.isPlaying)
        {
            audio.PlayOneShot(audio.clip);
        }
    }

    private void TurnOnNextCCTV()
    {
        // ���� �����ִ� CCTV�� ��
        m_CCTVs[mTurnOnIdx].IsTurnOn = false;
        m_CCTVs[mTurnOnIdx].RedLight.SetActive(false);
        Debug.Log($"������ CCTV : {mTurnOnIdx}");
        // �ε��� 1 ����. ���� ��ü������ŭ �ȴٸ� 0���� �ʱ�ȭ
        ++mTurnOnIdx;
        if (mTurnOnIdx == m_CCTVs.Length)
        {
            mTurnOnIdx = 0;
        }

        // ���� CCTV�� ��
        m_CCTVs[mTurnOnIdx].IsTurnOn = true;
        m_CCTVs[mTurnOnIdx].RedLight.SetActive(true);
        Debug.Log($"������ CCTV : {mTurnOnIdx}");
    }

    private IEnumerator NextCCTVCoroutine()
    {
        while (true)
        {
            TurnOnNextCCTV();
            yield return mChangeTime;

            // ����Ʈ�� �ϸ� �������� �ȳѾ. 
            while (m_CCTVs[mTurnOnIdx].IsDetect)
            {
                yield return mDetectWait;
            }
        }
    }

    public void IsGameOver()
    {
        // �ڷ�ƾ ���� 
        StopCoroutine(NextCCTVCoroutine());
        
        // ��� CCTV TurnOff
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
