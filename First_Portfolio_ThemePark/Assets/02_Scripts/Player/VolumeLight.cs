using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VolumeLight : MonoBehaviour
{
    private Light m_Light = null;

    // 20221115 양우석 : 좀비가 벽에 부딪힌 위치를 알기위해 만듬
    private List<Vector3> m_WallPosList = null;
    public List<Vector3> WallPosList
    {
        get { return m_WallPosList; }
    }

    private List<Vector3> m_LightVertList = null;
    private List<Vector3> m_SmallCircleVertList = null;

    // 레이를 쪼개는 비율. 클수록 잘게 쪼갠다
    [SerializeField] private int m_SubDivision = 100;

    // Mesh 생성관련. 20221106 양우석 : 나중에 클래스 쪼갤 수도 있음.  => 함수만 쪼갰음
    private Mesh mLightMesh = null;
    private MeshCollider m_MeshCollider = null;

    private void Awake()
    {
        mLightMesh = new Mesh();
        m_Light = GetComponent<Light>();
        mLightMesh.name = "LightMesh";
        GetComponentInChildren<MeshFilter>().mesh = mLightMesh; // 동적할당한 매쉬를 매쉬필터에 붙인다.
        m_MeshCollider = GetComponentInChildren<MeshCollider>();
        m_MeshCollider.sharedMesh = mLightMesh; // 매쉬콜라이더에 사용될 매쉬를 붙인다.

        m_LightVertList = new List<Vector3>(3 * m_SubDivision * 2); // 레이를 쏠 원의 개수는 정해져 있으므로, 그 크기만큼을 동적할당해둔다.(최적화)
        m_SmallCircleVertList = new List<Vector3>(3 * m_SubDivision);
        m_WallPosList = new List<Vector3>(3 * m_SubDivision * 2);   // Vector3 = float x 3 => int * 3 * 기준원의 점 개수
    }

    private void LateUpdate()
    {
        DrawCone();
        BuildMesh(m_LightVertList);
    }

    private void DrawCone()
    {
        float n = 1f / (m_SubDivision); // 반복문에서 쓰일거라서 나눗셈을 미리 계산해둔다. (최적화)
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
            float ratio = ((float)i) * n;            // 원을 비율로 나눔
            float theta = (Mathf.PI * 2f) * ratio;   // 원주 * 비율 = 각도
            float x = Mathf.Cos(theta) * radius;
            float y = Mathf.Sin(theta) * radius;

            origCircleVert = new Vector3(x, y, 1); //>> (0,0,1)을 중심으로 xy평면에 평행한 원의 좌표
            origSmallCircleVert = new Vector3(x * 0.5f, y * 0.5f, 1);

            // 회전행렬을 통해 회전시킨다.
            Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            Matrix4x4 rotMatrix = Matrix4x4.Rotate(rotation);

            newCircleVert = rotMatrix.MultiplyPoint3x4(origCircleVert);
            newSmallCircleVert = rotMatrix.MultiplyPoint3x4(origSmallCircleVert);

            //Debug.DrawLine(transform.position, newCircleVert, Color.green);
            //Debug.DrawLine(transform.position, CirclePointPos * length, Color.green);


            // 회전된 각각의 점 위치를 향해 length 길이만큼 레이를 쏜다. 
            Vector3 raycastPoint = SetRaycastPoint(newCircleVert, length);
            m_LightVertList.Add(raycastPoint);
            Vector3 raycastSmallPoint = SetRaycastPoint(newSmallCircleVert, smallLength);
            m_SmallCircleVertList.Add(raycastSmallPoint);
        }

        m_LightVertList.Add(m_LightVertList[0]);    // 원래 원의 끝점
        m_LightVertList.Add(transform.position);    // 작은 원의 시작점 = 플래시 위치
        m_LightVertList.AddRange(m_SmallCircleVertList);    // 작은 원 추가
        m_LightVertList.Add(m_SmallCircleVertList[0]);  // 작은원의 끝점
    }


    // Vertex 리스트를 받아서 메쉬를 만들어준다.
    private void BuildMesh(List<Vector3> _lightVertList)
    {
        // +1 해주는 이유 : 맨 처음 시작점 개수를 추가해줘야 한다.
        int vertexCnt = _lightVertList.Count + 1;
        Vector3[] vertices = new Vector3[vertexCnt];


        // 삼각형 개수 : vertexCnt-2
        // 삼각형 꼭지점의 수 : 삼각형 개수 * 3
        int[] triangles = new int[(vertexCnt - 2) * 3];

        // 유니티 메시는 로컬좌표가 기준이다.
        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCnt - 1; ++i)
        {
            // InverseTransformPoint : 월드좌표 -> 로컬좌표 위치 받아오기.
            vertices[i + 1] = transform.InverseTransformPoint(_lightVertList[i]);

            // 배열맵핑
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

        // 콜라이더는 aabb tree를 사용하기 때문에, 메쉬가 변경되면 다시 빌드해주어야 한다.
        m_MeshCollider.enabled = false;
        m_MeshCollider.enabled = true;
    }


    // 20221109 양우석 : 원형에 맞게 함수 수정. 방향벡터와 길이를 받아와서 레이를 쏘고,
    // 충돌정보를 RaycastInfo 구조체에 저장한다.
    // 20221113 양우석 : 생각해보니까 isHit 을 쓸 필요가 없어서 지웠음.
    // 점 하나마다 구조체 동적할당하고 가비지콜렉터 돌리는거 개에바쎄바

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

    // 각도를 수치로 입력받으면 내 현재 각도에서 그만큼 회전한 방향의 방향벡터를 반환한다. : 기준벡터
    private Vector3 DirFromDegree(float _angleDegree, float _verticalAngleDegree)
    {
        _angleDegree += transform.eulerAngles.y;
        _verticalAngleDegree += transform.eulerAngles.x;


        // 20221108 양우석 : 위아래까지 다 구할 수 있도록 수정
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad),
                            Mathf.Tan(-_verticalAngleDegree * Mathf.Deg2Rad),
                            Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad)).normalized;
    }

}
