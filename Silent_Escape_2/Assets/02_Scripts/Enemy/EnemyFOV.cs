using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 원뿔 범위 앞에 플레이어가 걸렸는지 확인하는 스크립트
public class EnemyFOV : MonoBehaviour
{

    public float viewAngle = 120f;
    public float viewRange = 0f;


    private Transform m_LightTr = null;
    private int playerLayer = 0;
    private int obstacleLayer = 0;
    private int lightLayer = 0;
    private int layerMask = 0;

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

        if (colls.Length == 1)
        {
            Vector3 dir = (_targetTr.position - transform.position).normalized;

            //                                          -60 ~ 60 해서 120도
            if (Vector3.Angle(transform.forward, dir) < viewAngle * 0.5f)
            {
                Debug.Log($"원뿔 안에 {_targetTr.name} 검출됨");
                isTargetInFOV = true;
            }
        }
        return isTargetInFOV;
    }


    // 타겟에 레이쏴서 장애물 있는지 확인
    public bool IsLookTarget(float _detectRange, Transform _targetTr)
    {
        bool isLook = false;

        RaycastHit hitInfo;
        Vector3 dir = (_targetTr.position - transform.position).normalized;

        Debug.Log("레이 쏘는중");

        if (Physics.Raycast(transform.position + transform.forward + transform.up, dir, out hitInfo, _detectRange, layerMask))
        {
            Debug.Log("레이 맞았음!");
            isLook = hitInfo.collider.tag == _targetTr.tag;
        }

        return isLook;
    }

    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

}
