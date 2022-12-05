using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VolumeLight : MonoBehaviour
{
    private Light m_Light = null;

    // 20221115 ��켮 : ���� ���� �ε��� ��ġ�� �˱����� ����
    private List<Vector3> m_WallPosList = null;
    public List<Vector3> WallPosList
    {
        get { return m_WallPosList; }
    }

    private List<Vector3> m_LightVertList = null;
    private List<Vector3> m_SmallCircleVertList = null;

    // ���̸� �ɰ��� ����. Ŭ���� �߰� �ɰ���
    [SerializeField] private int m_SubDivision = 100;

    // Mesh ��������. 20221106 ��켮 : ���߿� Ŭ���� �ɰ� ���� ����.  => �Լ��� �ɰ���
    private Mesh mLightMesh = null;
    private MeshCollider m_MeshCollider = null;

    private void Awake()
    {
        mLightMesh = new Mesh();
        m_Light = GetComponent<Light>();
        mLightMesh.name = "LightMesh";
        GetComponentInChildren<MeshFilter>().mesh = mLightMesh; // �����Ҵ��� �Ž��� �Ž����Ϳ� ���δ�.
        m_MeshCollider = GetComponentInChildren<MeshCollider>();
        m_MeshCollider.sharedMesh = mLightMesh; // �Ž��ݶ��̴��� ���� �Ž��� ���δ�.

        m_LightVertList = new List<Vector3>(3 * m_SubDivision * 2); // ���̸� �� ���� ������ ������ �����Ƿ�, �� ũ�⸸ŭ�� �����Ҵ��صд�.(����ȭ)
        m_SmallCircleVertList = new List<Vector3>(3 * m_SubDivision);
        m_WallPosList = new List<Vector3>(3 * m_SubDivision * 2);   // Vector3 = float x 3 => int * 3 * ���ؿ��� �� ����
    }

    private void LateUpdate()
    {
        DrawCone();
        BuildMesh(m_LightVertList);
    }

    private void DrawCone()
    {
        float n = 1f / (m_SubDivision); // �ݺ������� ���ϰŶ� �������� �̸� ����صд�. (����ȭ)
        float radius = Mathf.Tan((m_Light.spotAngle * 0.5f) * Mathf.Deg2Rad);
        float length = m_Light.range / Mathf.Cos(m_Light.spotAngle * 0.5f * Mathf.Deg2Rad);
        float smallLength = m_Light.range / Mathf.Cos(m_Light.spotAngle * 0.5f * 0.5f * Mathf.Deg2Rad);
        Vector3 origCircleVert = Vector3.zero;
        Vector3 origSmallCircleVert = Vector3.zero;
        Vector3 newCircleVert = Vector3.zero;
        Vector3 newSmallCircleVert = Vector3.zero;

        m_LightVertList.Clear();
        m_SmallCircleVertList.Clear();
        m_WallPosList.Clear();
        for (int i = 0; i < m_SubDivision; ++i)
        {
            float ratio = ((float)i) * n;            // ���� ������ ����
            float theta = (Mathf.PI * 2f) * ratio;   // ���� * ���� = ����
            float x = Mathf.Cos(theta) * radius;
            float y = Mathf.Sin(theta) * radius;

            origCircleVert = new Vector3(x, y, 1); //>> (0,0,1)�� �߽����� xy��鿡 ������ ���� ��ǥ
            origSmallCircleVert = new Vector3(x * 0.5f, y * 0.5f, 1);

            // ȸ������� ���� ȸ����Ų��.
            Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            Matrix4x4 rotMatrix = Matrix4x4.Rotate(rotation);

            newCircleVert = rotMatrix.MultiplyPoint3x4(origCircleVert);
            newSmallCircleVert = rotMatrix.MultiplyPoint3x4(origSmallCircleVert);

            //Debug.DrawLine(transform.position, newCircleVert, Color.green);
            //Debug.DrawLine(transform.position, CirclePointPos * length, Color.green);


            // ȸ���� ������ �� ��ġ�� ���� length ���̸�ŭ ���̸� ���. 
            Vector3 raycastPoint = SetRaycastPoint(newCircleVert, length);
            m_LightVertList.Add(raycastPoint);
            Vector3 raycastSmallPoint = SetRaycastPoint(newSmallCircleVert, smallLength);
            m_SmallCircleVertList.Add(raycastSmallPoint);
        }

        m_LightVertList.Add(m_LightVertList[0]);    // ���� ���� ����
        m_LightVertList.Add(transform.position);    // ���� ���� ������ = �÷��� ��ġ
        m_LightVertList.AddRange(m_SmallCircleVertList);    // ���� �� �߰�
        m_LightVertList.Add(m_SmallCircleVertList[0]);  // �������� ����
    }


    // Vertex ����Ʈ�� �޾Ƽ� �޽��� ������ش�.
    private void BuildMesh(List<Vector3> _lightVertList)
    {
        // +1 ���ִ� ���� : �� ó�� ������ ������ �߰������ �Ѵ�.
        int vertexCnt = _lightVertList.Count + 1;
        Vector3[] vertices = new Vector3[vertexCnt];


        // �ﰢ�� ���� : vertexCnt-2
        // �ﰢ�� �������� �� : �ﰢ�� ���� * 3
        int[] triangles = new int[(vertexCnt - 2) * 3];

        // ����Ƽ �޽ô� ������ǥ�� �����̴�.
        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCnt - 1; ++i)
        {
            // InverseTransformPoint : ������ǥ -> ������ǥ ��ġ �޾ƿ���.
            vertices[i + 1] = transform.InverseTransformPoint(_lightVertList[i]);

            // �迭����
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
        mLightMesh.RecalculateNormals();    // �븻 �޾��ֱ�

        // �ݶ��̴��� aabb tree�� ����ϱ� ������, �޽��� ����Ǹ� �ٽ� ������־�� �Ѵ�.
        m_MeshCollider.enabled = false;
        m_MeshCollider.enabled = true;
    }


    // 20221109 ��켮 : ������ �°� �Լ� ����. ���⺤�Ϳ� ���̸� �޾ƿͼ� ���̸� ���,
    // �浹������ RaycastInfo ����ü�� �����Ѵ�.
    // 20221113 ��켮 : �����غ��ϱ� isHit �� �� �ʿ䰡 ��� ������.
    // �� �ϳ����� ����ü �����Ҵ��ϰ� �������ݷ��� �����°� �����ٽ��

    // ���⺤�Ϳ� ���̸� �޾ƿͼ� ���̸� ��� point�� ��ȯ�Ѵ�.
    private Vector3 SetRaycastPoint(Vector3 _dir, float _length)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, _dir, out hitInfo, _length))
        {
            m_WallPosList.Add(hitInfo.point);
            return hitInfo.point;
        }
        else
        {
            return transform.position + (_dir * _length);
        }
    }

    // ������ ��ġ�� �Է¹����� �� ���� �������� �׸�ŭ ȸ���� ������ ���⺤�͸� ��ȯ�Ѵ�. : ���غ���
    private Vector3 DirFromDegree(float _angleDegree, float _verticalAngleDegree)
    {
        _angleDegree += transform.eulerAngles.y;
        _verticalAngleDegree += transform.eulerAngles.x;


        // 20221108 ��켮 : ���Ʒ����� �� ���� �� �ֵ��� ����
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad),
                            Mathf.Tan(-_verticalAngleDegree * Mathf.Deg2Rad),
                            Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad)).normalized;
    }

}
