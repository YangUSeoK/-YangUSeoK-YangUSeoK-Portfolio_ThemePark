using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



// 원뿔 범위 앞에 플레이어가 걸렸는지 확인하는 스크립트
public class EnemyFOV : MonoBehaviour
{
    // 에디터용
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


    // 감지범위, 감지할놈 transform, 레이어 LayerMask.NameToLayer(" ")
    public bool IsInFOV(float _detectRange, Transform _targetTr, int _layer)
    {
        bool isTargetInFOV = false;

        Collider[] colls = Physics.OverlapSphere(transform.position, _detectRange, 1 << _layer);
        viewRange = _detectRange;

        if (colls.Length >= 1)
        {
            Vector3 dir = (_targetTr.position - transform.position).normalized;

            //                                          -60 ~ 60 해서 120도
            if (Vector3.Angle(transform.forward, dir) < viewAngle * 0.5f)
            {
                Debug.Log($"원뿔 안에 {_targetTr.name} 들어옴");
                // 부채꼴로 레이 쏴서 검사하는 함수
                isTargetInFOV = IsLookTarget();
            }
        }
        return isTargetInFOV;
    }


    // 타겟에 레이쏴서 장애물 있는지 확인
    public bool IsLookTarget()
    {
        bool isLook = false;

        // 각도 * 비율 반올림
        int stepCnt = Mathf.RoundToInt(viewAngle * m_stepRate);

        // 다시 빛의 각도로 나눠서 쪼개지는 앵글의 각을 구함
        float stepAngleSize = viewAngle / stepCnt;

        List<Collider> rayPointList = new List<Collider>();


        Debug.Log("부채꼴 레이 발싸!");
        // 부채꼴로 레이를 쏴서 구조체에 정보를 저장한다.
        for (int i = 0; i <= stepCnt; ++i)
        {
            float angle = transform.eulerAngles.y - (viewAngle / 2) + (stepAngleSize * i);

            // 디버그용 DrawLine
            
            
            Vector3 dir = m_FovMaker.DirFromAngle(angle, transform.eulerAngles.x, true);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit, m_SightMan.LookPlayerRange))
            {
                if (hit.transform.CompareTag("PLAYER"))
                {
                    Debug.Log("플레이어");
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

        // y값(세로 회전) 구면좌표계로 벡터 구하는걸로 수정해야 함.
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad), 0f, Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad));
    }
}
