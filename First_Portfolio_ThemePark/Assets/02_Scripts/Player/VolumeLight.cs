using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VolumeLight : MonoBehaviour
{
    // 레이를 쪼개는 비율. 클수록 잘게 쪼갠다.
    [SerializeField] private int m_SubDivision = 100;

    // 20221115 양우석 : 벽에 부딪힌 점의 위치값 저장
    private List<Vector3> m_WallPosList = null;
    public List<Vector3> WallPosList
    {
        get { return m_WallPosList; }
    }
    private Light m_Light = null;

    // Mesh 생성관련 변수
    private Mesh mLightMesh = null;
    private MeshCollider m_MeshCollider = null;
    private List<Vector3> m_LightVertList = null;
    private List<Vector3> m_SmallCircleVertList = null;

    private void Awake()
    {
        mLightMesh = new Mesh();
        m_Light = GetComponent<Light>();
        mLightMesh.name = "LightMesh";
        GetComponentInChildren<MeshFilter>().mesh = mLightMesh; 
        m_MeshCollider = GetComponentInChildren<MeshCollider>();
        m_MeshCollider.sharedMesh = mLightMesh;

        // 점의 개수는 삼각형 개수의 3배. 한번 쪼갤 때 마다 삼각형이 1개씩 나온다. 원을 2개 그릴거라서 x2
        m_LightVertList = new List<Vector3>(3 * m_SubDivision * 2); 
        m_SmallCircleVertList = new List<Vector3>(3 * m_SubDivision);
        m_WallPosList = new List<Vector3>(3 * m_SubDivision * 2); // 벽에 부딪히는 점은 최대 정점의 개수와 같다.
    }

    private void LateUpdate()
    {
        DrawCone();
        BuildMesh(m_LightVertList);
    }

    private void DrawCone()
    {
        float n = 1f / (m_SubDivision); // 나눗셈은 계산이 느리기 때문에, 한번 계산해서 변수에 담아둔다.
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
            float ratio = ((float)i) * n;            // 비율. 한번 반복할 때 마다 n만큼 증가한다.
            float theta = (Mathf.PI * 2f) * ratio;   // 2PI x 비율 = 각도(라디안)
            float x = Mathf.Cos(theta) * radius;
            float y = Mathf.Sin(theta) * radius;

            origCircleVert = new Vector3(x, y, 1); //>> (0,0,1)을 중심으로 하고 XY 평면에 수평인 원
            origSmallCircleVert = new Vector3(x * 0.5f, y * 0.5f, 1);

            // 회전 행렬을 이용해 점을 회전시킨다.
            Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            Matrix4x4 rotMatrix = Matrix4x4.Rotate(rotation);
            newCircleVert = rotMatrix.MultiplyPoint3x4(origCircleVert);
            newSmallCircleVert = rotMatrix.MultiplyPoint3x4(origSmallCircleVert);

            // 회전한 벡터를 향해 length 길이만큼 레이를 쏜다.
            // 장애물에 부딪히면 그 점의 좌표, 아니라면 Ray의 목표지점 좌표를 정점리스트에 저장한다.
            Vector3 raycastPoint = SetRaycastPoint(newCircleVert, length);
            m_LightVertList.Add(raycastPoint);
            Vector3 raycastSmallPoint = SetRaycastPoint(newSmallCircleVert, smallLength);
            m_SmallCircleVertList.Add(raycastSmallPoint);
        }

        // 큰 원과 작은원을 리스트로 관리하기 위한 추가작업
        m_LightVertList.Add(m_LightVertList[0]);     
        m_LightVertList.Add(transform.position);    
        m_LightVertList.AddRange(m_SmallCircleVertList); 
        m_LightVertList.Add(m_SmallCircleVertList[0]);  
    }


    // Vertex 를 가지고 Mesh를 만든다.
    private void BuildMesh(List<Vector3> _lightVertList)
    {
        // 처음 시작할 원점이 있어야 해서 +1
        int vertexCnt = _lightVertList.Count + 1;
        Vector3[] vertices = new Vector3[vertexCnt];


        // 삼각형 개수 : vertexCnt-2
        // 삼각형 꼭지점의 수 : 삼각형 개수 * 3
        int[] triangles = new int[(vertexCnt - 2) * 3];

        // 유니티 메시는 로컬좌표가 기준이다.
        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCnt - 1; ++i)
        {
            // InverseTransformPoint : 로컬좌표 위치 받아오기
            vertices[i + 1] = transform.InverseTransformPoint(_lightVertList[i]);

            // 배열 맵핑
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
        mLightMesh.RecalculateNormals();    // 노말 달아주기

        // 콜라이더는 메쉬가 변경되면 다시 빌드해주어야 한다.
        m_MeshCollider.enabled = false;
        m_MeshCollider.enabled = true;
    }


    // 20221109 : 원형에 맞게 함수 수정. 방향벡터와 길이를 받아와서 레이를 쏘고,
    // 충돌정보를 RaycastInfo 구조체에 저장한다.
    // 20221113 : 생각해보니까 isHit을 쓸 필요가 없어서 구조체 사용안함.

    // 방향벡터와 길이를 받아와서 레이를 쏘고 point를 반환한다.
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
