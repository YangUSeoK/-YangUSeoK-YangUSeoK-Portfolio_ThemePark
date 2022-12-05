using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class PlayerHeight : MonoBehaviour
{
    private XROrigin m_XROrigin;
    private CharacterController m_CharacterController;
    private CharacterControllerDriver m_Driver;

    private void Awake()
    {
        m_XROrigin = GetComponent<XROrigin>();
        m_CharacterController = GetComponent<CharacterController>();
        m_Driver = GetComponent<CharacterControllerDriver>();
    }

    private void Update()
    {
        UpdateCharacterController();
    }

    private void UpdateCharacterController()
    {
        if (m_XROrigin == null || m_CharacterController == null)
            return;

        var height = Mathf.Clamp(m_XROrigin.CameraInOriginSpaceHeight, m_Driver.minHeight, m_Driver.maxHeight);

        Vector3 center = m_XROrigin.CameraInOriginSpacePos;
        center.y = height / 2f + m_CharacterController.skinWidth;

        m_CharacterController.height = height;
        m_CharacterController.center = center;
    }
}
