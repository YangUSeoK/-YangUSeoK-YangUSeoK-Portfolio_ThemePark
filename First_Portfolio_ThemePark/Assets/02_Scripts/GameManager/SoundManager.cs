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
        // ���� ���
        Debug.Log("PatrolBGM");
    }

    public void SetTracePlayerBGM()
    {
        // if(TracePlayer ����� �ƴ϶��)
        // TracePlayer ���·� ��� �ٲ�
        Debug.Log("SetTracePlayerBGM");
    }

    public void FadeOutTracePlayerBGM()
    {
        // TracePlayer���� ������ ���� ���̵�ƿ�
        Debug.Log("ExitTracePlayerBGM");
    }

    // ���ӿ��� �� ȣ���
    public void IsGameOver()
    {
        bgm.Stop();

        // ���ӿ��� ��� ���
        Debug.Log("GameOver");
    }

}
