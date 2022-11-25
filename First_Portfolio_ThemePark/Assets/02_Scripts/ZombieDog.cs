using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDog : MonoBehaviour
{
    Animator animator;
    public bool IsBark = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(BarkDelay());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            StopCoroutine(BarkDelay());
        }
    }

    IEnumerator BarkDelay()
    {
        animator.SetTrigger("IsBark");

        while (true)
        {
            float delay = Random.Range(0, 3);

            animator.SetFloat("RandomBark", delay);
            yield return new WaitForSeconds(1f);
        }
    }
}
