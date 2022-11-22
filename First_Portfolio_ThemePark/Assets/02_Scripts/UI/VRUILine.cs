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
    void Update()//2022 11 15 ���ؿ�
    {
        ray = new Ray(Hand.position, Hand.rotation * Vector3.forward);
        //Debug.DrawRay(Hand.position, Hand.rotation * Vector3.forward * hitInfoDistance, Color.green);
        DrawLineForward(Hand);//���̷� �� �׸���
        if (Physics.Raycast(Hand.position, Hand.rotation * Vector3.forward, out hitInfo,hitInfoDistance))
        {
            Debug.Log("ù��° ����(����ĳ��Ʈ) ������");
            if (hitInfo.transform.gameObject.CompareTag("BUTTON"))//UI�� ��Ʈ�� ���
            {
                Debug.Log("�ι�° ����(���̾� ���� �˻�) ������");
                lR.gameObject.SetActive(true);
                MousePointer.gameObject.SetActive(true);
                Button btn = hitInfo.transform.GetComponent<Button>();
                MousePointer.position = btn.transform.position;
                if (OVRInput.GetDown(OVRInput.Button.Any) && btn != null)
                {
                    Debug.Log("����° ����(���η�����, ��ư �Է�) ������");
                    Debug.Log("�׹�° ����(��ư�� null�� �ƴ� ��) ������");
                    btn.onClick.Invoke();
                }
            }
            else
            {
                Debug.Log("�ι�° ������ else ������");
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
