using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgm;
    
    void Awake()
    {
        //bgm.Play();   
    }

 
    





















    public void SetPatrolBGM()
    {
        // 평상시 브금
        Debug.Log("PatrolBGM");
    }

    public void SetTracePlayerBGM()
    {
        // if(TracePlayer 브금이 아니라면)
        // TracePlayer 상태로 브금 바뀜
        Debug.Log("SetTracePlayerBGM");
    }

    public void FadeOutTracePlayerBGM()
    {
        // TracePlayer에서 나갈때 볼륨 페이드아웃
        Debug.Log("ExitTracePlayerBGM");
    }

    // 게임오버 시 호출됨
    public void IsGameOver()
    {
        bgm.Stop();

        // 게임오버 브금 출력
        Debug.Log("GameOver");
    }

}
