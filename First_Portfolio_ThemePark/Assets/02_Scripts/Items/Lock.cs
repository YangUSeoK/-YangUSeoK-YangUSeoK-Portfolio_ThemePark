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
                Debug.Log($"¿⁄π∞ºË ≈∏¿Ã∏” : {mUnlockTimer}");
                if (!mbIsAlert && mUnlockTimer >= m_AlertTime)
                {
                    // º“∏Æ øÔ∏Æ±‚, ª°∞≤∞‘ π¯¬Ωπ¯¬Ω
                    Debug.Log("¿⁄π∞ºË : ∞Ê∞Ì");
                    m_Audio.Play();
                    StartCoroutine(RedLightCoroutine());
                    mbIsAlert = true;
                }

                if (!mbIsCallZombie && mUnlockTimer >= m_CallZombieTime)
                {
                    // ¡ª∫Ò º“»Ø
                    Debug.Log("¿⁄π∞ºË : ¡ª∫Ò º“»Ø");
                    mbIsCallZombie = true;
                }

                if (!mbIsUnlock && mUnlockTimer >= 8)
                {
                    Debug.Log("≈ª√‚");
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
