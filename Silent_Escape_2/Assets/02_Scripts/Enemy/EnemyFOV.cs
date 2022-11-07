using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



// ���� ���� �տ� �÷��̾ �ɷȴ��� Ȯ���ϴ� ��ũ��Ʈ
public class EnemyFOV : MonoBehaviour
{
    // �����Ϳ�
    [HideInInspector] public float viewAngle = 120f;
    [HideInInspector] public float viewRange = 0f;

    private FOVMaker m_FovMaker = null;
    private float m_stepRate = 0.75f;


    private Enemy_SightMan m_SightMan = null;
    private Transform m_TargetTr = null;
    private Vector3 m_TargetPos = Vector3.zero;
    private int playerLayer = 0;
    private int obstacleLayer = 0;
    private int lightLayer = 0;
    private int layerMask = 0;

    private void Awake()
    {
        m_SightMan = GetComponent<Enemy_SightMan>();
        m_FovMaker = GetComponent<FOVMaker>();
    }
    private void Start()
    {
        playerLayer = LayerMask.NameToLayer("PLAYER");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        lightLayer = LayerMask.NameToLayer("LIGHT");
        layerMask = (1 << playerLayer) | (1 << obstacleLayer) | (1 << lightLayer);
    }


    // ��������, �����ҳ� transform, ���̾� LayerMask.NameToLayer(" ")
    public bool IsInFOV(float _detectRange, Transform _targetTr, int _layer)
    {
        bool isTargetInFOV = false;

        Collider[] colls = Physics.OverlapSphere(transform.position, _detectRange, 1 << _layer);
        viewRange = _detectRange;

        if (colls.Length >= 1)
        {
            Vector3 dir = (_targetTr.position - transform.position).normalized;

            //                                          -60 ~ 60 �ؼ� 120��
            if (Vector3.Angle(transform.forward, dir) < viewAngle * 0.5f)
            {
                Debug.Log($"���� �ȿ� {_targetTr.name} ����");
                // ��ä�÷� ���� ���� �˻��ϴ� �Լ�
                isTargetInFOV = IsLookTarget();
            }
        }
        return isTargetInFOV;
    }


    // Ÿ�ٿ� ���̽��� ��ֹ� �ִ��� Ȯ��
    public bool IsLookTarget()
    {
        bool isLook = false;

        // ���� * ���� �ݿø�
        int stepCnt = Mathf.RoundToInt(viewAngle * m_stepRate);

        // �ٽ� ���� ������ ������ �ɰ����� �ޱ��� ���� ����
        float stepAngleSize = viewAngle / stepCnt;

        List<Collider> rayPointList = new List<Collider>();


        Debug.Log("��ä�� ���� �߽�!");
        // ��ä�÷� ���̸� ���� ����ü�� ������ �����Ѵ�.
        for (int i = 0; i <= stepCnt; ++i)
        {
            float angle = transform.eulerAngles.y - (viewAngle / 2) + (stepAngleSize * i);

            // ����׿� DrawLine
            
            
            Vector3 dir = m_FovMaker.DirFromAngle(angle, transform.eulerAngles.x, true);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit, m_SightMan.LookPlayerRange))
            {
                if (hit.transform.CompareTag("PLAYER"))
                {
                    Debug.Log("�÷��̾�");
                    m_TargetTr = hit.transform;
                }
            }
            else if (Physics.Raycast(transform.position, dir, out hit, m_SightMan.LookLightRange))
            {
                if (hit.transform.CompareTag("LIGHT"))
                {
                    Debug.Log("Light!");
                    m_TargetPos = hit.point;
                }
            

                isLook = true;
            }
        }

        return isLook;
    }



    public Vector3 CirclePoint(float _angleDegree)
    {
        _angleDegree += transform.eulerAngles.y;

        // y��(���� ȸ��) ������ǥ��� ���� ���ϴ°ɷ� �����ؾ� ��.
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad), 0f, Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad));
    }
}
