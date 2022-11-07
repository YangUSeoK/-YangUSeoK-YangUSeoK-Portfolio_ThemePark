using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// ���� ���� �տ� �÷��̾ �ɷȴ��� Ȯ���ϴ� ��ũ��Ʈ
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


    // ��������, �����ҳ� transform, ���̾� LayerMask.NameToLayer(" ")
    public bool IsInFOV(float _detectRange, Transform _targetTr, int _layer)
    {
        bool isTargetInFOV = false;

        Collider[] colls = Physics.OverlapSphere(transform.position, _detectRange, 1 << _layer);
        viewRange = _detectRange;

        if (colls.Length == 1)
        {
            Vector3 dir = (_targetTr.position - transform.position).normalized;

            //                                          -60 ~ 60 �ؼ� 120��
            if (Vector3.Angle(transform.forward, dir) < viewAngle * 0.5f)
            {
                Debug.Log($"���� �ȿ� {_targetTr.name} �����");
                isTargetInFOV = true;
            }
        }
        return isTargetInFOV;
    }


    // Ÿ�ٿ� ���̽��� ��ֹ� �ִ��� Ȯ��
    public bool IsLookTarget(float _detectRange, Transform _targetTr)
    {
        bool isLook = false;

        RaycastHit hitInfo;
        Vector3 dir = (_targetTr.position - transform.position).normalized;

        Debug.Log("���� �����");

        if (Physics.Raycast(transform.position + transform.forward + transform.up, dir, out hitInfo, _detectRange, layerMask))
        {
            Debug.Log("���� �¾���!");
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
