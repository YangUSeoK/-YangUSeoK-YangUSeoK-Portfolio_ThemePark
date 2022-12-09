using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// 원뿔 범위 앞에 플레이어가 걸렸는지 확인하는 스크립트
public class FOV : MonoBehaviour
{
    // 에디터용
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
        // Enemy의 포지션에서부터 viewRange 만큼 구체를 
        // 그려서 그 중에 playerLayer 가 있으면 colls 배열에 추가
        Collider[] colls = new Collider[1];
        Physics.OverlapSphereNonAlloc(transform.position, 100f /*_detectRange*/, colls, _layerMask);


        // 플레이어 레이어가 검출되었을 때
        if (colls[0] != null)
        {
            Vector3 dir = (colls[0].transform.position - transform.position).normalized;

            // 내가 본 전방방향에서 방금 구한 dir방향의 각도가 120도 범위 안에 있으면
            if (Vector3.Angle(transform.forward, dir) <= _angle * 0.5f)
            {
                isInFOV = true;
            }
            //Debug.Log($"플레이어랑 좀비사이 각도 :{transform.position} / {Vector3.Angle(transform.forward, dir)} / {isInFOV}");
        }
        return isInFOV;
    }

    public bool IsLookDirect(Transform _targetTr, float _detectRange, int _layerMask)
    {
        bool isLook = false;

        RaycastHit hitInfo;
        Vector3 dir = (_targetTr.position - transform.position).normalized;

        // 20221114 양우석 : 레이쏘는 위치 좀비에따라 보정해야 함
        // 20221202 양우석 : 직접보는 레이는 offset 없앴음
        if (Physics.Raycast(transform.position + (Vector3.up * mHalfHeight), dir, out hitInfo, _detectRange, m_LayerMask))
        {
            //Debug.Log($" FOV IsLookDirect / 밑에레이져 맞은놈 : {hitInfo.transform.name}");
            isLook = hitInfo.collider.CompareTag(_targetTr.tag);
        }

        // 20221203 양우석 : 아래쪽에 하나, 위쪽에 하나 레이 두개 쏨
        if (!isLook)
        {
            if (Physics.Raycast(transform.position + (Vector3.up * mHeight), dir, out hitInfo, _detectRange, m_LayerMask))
            {
                //Debug.Log($" FOV IsLookDirect / 위에레이져 맞은놈 : {hitInfo.transform.name}");
                isLook = hitInfo.collider.CompareTag(_targetTr.tag);
            }
        }

        if (isLook)
        {
            Debug.Log($"레이 쏜놈 : {transform.name}");
        }
        return isLook;
    }

    // 부채꼴로 레이를 쏴서 부딪히면 위치를 저장한다.
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


            RaycastHit hitInfo;                     // 높이보정 offset
            if (Physics.Raycast(transform.position + (Vector3.up * mHeight), dir, out hitInfo, _detectRange, _layerMask))
            {
                isInDirectFOV = hitInfo.collider.CompareTag(_tag);
                if (isInDirectFOV)
                {
                    _collPos = hitInfo.point;
                    break;
                }
            }

            // 20221203 양우석 : 아래쪽에 하나, 위쪽에 하나 레이 두개 쏨
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

    // 맞은놈의 트랜스폼 반환용 오버라이드
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

            RaycastHit hitInfo;                     // 높이보정 offset
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

            // 20221203 양우석 : 아래쪽에 하나, 위쪽에 하나 레이 두개 쏨
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

    // 각도를 수치로 입력받으면 내 현재 각도에서 그만큼 회전한 방향의 방향벡터를 반환한다. : 기준벡터
    private Vector3 DirFromAngle(float _angleDegree, float _verticalAngleDegree)
    {

        _angleDegree += transform.eulerAngles.y;
        _verticalAngleDegree += transform.eulerAngles.x;

        // 20221108 양우석 : 3차원 벡터 완료.
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad),
                            Mathf.Tan(-_verticalAngleDegree * Mathf.Deg2Rad),
                            Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad)).normalized;
    }
}
