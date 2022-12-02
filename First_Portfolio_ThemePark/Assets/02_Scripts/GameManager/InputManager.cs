using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    private static InputManager instance = null;
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private InputManager() { }

    private InputDevice m_LeftController;
    public InputDevice LeftController
    {
        get { return m_LeftController; }
    }
    private InputDevice m_RightController;
    public InputDevice RightController
    {
        get { return m_RightController; }
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Invoke("GetHandDevice", 1f);
    }

    public void GetHandDevice()
    {
        Debug.Log("��ġ ���� ����");

        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            m_LeftController = devices[0];
            Debug.Log("�޼� ��Ʈ�ѷ� ��� �Ϸ�");
        }

        devices.Clear();
        InputDevices.GetDevices(devices);
        InputDeviceCharacteristics RightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(RightControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            m_RightController = devices[0];
            Debug.Log("������ ��Ʈ�ѷ� ��� �Ϸ�");
        }
    }
}
