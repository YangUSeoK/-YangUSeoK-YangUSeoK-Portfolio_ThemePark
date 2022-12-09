using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    public delegate void VoidTransformDelegate(Transform _tr);
    VoidTransformDelegate detectDelegate = null;

    [SerializeField] private bool mbIsTurnOn = false; // 20221118 ��켮 : ���߿� cctv�Ŵ��� ����� false�� �ؾ���. 
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


    // Ray ����
    private Light m_Light = null;
    [SerializeField] private float m_RayToPlayerDistance = 100f;
    private int m_LayerMask = 0;
    // ���̸� �ɰ��� ����. Ŭ���� �߰� �ɰ���
    [SerializeField] private int m_SubDivision = 10;

    // CCTV�� �溸�� �߷��ϴµ� ���� �ɸ��� �ð�
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
    

    // �÷��̾� �߰� -> �÷��̾� �ٶ� -> �÷��̾���ġ�� ���� �� -> ��ֹ� �ڿ� �ȼ����� x�� �� �溸�߷�
    private IEnumerator DetectPlayerCoroutine(Transform _targetTr)
    {
        m_Head.transform.LookAt(_targetTr.position);
        WaitForSeconds ws001 = new WaitForSeconds(0.01f);
        float timer = 0;

        while (mbIsTurnOn && mbIsDetect)
        {
            Debug.DrawLine(transform.position, _targetTr.position, Color.yellow);
            // �÷��̾�� ���̸� ��� �����°� ������
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, _targetTr.position - transform.position, out hitInfo, m_RayToPlayerDistance, m_LayerMask))
            {
                Debug.Log(hitInfo.transform.name);
                if (hitInfo.transform.CompareTag("PLAYER"))
                {
                    //// Ÿ��Tr�� ������(ó�� �÷��̾ �߰��� �Ŷ��) ����
                    //if (m_TargetTr == null)
                    //{
                    //    m_TargetTr = hitInfo.transform;
                    //}
                    m_Head.transform.LookAt(_targetTr.position);

                    // �ð��� ����
                    timer += 0.01f;
                    Debug.Log($"CCTV - �溸���� ���� �ð� :{m_WarningTimer - timer}");
                    if (timer >= m_WarningTimer)
                    {
                        Debug.Log("CCTV : �溸 �߷�!");

                        // 20221118 ��켮 : CCTV �Ŵ������� ��������Ʈ�� �÷��̾� ��ġ�� �����ָ� ��.
                        // CCTV�Ŵ��� -> Enemy�Ŵ��� -> �÷��̾� ��ġ ���� �Ÿ� �� ����� ���� TracePlayer�� �ٲ��ְ� _playerPos ����
                        detectDelegate?.Invoke(_targetTr);
                        AlertSound(_targetTr, _targetTr.position);

                        timer = 0f; // �溸�Ŀ� Ÿ�̸� �ʱ�ȭ
                    }
                    yield return ws001;
                    continue;
                }
                timer = 0f;
                yield return ws001;
                SetUnDetect();
            }

            // ���̸� ���µ� �÷��̾ �Ȱɸ��� UnDetect
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

        // ���ؿ��� ���� ���ͷ� ���̸� ���� �÷��̾� ����
        for (int i = 0; i < m_SubDivision; ++i)
        {
            float ratio = ((float)i) * n;            // ���� ������ ����
            float theta = (Mathf.PI * 2f) * ratio;   // ���� * ���� = ����
            float x = Mathf.Cos(theta) * radius;
            float y = Mathf.Sin(theta) * radius;

            origCircleVert = new Vector3(x, y, 1); //>> z�� �߽����� ���� ���� ��ǥ

            // ȸ������� ���� ȸ����Ų��.
            Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            Matrix4x4 rotMatrix = Matrix4x4.Rotate(rotation);

            newCircleVert = rotMatrix.MultiplyPoint3x4(origCircleVert);

            Debug.DrawLine(transform.position, (transform.position + (newCircleVert * m_Light.range)), Color.green);

            // ���� ������ �� ��ġ�� ���� length ���̸�ŭ ���̸� ��� �ȴ�. 
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

        // CCTV �ٷξ��� ���������� üũ
        Debug.DrawLine(transform.position, transform.position + (transform.forward * m_Light.range), Color.green);
        RaycastHit hitForwardInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitForwardInfo, m_Light.range, m_LayerMask))
        {
            Debug.Log("�������� ���");
            if (hitForwardInfo.transform.CompareTag("PLAYER"))
            {
                Debug.Log("������ �¾Ҵ�");
                SetDetect(hitForwardInfo.transform);
                StartCoroutine(DetectPlayerCoroutine(hitForwardInfo.transform));
            }
        }
    }

    // �÷��̾� �߰��ϸ� ���������Ǿ� ���� Listener �θ�
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
        Debug.Log("CCTV : �÷��̾ ���ƴ�.");
        //m_TargetTr = null;
        m_Light.intensity = 0f;
        mbIsDetect = false;
    }

    private void SetDetect(Transform _tr)
    {
        Debug.Log("CCTV : �÷��̾� �߰�!");
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
