using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    [SerializeField] private Transform leftAttachTransform;
    [SerializeField] private Transform rightAttachTransform;
    
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if(args.interactorObject.transform.CompareTag("LHAND"))
        {
            attachTransform = leftAttachTransform;
        }
        else if (args.interactorObject.transform.CompareTag("RHAND"))
        {
            attachTransform = rightAttachTransform;
        }

        base.OnSelectEntered(args);
    }
}
