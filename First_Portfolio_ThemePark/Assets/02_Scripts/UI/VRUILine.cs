using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRUILine : MonoBehaviour
{
    LineRenderer lR;
    Vector3 startPos, endPos;
    public Transform Hand;
    public RectTransform MousePointer = null;
    Ray ray;
    RaycastHit hitInfo;
    float hitInfoDistance = 100f;

    private void Start()
    {
        lR = GetComponent<LineRenderer>();
        lR.startColor = Color.white;
        lR.endColor = Color.blue;
        lR.startWidth = 0.01f;
        lR.endWidth = 0.01f;
    }
    void Update()//2022 11 15 김준우
    {
        ray = new Ray(Hand.position, Hand.rotation * Vector3.forward);
        //Debug.DrawRay(Hand.position, Hand.rotation * Vector3.forward * hitInfoDistance, Color.green);
        DrawLineForward(Hand);//레이로 선 그리기
        if (Physics.Raycast(Hand.position, Hand.rotation * Vector3.forward, out hitInfo,hitInfoDistance))
        {
            Debug.Log("첫번째 조건(레이캐스트) 진입함");
            if (hitInfo.transform.gameObject.CompareTag("BUTTON"))//UI에 히트할 경우
            {
                Debug.Log("두번째 조건(레이어 조건 검사) 진입함");
                lR.gameObject.SetActive(true);
                MousePointer.gameObject.SetActive(true);
                Button btn = hitInfo.transform.GetComponent<Button>();
                MousePointer.position = btn.transform.position;
                if (OVRInput.GetDown(OVRInput.Button.Any) && btn != null)
                {
                    Debug.Log("세번째 조건(라인랜더러, 버튼 입력) 진입함");
                    Debug.Log("네번째 조건(버튼이 null이 아닐 때) 진입함");
                    btn.onClick.Invoke();
                }
            }
            else
            {
                Debug.Log("두번째 조건의 else 진입함");
                MousePointer.transform.gameObject.SetActive(false);
            }
        }
    }
    void DrawLineForward(Transform _hand)
    {
        startPos = _hand.position;
        endPos = _hand.position;
        endPos += ray.direction*hitInfoDistance;
        lR.SetPosition(0, startPos);
        lR.SetPosition(1, endPos);
    }
    IEnumerator DeleteUI()
    {
        yield return new WaitForSeconds(3f);
    }
}
