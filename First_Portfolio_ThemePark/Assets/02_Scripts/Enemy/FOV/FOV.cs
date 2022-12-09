using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// ���� ���� �տ� �÷��̾ �ɷȴ��� Ȯ���ϴ� ��ũ��Ʈ
public class FOV : MonoBehaviour
{
    // �����Ϳ�
    public float m_Angle = 120f;
    public float m_Range = 10f;

    private float mHeight = 1.4f;
    private float mHalfHeight;

    protected int m_PlayerLayer = 0;
    public int PlayerLayer
    {
        get { return m_PlayerLayer; }
    }

    protected int m_ObstacleLayer = 0;
    public int ObstacleLayer
    {
        get { return m_ObstacleLayer; }
    }

    protected int m_LightLayer = 0;
    public int LightLayer
    {
        get { return m_LightLayer; }
    }

    protected int m_FlashLayer = 0;
    public int FlashLayer
    {
        get { return m_FlashLayer; }
    }

    protected int m_LayerMask = 0;
    public int mLayerMask
    {
        get { return m_LayerMask; }
    }

    protected Transform m_TargetTr = null;
    protected Enemy m_Enemy;

    protected void Awake()
    {
        m_Enemy = GetComponent<Enemy>();
    }

    protected virtual void Start()
    {
        m_PlayerLayer = 1 << LayerMask.NameToLayer("PLAYER");
        m_ObstacleLayer = 1 << LayerMask.NameToLayer("OBSTACLE");
        m_LightLayer = 1 << LayerMask.NameToLayer("LIGHT");
        m_FlashLayer = 1 << LayerMask.NameToLayer("FLASH");

        m_LayerMask = ~((1 << m_PlayerLayer) | (1 << m_ObstacleLayer) | (1 << m_LightLayer) | (1 << m_FlashLayer));

        mHalfHeight = mHeight * 0.5f;
    }

    public bool IsInFOV(float _detectRange, float _angle, int _layerMask)
    {
        bool isInFOV = false;
        // Enemy�� �����ǿ������� viewRange ��ŭ ��ü�� 
        // �׷��� �� �߿� playerLayer �� ������ colls �迭�� �߰�
        Collider[] colls = new Collider[1];
        Physics.OverlapSphereNonAlloc(transform.position, 100f /*_detectRange*/, colls, _layerMask);


        // �÷��̾� ���̾ ����Ǿ��� ��
        if (colls[0] != null)
        {
            Vector3 dir = (colls[0].transform.position - transform.position).normalized;

            // ���� �� ������⿡�� ��� ���� dir������ ������ 120�� ���� �ȿ� ������
            if (Vector3.Angle(transform.forward, dir) <= _angle * 0.5f)
            {
                isInFOV = true;
            }
            //Debug.Log($"�÷��̾�� ������� ���� :{transform.position} / {Vector3.Angle(transform.forward, dir)} / {isInFOV}");
        }
        return isInFOV;
    }

    public bool IsLookDirect(Transform _targetTr, float _detectRange, int _layerMask)
    {
        bool isLook = false;

        RaycastHit hitInfo;
        Vector3 dir = (_targetTr.position - transform.position).normalized;

        // 20221114 ��켮 : ���̽�� ��ġ ���񿡵��� �����ؾ� ��
        // 20221202 ��켮 : �������� ���̴� offset ������
        if (Physics.Raycast(transform.position + (Vector3.up * mHalfHeight), dir, out hitInfo, _detectRange, m_LayerMask))
        {
            //Debug.Log($" FOV IsLookDirect / �ؿ������� ������ : {hitInfo.transform.name}");
            isLook = hitInfo.collider.CompareTag(_targetTr.tag);
        }

        // 20221203 ��켮 : �Ʒ��ʿ� �ϳ�, ���ʿ� �ϳ� ���� �ΰ� ��
        if (!isLook)
        {
            if (Physics.Raycast(transform.position + (Vector3.up * mHeight), dir, out hitInfo, _detectRange, m_LayerMask))
            {
                //Debug.Log($" FOV IsLookDirect / ���������� ������ : {hitInfo.transform.name}");
                isLook = hitInfo.collider.CompareTag(_targetTr.tag);
            }
        }

        if (isLook)
        {
            Debug.Log($"���� ��� : {transform.name}");
        }
        return isLook;
    }

