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

 
    // ���ӿ��� �� ȣ���
    public void IsGameOver()
    {
        bgm.Stop();
    }
}
