using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateGrabRay : MonoBehaviour
{
    [SerializeField] private GameObject leftGrabRay = null;
    [SerializeField] private GameObject rightGrabRay = null;

    [SerializeField] private XRDirectInteractor leftDirectGrab = null;
    [SerializeField] private XRDirectInteractor rightDirectGrab = null;

    [SerializeField] private GameObject leftGrabbedObject = null;
    [SerializeField] private GameObject rightGrabbedObject = null;

    private void Update()
    {
        leftGrabRay.SetActive(leftDirectGrab.interactablesSelected.Count == 0);
        rightGrabRay.SetActive(rightDirectGrab.interactablesSelected.Count == 0);

        // �޼տ� ���� ������ �ٲ�� �� ������Ʈ�� �޼հ��ӿ�����Ʈ�� ����(�κ��丮��)
        if (leftDirectGrab.interactablesSelected.Count > 0)
        {
            if (leftGrabbedObject != leftDirectGrab.interactablesSelected[0].transform.gameObject)
            {
                leftGrabbedObject = leftDirectGrab.interactablesSelected[0].transform.gameObject;
            }
        }
        else
        {
            leftGrabbedObject = null;
        }

        // ������
        if (rightDirectGrab.interactablesSelected.Count > 0)
        {
            if (rightGrabbedObject != rightDirectGrab.interactablesSelected[0].transform.gameObject)
            {
                rightGrabbedObject = rightDirectGrab.interactablesSelected[0].transform.gameObject;
            }
        }
        else
        {
            rightGrabbedObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("INVEN"))
        {
            //other.GetComponent<ItemSlot>().
        }
    }
    public void PopItem()
    {

    }

    public void PushItem()
    {

    }
}
