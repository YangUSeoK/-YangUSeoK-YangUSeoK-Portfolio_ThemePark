using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSound : MonoBehaviour
{
    [SerializeField] private float stepSound = 50f;
    private float stepTime = 0.1f;

    PlayerCtrl.PlayerIdle playerIdle;
    private void Update()
    {
        StartCoroutine(PlayerState());
    }
    IEnumerator PlayerState()
    {
        switch(playerIdle)
        {
            case PlayerCtrl.PlayerIdle.NonMove:
                stepTime = 0f;
                stepSound = 0f;
                FootStepSound();
                break;
            case PlayerCtrl.PlayerIdle.Walk:
                stepTime = 0.4f;
                stepSound = 20f;
                FootStepSound();
                yield return new WaitForSeconds(stepTime);
                break;
            case PlayerCtrl.PlayerIdle.SlowWalk:
                stepTime = 0.6f;
                stepSound = 10f;
                FootStepSound();
                yield return new WaitForSeconds(stepTime);
                break;
            case PlayerCtrl.PlayerIdle.Run:
                stepTime = 0.2f;
                stepSound = 50f;
                FootStepSound();
                yield return new WaitForSeconds(stepTime);
                break;
            case PlayerCtrl.PlayerIdle.Squat:
                stepTime = 1f;
                stepSound = 5f;
                FootStepSound();
                yield return new WaitForSeconds(stepTime);
                break;
        }
    }
    void FootStepSound()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, stepSound, 1 << LayerMask.NameToLayer("LISTENER"));
        for (int i = 0; i < colls.Length; ++i)
        {
            colls[i].gameObject.GetComponent<Enemy_Listener>().Listen(transform, transform.position);
        }
    }
}
