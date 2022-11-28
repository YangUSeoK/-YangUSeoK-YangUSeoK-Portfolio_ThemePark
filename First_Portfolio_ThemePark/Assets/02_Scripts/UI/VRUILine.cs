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
    float hitInfoDistance = 10f;

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
        DrawLineForward(Hand);
        if (Physics.Raycast(Hand.position, Hand.rotation * Vector3.forward, out hitInfo,hitInfoDistance))
        {
            if (hitInfo.transform.gameObject.CompareTag("BUTTON"))//
            {
                MousePointer.gameObject.SetActive(true);
                Button btn = hitInfo.transform.GetComponent<Button>();
                MousePointer.position = btn.transform.position;
                if (OVRInput.GetDown(OVRInput.Button.Any) && btn != null)
                {
                    btn.onClick.Invoke();
                }
            }
            else
            {
                MousePointer.gameObject.SetActive(false);
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