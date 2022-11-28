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

 
    // 게임오버 시 호출됨
    public void IsGameOver()
    {
        bgm.Stop();
    }
}
