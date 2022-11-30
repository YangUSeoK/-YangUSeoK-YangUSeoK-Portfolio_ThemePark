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

    private void Update()
    {
        leftGrabRay.SetActive(leftDirectGrab.interactablesSelected.Count == 0);
        rightGrabRay.SetActive(rightDirectGrab.interactablesSelected.Count == 0);
    }

}
