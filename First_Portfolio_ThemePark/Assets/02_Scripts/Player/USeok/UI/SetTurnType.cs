using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetTurnType : MonoBehaviour
{
    [SerializeField] private ActionBasedSnapTurnProvider snapTurn = null;
    [SerializeField] private ActionBasedContinuousTurnProvider continuousTurn = null;

    public void SetTypeFromIndex(int _index)
    {
        if(_index == 0)
        {
            snapTurn.enabled = false;
            continuousTurn.enabled = true;
        }
        else if(_index == 1)
        {
            continuousTurn.enabled = false;
            snapTurn.enabled = true;
        }
    }
}
