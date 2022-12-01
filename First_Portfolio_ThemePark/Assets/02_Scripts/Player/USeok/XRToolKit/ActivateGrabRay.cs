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

        // 왼손에 잡은 물건이 바뀌면 그 오브젝트를 왼손게임오브젝트에 저장(인벤토리용)
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

        // 오른손
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
