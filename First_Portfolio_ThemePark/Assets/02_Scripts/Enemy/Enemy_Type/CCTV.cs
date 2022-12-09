using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    public delegate void VoidTransformDelegate(Transform _tr);
    VoidTransformDelegate detectDelegate = null;

    [SerializeField] private bool mbIsTurnOn = false; // 20221118 양우석 : 나중에 cctv매니저 만들면 false로 해야함. 
    public bool IsTurnOn
    {
        get { return mbIsTurnOn; }
        set { mbIsTurnOn = value; }
    }

    [SerializeField] private bool mbIsDetect = false;
    public bool IsDetect
    {
        get { return mbIsDetect; }
    }

    private Vector3 m_OriAngle = Vector3.zero;
    [SerializeField] private float m_SoundRange = 50f;
    private float m_VolumeLevel = 100;

    private GameObject m_RedLight = null;
    public GameObject RedLight
    {
        get { return m_RedLight; }
    }

    [SerializeField] private Transform m_Head = null;


    // Ray 관련
    private Light m_Light = null;
    [SerializeField] private float m_RayToPlayerDistance = 100f;
    private int m_LayerMask = 0;
    // 레이를 쪼개는 비율. 클수록 잘게 쪼갠다
    [SerializeField] private int m_SubDivision = 10;

    // CCTV가 경보를 발령하는데 까지 걸리는 시간
    [SerializeField] private float m_WarningTimer = 3f;


    private void Awake()
    {
        m_Light = GetComponent<Light>();
        m_Light.intensity = 0f;
        m_RedLight = GetComponentInChildren<RedLight>().gameObject;
        m_RedLight.SetActive(false);
    }

    private void Start()
    {
        m_OriAngle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        m_LayerMask = 1 << LayerMask.NameToLayer("PLAYER") | 1 << LayerMask.NameToLayer("OBSTACLE");
    }

    private void LateUpdate()
    {
        if (mbIsTurnOn && !mbIsDetect)
        {
            RayCone();
        }
    }
    

    // 플레이어 발견 -> 플레이어 바라봄 -> 플레이어위치로 레이 쏨 -> 장애물 뒤에 안숨으면 x초 후 경보발령
    private IEnumerator DetectPlayerCoroutine(Transform _targetTr)
    {
        m_Head.transform.LookAt(_targetTr.position);
        WaitForSeconds ws001 = new WaitForSeconds(0.01f);
        float timer = 0;

        while (mbIsTurnOn && mbIsDetect)
        {
            Debug.DrawLine(transform.position, _targetTr.position, Color.yellow);
            // 플레이어에게 레이를 쏘고 가리는게 없으면
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, _targetTr.position - transform.position, out hitInfo, m_RayToPlayerDistance, m_LayerMask))
            {
                Debug.Log(hitInfo.transform.name);
                if (hitInfo.transform.CompareTag("PLAYER"))
                {
                    //// 타겟Tr이 없으면(처음 플레이어를 발견한 거라면) 저장
                    //if (m_TargetTr == null)
                    //{
                    //    m_TargetTr = hitInfo.transform;
                    //}
                    m_Head.transform.LookAt(_targetTr.position);

                    // 시간초 세기
                    timer += 0.01f;
                    Debug.Log($"CCTV - 경보까지 남은 시간 :{m_WarningTimer - timer}");
                    if (timer >= m_WarningTimer)
                    {
                        Debug.Log("CCTV : 경보 발령!");

                        // 20221118 양우석 : CCTV 매니저한테 델리게이트로 플레이어 위치를 전해주면 됨.
                        // CCTV매니저 -> Enemy매니저 -> 플레이어 위치 일정 거리 내 좀비들 상태 TracePlayer로 바꿔주고 _playerPos 전달
                        detectDelegate?.Invoke(_targetTr);
                        AlertSound(_targetTr, _targetTr.position);

                        timer = 0f; // 경보후에 타이머 초기화
                    }
                    yield return ws001;
                    continue;
                }
                timer = 0f;
                yield return ws001;
                SetUnDetect();
            }

            // 레이를 쐈는데 플레이어가 안걸리면 UnDetect
            SetUnDetect();
        }
    }

    private void RayCone()
    {
        float n = 1f / (m_SubDivision);
        float radius = Mathf.Tan((m_Light.spotAngle * 0.5f) * Mathf.Deg2Rad);
        float length = m_Light.range / Mathf.Cos(m_Light.spotAngle * 0.5f * Mathf.Deg2Rad);
        Vector3 origCircleVert;
        Vector3 newCircleVert;

        // 기준원을 향한 벡터로 레이를 쏴서 플레이어 검출
        for (int i = 0; i < m_SubDivision; ++i)
        {
            float ratio = ((float)i) * n;            // 원을 비율로 나눔
            float theta = (Mathf.PI * 2f) * ratio;   // 원주 * 비율 = 각도
            float x = Mathf.Cos(theta) * radius;
            float y = Mathf.Sin(theta) * radius;

            origCircleVert = new Vector3(x, y, 1); //>> z축 중심으로 만든 원의 좌표

            // 회전행렬을 통해 회전시킨다.
            Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            Matrix4x4 rotMatrix = Matrix4x4.Rotate(rotation);

            newCircleVert = rotMatrix.MultiplyPoint3x4(origCircleVert);

            Debug.DrawLine(transform.position, (transform.position + (newCircleVert * m_Light.range)), Color.green);

            // 이제 각각의 점 위치를 향해 length 길이만큼 레이를 쏘면 된다. 
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, newCircleVert, out hitInfo, length, m_LayerMask))
            {
                if (hitInfo.transform.CompareTag("PLAYER"))
                {
                    SetDetect(hitInfo.transform);
                    StartCoroutine(DetectPlayerCoroutine(hitInfo.transform));
                }
            }
        }

        // CCTV 바로앞을 지나갈때도 체크
        Debug.DrawLine(transform.position, transform.position + (transform.forward * m_Light.range), Color.green);
        RaycastHit hitForwardInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitForwardInfo, m_Light.range, m_LayerMask))
        {
            Debug.Log("직선으로 쏜다");
            if (hitForwardInfo.transform.CompareTag("PLAYER"))
            {
                Debug.Log("직선에 맞았다");
                SetDetect(hitForwardInfo.transform);
                StartCoroutine(DetectPlayerCoroutine(hitForwardInfo.transform));
            }
        }
    }

    // 플레이어 발견하면 오버랩스피어 쏴서 Listener 부름
    private void AlertSound(Transform _tr, Vector3 _soundPos)
    {
        Collider[] listener = Physics.OverlapSphere(transform.position, m_SoundRange, 1 << LayerMask.NameToLayer("LISTENER"));

        if(listener.Length != 0)
        {
            for(int i = 0; i < listener.Length; ++i)
            {
                listener[i].transform.GetComponent<Enemy_Listener>().Listen(_tr, _soundPos, m_VolumeLevel);
            }
        }
    }

    private void SetUnDetect()
    {
        Debug.Log("CCTV : 플레이어를 놓쳤다.");
        //m_TargetTr = null;
        m_Light.intensity = 0f;
        mbIsDetect = false;
    }

    private void SetDetect(Transform _tr)
    {
        Debug.Log("CCTV : 플레이어 발견!");
        //m_TargetTr = _tr.transform;
        m_Light.intensity = 2f;
        mbIsDetect = true;
    }

    #region Delegate_Callback
    public void SetDelegate(VoidTransformDelegate _detectCallback)
    {
        detectDelegate = _detectCallback;
    }
    #endregion
}
