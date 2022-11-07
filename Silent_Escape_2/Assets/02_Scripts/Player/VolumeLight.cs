using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VolumeLight : MonoBehaviour
{

    // Ray 관련
    private Light m_Light = null;
    private float m_LightRange = 0f;

    // 레이를 쪼개는 비율. 클수록 잘게 쪼갠다
    [SerializeField] private float m_MeshRate = 0.1f;


    // Mesh 생성관련. 20221106 양우석 : 나중에 클래스 쪼갤 수도 있음.
    // 20221107 양우석 : 쪼갰음. 메쉬 만드는거랑 기타 각도얻기용 함수.
    private Mesh mLightMesh = null;
    private MeshCollider m_MeshCollider = null;


    private FOVMaker m_FovMaker = null;



    private void Awake()
    {
        m_Light = GetComponent<Light>();
        m_FovMaker = GetComponent<FOVMaker>();

        // 프로퍼티에 계속 접근하는거 싫어서 변수에 저장.
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
        // 각도 * 비율 반올림
        int stepCnt = Mathf.RoundToInt(m_Light.spotAngle * m_MeshRate);

        // 다시 빛의 각도로 나눠서 쪼개지는 앵글의 각을 구함
        float stepAngleSize = m_Light.spotAngle / stepCnt;

        List<Vector3> lightPointList = new List<Vector3>();

        // 부채꼴로 레이를 쏴서 구조체에 정보를 저장한다.
        for (int i = 0; i <= stepCnt; ++i)
        {
            float angle = transform.eulerAngles.y - (m_Light.spotAngle / 2) + (stepAngleSize * i);

            // 디버그용 DrawLine
            Debug.DrawLine(transform.position, transform.position + (m_FovMaker.DirFromAngle(angle, transform.eulerAngles.x, true) * m_Light.range), Color.green);


            lightPointList.Add(m_FovMaker.SetRaycastInfo(angle, m_LightRange, true).pointPos);
        }

        // 수직 레이쏘기
        //for(int i = 0; i <= stepCnt; ++i)
        //{
        //    float angle = transform.eulerAngles.x - (m_Light.spotAngle / 2) + (stepAngleSize * i) + 90;
        //    lightPointList.Add(m_FovMaker.SetRaycastInfo(angle, m_LightRange,false).pointPos);
        //}

        // 20221106 양우석 : 메쉬 만드는 함수는 따로 쪼갤 것
        // +1 해주는 이유 : 맨 처음 시작점 개수를 추가해줘야 한다.
        int vertexCnt = lightPointList.Count + 1;
        Vector3[] vertices = new Vector3[vertexCnt];



        // 삼각형 개수 : vertexCnt-2
        // 삼각형 꼭지점의 수 : 삼각형 개수 * 3
        int[] triangles = new int[(vertexCnt - 2) * 3];

        // 유니티 메시는 로컬좌표가 기준이다.
        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCnt - 1; ++i)
        {
            // InverseTransformPoint : 로컬좌표 위치 받아오기.
            vertices[i + 1] = transform.InverseTransformPoint(lightPointList[i]);

            // 배열맵핑으로 수정
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


        // 콜라이더는 aabb tree를 사용하기 때문에, 메쉬가 변경되면 다시 빌드해주어야 한다.
        m_MeshCollider.enabled = false;
        m_MeshCollider.enabled = true;
    }




}
