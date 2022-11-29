using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    PlayerCtrl PC;

    private void Update()
    {
        if(PC.playerIdle==PlayerCtrl.PlayerIdle.Squat)
        {
            gameObject.GetComponent<RawImage>().enabled = false;
        }
    }
}