    // ��ä�÷� ���̸� ���� �ε����� ��ġ�� �����Ѵ�.
    public bool IsInFovWithRayCheckDirect(float _detectRange, float _angle, string _tag, int _layerMask, ref Vector3 _collPos)
    {
        bool isInDirectFOV = false;
        int stepCnt = 20;
        float stepAngleSize = _angle / stepCnt;

        for (int i = 0; i <= stepCnt; ++i)
        {
            float rayAngle = ((_angle / 2) + (stepAngleSize * i)) - _angle;
            Vector3 dir = DirFromAngle(rayAngle);

            Debug.DrawLine(transform.position + (Vector3.up * mHeight), transform.position + (Vector3.up * mHeight) + (dir * _detectRange), Color.green);
            Debug.DrawLine(transform.position + (Vector3.up * mHalfHeight), transform.position + (Vector3.up * mHalfHeight) + (dir * _detectRange), Color.green);


            RaycastHit hitInfo;                     // ���̺��� offset
            if (Physics.Raycast(transform.position + (Vector3.up * mHeight), dir, out hitInfo, _detectRange, _layerMask))
            {
                isInDirectFOV = hitInfo.collider.CompareTag(_tag);
                if (isInDirectFOV)
                {
                    _collPos = hitInfo.point;
                    break;
                }
            }

            // 20221203 ��켮 : �Ʒ��ʿ� �ϳ�, ���ʿ� �ϳ� ���� �ΰ� ��
            if (Physics.Raycast(transform.position + (Vector3.up * mHalfHeight), dir, out hitInfo, _detectRange, _layerMask))
            {
                isInDirectFOV = hitInfo.collider.CompareTag(_tag);
                if (isInDirectFOV)
                {
                    _collPos = hitInfo.point;
                    break;
                }
            }
        }
        return isInDirectFOV;
    }

    // �������� Ʈ������ ��ȯ�� �������̵�
    public bool IsInFovWithRayCheckDirect(float _detectRange, float _angle, string _tag, int _layerMask, ref Vector3 _collPos, ref Transform _flash)
    {
        bool isInDirectFOV = false;
        int stepCnt = 30;
        float stepAngleSize = _angle / stepCnt;

        for (int i = 0; i <= stepCnt; ++i)
        {
            float rayAngle = ((_angle / 2) + (stepAngleSize * i)) - _angle;
            Vector3 dir = DirFromAngle(rayAngle);

            Debug.DrawLine(transform.position + (Vector3.up * mHeight), transform.position + (Vector3.up * mHeight) + (dir * _detectRange), Color.green);
            Debug.DrawLine(transform.position + (Vector3.up * mHeight * 0.5f), transform.position + (Vector3.up * mHeight * 0.5f) + (dir * _detectRange), Color.green);

            RaycastHit hitInfo;                     // ���̺��� offset
            if (Physics.Raycast(transform.position + (Vector3.up * mHeight), dir, out hitInfo, _detectRange, _layerMask))
            {
                isInDirectFOV = hitInfo.collider.CompareTag(_tag);
                if (isInDirectFOV)
                {
                    _collPos = hitInfo.point;
                    _flash = hitInfo.transform.parent;
                    break;
                }
            }

            // 20221203 ��켮 : �Ʒ��ʿ� �ϳ�, ���ʿ� �ϳ� ���� �ΰ� ��
            if (Physics.Raycast(transform.position + (Vector3.up * mHalfHeight), dir, out hitInfo, _detectRange, _layerMask))
            {
                isInDirectFOV = hitInfo.collider.CompareTag(_tag);
                if (isInDirectFOV)
                {
                    _collPos = hitInfo.point;
                    _flash = hitInfo.transform.parent;
                    break;
                }
            }
        }
        return isInDirectFOV;
    }

    public Vector3 DirFromAngle(float _angleDegree)
    {
        _angleDegree += transform.eulerAngles.y;
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad),
                            0f,
                            Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad)).normalized;
    }

    // ������ ��ġ�� �Է¹����� �� ���� �������� �׸�ŭ ȸ���� ������ ���⺤�͸� ��ȯ�Ѵ�. : ���غ���
    private Vector3 DirFromAngle(float _angleDegree, float _verticalAngleDegree)
    {

        _angleDegree += transform.eulerAngles.y;
        _verticalAngleDegree += transform.eulerAngles.x;

        // 20221108 ��켮 : 3���� ���� �Ϸ�.
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad),
                            Mathf.Tan(-_verticalAngleDegree * Mathf.Deg2Rad),
                            Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad)).normalized;
    }
}
