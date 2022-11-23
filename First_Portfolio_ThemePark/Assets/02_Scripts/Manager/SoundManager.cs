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

    
    void Update()
    {
        
    }
}
