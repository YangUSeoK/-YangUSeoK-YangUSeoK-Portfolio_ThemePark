using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDog : MonoBehaviour
{
    Animator animator;
    AudioSource audio;
    public bool IsBark = false;
    float mTimer = 0f;
    void Start()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 소리를 10초동안 짖는다.
        if (IsBark == true)
        {
            mTimer += Time.deltaTime;
            while (mTimer < 1f)
            {
                float delay = Random.Range(0, 3);

                animator.SetFloat("RandomBark", delay);
                if (delay != 0f)
                {
                    audio.Play();
                }
            }
            mTimer = 0f;
        }
    }
}
