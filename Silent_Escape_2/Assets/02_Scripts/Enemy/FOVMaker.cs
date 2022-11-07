using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVMaker : MonoBehaviour
{
    // ray의 정보를 저장하는 구조체
    public struct RaycastInfo
    {
        public bool isHit;
        public Vector3 pointPos;
        public float distance;
        public float angle;
        public Collider collider;

        // 생성자
        public RaycastInfo(bool _isHit, Vector3 _pointPos, float _distance, float _angle, Collider _collider)
        {
            isHit = _isHit;
            pointPos = _pointPos;
            distance = _distance;
            angle = _angle;
            collider = _collider;
        }
    }

    // tarnsform의 Euler.y 값을 인자로 받아와서, 그 방향으로 Ray를 쏴서 hit 정보를 구조체에 저장한다.
    public RaycastInfo SetRaycastInfo(float _angleDegree, float _distance, bool _pivotY)
    {
        Vector3 dir;
        if (_pivotY)
        {
            dir = DirFromAngle(_angleDegree, transform.eulerAngles.x, true);
        }
        else
        {
            dir = DirFromAngleByVertical(_angleDegree);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, _distance))
        {
            return new RaycastInfo(true, hit.point, hit.distance, _angleDegree, hit.collider);
        }
        else
        {
            return new RaycastInfo(false, transform.position + (dir * _distance), _distance, _angleDegree, hit.collider);
        }
    }

    public RaycastInfo GetZeroPos()
    {
        return new RaycastInfo(false, transform.position, 0f, 0f, null);
    }

    // 수평 각도를 수치로 입력받으면 내 현재 각도에서 그만큼 회전한 방향의 방향벡터를 반환한다.
    public Vector3 DirFromAngle(float _angleDegree, float _verticalAngleDegree, bool _angleIsGlobal)
    {
        if (!_angleIsGlobal)
        {
            _angleDegree += transform.eulerAngles.y;
            _verticalAngleDegree += transform.eulerAngles.x;
        }

        // 원 좌표 (수평, 세로 각도조절 이상함)
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad),
            2f * Mathf.Sin(-_verticalAngleDegree * Mathf.Deg2Rad),
            Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad));

        // 20221107 양우석
        // 구 좌표(각도가 0일 경우 레이가 하나만 발사되서 메쉬를 생성할 수 없음)
        // x = cos(가로)sin(세로)     y = sin(가로)sin(세로)      z = cos(세로)
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad) * Mathf.Sin((-_verticalAngleDegree) * Mathf.Deg2Rad),
            Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad) * Mathf.Sin((-_verticalAngleDegree) * Mathf.Deg2Rad),
            Mathf.Cos((-_verticalAngleDegree) * Mathf.Deg2Rad));
    }


    public Vector3 DirFromAngleByVertical(float _verticalAngleDegree)
    {
        _verticalAngleDegree -= transform.eulerAngles.x;

        return new Vector3(0f,
            Mathf.Sin((_verticalAngleDegree - 90f) * Mathf.Deg2Rad),
            Mathf.Cos((_verticalAngleDegree - 90f) * Mathf.Deg2Rad));
    }
}
