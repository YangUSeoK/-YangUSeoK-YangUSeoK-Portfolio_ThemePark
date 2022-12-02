using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] AudioSource[] playerAudio;
    float h;
    float v;
    bool iswalking = false;

    void Start()
    {
        playerAudio = GetComponents<AudioSource>();
    }

    void Update()
    {
        Move();
        StepSound();
        StopSound();
    }

    void Move()
    {
         h = Input.GetAxis("Horizontal");
         v = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(h, 0f, v).normalized;

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    void StepSound()
    {
        if (Input.anyKeyDown && iswalking == false)
        {
            StartCoroutine(SoundPlay());
            iswalking = true;
        }
    }

    void StopSound()
    {
        if (Input.anyKey == false && iswalking == true)
        {
            StopAllCoroutines();
            iswalking = false;
        }
    }

    IEnumerator SoundPlay()
    {
        while(true)
        {
            playerAudio[0].Play();
            yield return new WaitForSeconds(playerAudio[0].clip.length);
            playerAudio[1].Play();
            yield return new WaitForSeconds(playerAudio[1].clip.length);
        }
    }


}
