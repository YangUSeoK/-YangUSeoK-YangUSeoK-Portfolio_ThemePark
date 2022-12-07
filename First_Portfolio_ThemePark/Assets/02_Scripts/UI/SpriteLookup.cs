using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLookup : MonoBehaviour
{
    [SerializeField] private Transform m_MinimapCam = null;
    private void Update()
    {
        transform.LookAt(m_MinimapCam.position);
    }
}
