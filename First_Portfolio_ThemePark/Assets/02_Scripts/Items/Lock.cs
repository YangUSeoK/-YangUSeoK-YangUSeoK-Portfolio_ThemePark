using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Lock : MonoBehaviour
{
    [SerializeField] Light m_DirectionalLight = null;
    [SerializeField] float m_AlertTime = 2f;
    [SerializeField] float m_CallZombieTime = 5f;
    [SerializeField] float m_UnlockTime = 10f;

    private AudioSource m_Audio = null;

    private float mUnlockTimer = 0f;
    private bool mbIsAlert = false;
    private bool mbIsCallZombie = false;
    private bool mbIsUnlock = false;

    private Vector3 prePos = Vector3.zero;

    private void Awake()
    {
        m_Audio = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Key")
        {
            if (prePos != other.transform.position)
            {
                mUnlockTimer += Time.deltaTime;
                Debug.Log($"�ڹ��� Ÿ�̸� : {mUnlockTimer}");
                if (!mbIsAlert && mUnlockTimer >= m_AlertTime)
                {
                    // �Ҹ� �︮��, ������ ��½��½
                    Debug.Log("�ڹ��� : ���");
                    m_Audio.Play();
                    StartCoroutine(RedLightCoroutine());
                    mbIsAlert = true;
                }

                if (!mbIsCallZombie && mUnlockTimer >= m_CallZombieTime)
                {
                    // ���� ��ȯ
                    Debug.Log("�ڹ��� : ���� ��ȯ");
                    mbIsCallZombie = true;
                }

                if (!mbIsUnlock && mUnlockTimer >= 8)
                {
                    Debug.Log("Ż��");
                }
            }
            prePos = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        mUnlockTimer = 0f;
    }

    private IEnumerator RedLightCoroutine()
    {
        m_DirectionalLight.color = Color.red;
        
        float minIntensity = 0.2f;
        float maxIntensity = 0.6f;
        int sign = 1;

        while (true)
        {
            if (m_DirectionalLight.intensity <= minIntensity)
            {
                sign = 1;
            }
            else if (m_DirectionalLight.intensity >= maxIntensity)
            {
                sign = -1;
            }
            m_DirectionalLight.intensity += (0.005f * sign);

            yield return null;
        }
    }
}
