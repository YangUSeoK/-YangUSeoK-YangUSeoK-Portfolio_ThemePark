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
    float hitInfoDistance = 1f;

    private void Start()
    {
        lR = GetComponent<LineRenderer>();
        lR.startColor = Color.white;
        lR.endColor = Color.blue;
        lR.startWidth = 0.01f;
        lR.endWidth = 0.01f;
        MousePointer.gameObject.SetActive(false);
    }
    void Update()//2022 11 15 김준우
    {
        ray = new Ray(Hand.position, Hand.rotation * Vector3.forward);
        //Debug.DrawRay(Hand.position, Hand.rotation * Vector3.forward * hitInfoDistance, Color.green);
        DrawLineForward(Hand);
        if (Physics.Raycast(Hand.position, Hand.rotation * Vector3.forward, out hitInfo,hitInfoDistance))
        {
            string tag = hitInfo.collider.tag;
            //Debug.Log($"hitInfo.{tag}");
            if (tag.Equals("BUTTON"))//hitInfo 콜라이더의 태그값으로 비교
            {
                lR.gameObject.SetActive(true);
                MousePointer.gameObject.SetActive(true);//마우스 포인터 활성화
                MousePointer.GetComponent<Image>().enabled = true;
                //Debug.Log($"MousePointer{MousePointer}");

                //Debug.DrawRay(Hand.position, Hand.rotation*Vector3.forward);
                //Button btn = hitInfo.GetComponent<Button>();
                Button btn = hitInfo.collider.GetComponent<Button>();
                //Debug.Log($"Btnpos:{btn.transform.position}");
                MousePointer.transform.position = btn.transform.position;
                if (OVRInput.GetDown(OVRInput.Button.Any) && btn != null)//버튼 실행
                {
                    btn.onClick.Invoke();
                }
            }
            else
            {
                MousePointer.GetComponent<Image>().enabled = false;
                if (MousePointer != null)
                {
                    Debug.Log("마우스포인터 인식 못함");
                }
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
}