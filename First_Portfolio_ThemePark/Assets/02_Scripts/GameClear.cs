using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PLAYER")
        {
            GameManager.Instance.GameClear();
        }
    }
}
