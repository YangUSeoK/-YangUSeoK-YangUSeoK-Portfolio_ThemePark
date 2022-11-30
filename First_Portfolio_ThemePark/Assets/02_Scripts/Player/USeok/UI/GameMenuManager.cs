using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menu = null;
    [SerializeField] private InputActionProperty showButton;
    [SerializeField] private Transform head = null;
    private float spawnDistance = 3f;

    private void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            menu.SetActive(!menu.activeSelf); 
        }
        menu.transform.position = head.position + (new Vector3(head.forward.x, 0f, head.forward.z).normalized * spawnDistance);
        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1;
    }
}
