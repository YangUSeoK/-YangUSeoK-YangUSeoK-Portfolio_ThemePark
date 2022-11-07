using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VolumeLight : MonoBehaviour
{

    // Ray ����
    private Light m_Light = null;
    private float m_LightRange = 0f;

    // ���̸� �ɰ��� ����. Ŭ���� �߰� �ɰ���
    [SerializeField] private float m_MeshRate = 0.1f;


    // Mesh ��������. 20221106 ��켮 : ���߿� Ŭ���� �ɰ� ���� ����.
    // 20221107 ��켮 : �ɰ���. �޽� ����°Ŷ� ��Ÿ �������� �Լ�.
    private Mesh mLightMesh = null;
    private MeshCollider m_MeshCollider = null;


    private FOVMaker m_FovMaker = null;



    private void Awake()
    {
        m_Light = GetComponent<Light>();
        m_FovMaker = GetComponent<FOVMaker>();

        // ������Ƽ�� ��� �����ϴ°� �Ⱦ ������ ����.
        m_LightRange = m_Light.range;

        mLightMesh = new Mesh();
        mLightMesh.name = "LightMesh";
        GetComponentInChildren<MeshFilter>().mesh = mLightMesh;
        m_MeshCollider = GetComponentInChildren<MeshCollider>();
        m_MeshCollider.sharedMesh = mLightMesh;
    }

    private void LateUpdate()
    {
        BuildLightingMesh();
    }

    private void BuildLightingMesh()
    {
        // ���� * ���� �ݿø�
        int stepCnt = Mathf.RoundToInt(m_Light.spotAngle * m_MeshRate);

        // �ٽ� ���� ������ ������ �ɰ����� �ޱ��� ���� ����
        float stepAngleSize = m_Light.spotAngle / stepCnt;

        List<Vector3> lightPointList = new List<Vector3>();

        // ��ä�÷� ���̸� ���� ����ü�� ������ �����Ѵ�.
        for (int i = 0; i <= stepCnt; ++i)
        {
            float angle = transform.eulerAngles.y - (m_Light.spotAngle / 2) + (stepAngleSize * i);

            // ����׿� DrawLine
            Debug.DrawLine(transform.position, transform.position + (m_FovMaker.DirFromAngle(angle, transform.eulerAngles.x, true) * m_Light.range), Color.green);


            lightPointList.Add(m_FovMaker.SetRaycastInfo(angle, m_LightRange, true).pointPos);
        }

        // ���� ���̽��
        //for(int i = 0; i <= stepCnt; ++i)
        //{
        //    float angle = transform.eulerAngles.x - (m_Light.spotAngle / 2) + (stepAngleSize * i) + 90;
        //    lightPointList.Add(m_FovMaker.SetRaycastInfo(angle, m_LightRange,false).pointPos);
        //}

        // 20221106 ��켮 : �޽� ����� �Լ��� ���� �ɰ� ��
        // +1 ���ִ� ���� : �� ó�� ������ ������ �߰������ �Ѵ�.
        int vertexCnt = lightPointList.Count + 1;
        Vector3[] vertices = new Vector3[vertexCnt];



        // �ﰢ�� ���� : vertexCnt-2
        // �ﰢ�� �������� �� : �ﰢ�� ���� * 3
        int[] triangles = new int[(vertexCnt - 2) * 3];

        // ����Ƽ �޽ô� ������ǥ�� �����̴�.
        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCnt - 1; ++i)
        {
            // InverseTransformPoint : ������ǥ ��ġ �޾ƿ���.
            vertices[i + 1] = transform.InverseTransformPoint(lightPointList[i]);

            // �迭�������� ����
            if (i < vertexCnt - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        mLightMesh.Clear();
        mLightMesh.vertices = vertices;
        mLightMesh.triangles = triangles;
        mLightMesh.RecalculateNormals();


        // �ݶ��̴��� aabb tree�� ����ϱ� ������, �޽��� ����Ǹ� �ٽ� �������־�� �Ѵ�.
        m_MeshCollider.enabled = false;
        m_MeshCollider.enabled = true;
    }




}
