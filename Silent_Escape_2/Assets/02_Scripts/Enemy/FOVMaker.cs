using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVMaker : MonoBehaviour
{
    // ray�� ������ �����ϴ� ����ü
    public struct RaycastInfo
    {
        public bool isHit;
        public Vector3 pointPos;
        public float distance;
        public float angle;
        public Collider collider;

        // ������
        public RaycastInfo(bool _isHit, Vector3 _pointPos, float _distance, float _angle, Collider _collider)
        {
            isHit = _isHit;
            pointPos = _pointPos;
            distance = _distance;
            angle = _angle;
            collider = _collider;
        }
    }

    // tarnsform�� Euler.y ���� ���ڷ� �޾ƿͼ�, �� �������� Ray�� ���� hit ������ ����ü�� �����Ѵ�.
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

    // ���� ������ ��ġ�� �Է¹����� �� ���� �������� �׸�ŭ ȸ���� ������ ���⺤�͸� ��ȯ�Ѵ�.
    public Vector3 DirFromAngle(float _angleDegree, float _verticalAngleDegree, bool _angleIsGlobal)
    {
        if (!_angleIsGlobal)
        {
            _angleDegree += transform.eulerAngles.y;
            _verticalAngleDegree += transform.eulerAngles.x;
        }

        // �� ��ǥ (����, ���� �������� �̻���)
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad),
            2f * Mathf.Sin(-_verticalAngleDegree * Mathf.Deg2Rad),
            Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad));

        // 20221107 ��켮
        // �� ��ǥ(������ 0�� ��� ���̰� �ϳ��� �߻�Ǽ� �޽��� ������ �� ����)
        // x = cos(����)sin(����)     y = sin(����)sin(����)      z = cos(����)
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
