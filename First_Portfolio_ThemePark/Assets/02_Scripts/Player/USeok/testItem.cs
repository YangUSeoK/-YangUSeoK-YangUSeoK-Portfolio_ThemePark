using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class testItem : MonoBehaviour
{
    private InputDevice targetDevice;

    private void Update()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        //if (devices.Count > 0)
        //{
        //    Debug.Log(devices[0]);
        //    targetDevice = devices[0];
        //}
            
        //List<InputDevice> allDevices = new List<InputDevice>();
        //InputDevices.GetDevices(allDevices);


        //targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        //if (primaryButtonValue)
        //{
        //    Debug.Log("Pressing primary button");
        //}

        //targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        //if (triggerValue > 0.1f)
        //{
        //    Debug.Log("Trigger pressed" + triggerValue);
        //}

        //targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue);
        //if (primary2DAxisValue != Vector2.zero)
        //{
        //    Debug.Log("Primary Touchpad " + primary2DAxisValue);
        //}
    }

    public void IsGrab()
    {
        Debug.Log("is grab");
    }

    public void StayGrab()
    {
        Debug.Log("Stay Grab");
    }

    public void ExitGrab()
    {
        Debug.Log("Exit Grab");
    }

    public void EnterHover()
    {
        Debug.Log("Enter Hover");
    }

    public void StayHover()
    {
        Debug.Log("Ho~~Ver");
    }

    public void ExitHover()
    {
        Debug.Log("Exit Hover");
    }

    public void FirstEnterSelect()
    {
        Debug.Log("First Enter Select");
    }
    public void EnterSelect()
    {
        Debug.Log("Enter Select");
    }

    public void ExitSelect()
    {
        Debug.Log("Exit Select");
    }

    public void LastExitSelect()
    {
        Debug.Log("Last Exit Select");
    }

    public void EnterActivate()
    {
        Debug.Log("Enter Activate");
    }

    public void ExitActivate()
    {
        Debug.Log("Exit Activate");
    }

}
