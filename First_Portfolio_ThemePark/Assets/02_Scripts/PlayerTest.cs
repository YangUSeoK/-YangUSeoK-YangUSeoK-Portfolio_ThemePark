using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] AudioSource[] playerAudio;

    void Start()
    {
        playerAudio = GetComponents<AudioSource>();
    }

    void Update()
    {
        Move();
        StepSound();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(h, 0f, v).normalized;

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    void StepSound()
    {
        Vector3 currPos = transform.position;
        if (transform.position != currPos)
        {
            StartCoroutine(SoundPlay());
        }
        else
        {
            StopCoroutine(SoundPlay());
        }
    }

    IEnumerator SoundPlay()
    {
        playerAudio[0].Play();
        yield return new WaitForSeconds(playerAudio[0].clip.length);
        playerAudio[1].Play();
        yield return new WaitForSeconds(playerAudio[1].clip.length);
    }
}
