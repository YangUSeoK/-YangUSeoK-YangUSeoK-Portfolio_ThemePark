using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgm;
    
    void Awake()
    {
        bgm.Play();   
    }

 
    





















    public void SetPatrolBGM()
    {
        // ���� ���
    }

    public void SetTracePlayerBGM()
    {
        // if(TracePlayer ����� �ƴ϶��)
        // TracePlayer ���·� ��� �ٲ�
    }

    public void FadeOutTracePlayerBGM()
    {
        // TracePlayer���� ������ ���� ���̵�ƿ�
    }

    // ���ӿ��� �� ȣ���
    public void IsGameOver()
    {
        bgm.Stop();

        // ���ӿ��� ��� ���
    }

}
