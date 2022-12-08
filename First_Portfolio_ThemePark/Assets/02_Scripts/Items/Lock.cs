using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Lock : MonoBehaviour
{
    [SerializeField] Light m_DirectionalLight = null;
    [SerializeField] StreetLightManager m_StreetLightManager = null;
    [SerializeField] Transform m_LockHead = null;
    [SerializeField] EnemyManager m_EnemyManager = null;
    [SerializeField] Transform m_PlayerTr = null;
    [SerializeField] SlaughterFactory[] m_ZombieGroups = null;
    [SerializeField] JailDoor m_Door = null;

    [Space]
    [Header("Timer")]
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

                    // ��� ���ε� �� ����
                    m_StreetLightManager.TurnOffAllLight();
                }

                if (!mbIsCallZombie && mUnlockTimer >= m_CallZombieTime)
                {
                    Debug.Log("�ڹ��� : ���� ��ȯ");

                    // ���� ��ȯ => ��ȯ�� ����(������) �����ϴ� ZombiGroup �ν����Ϳ� ������ ��.
                    // �����ʴ� �׳� �Ҹ���� �˾Ƽ� ��
                    for(int i = 0; i < m_ZombieGroups.Length; ++i)
                    {
                        m_ZombieGroups[i].IsUnlcking = true;
                        m_ZombieGroups[i].SetActiveZombies();
                    }

                    // ��ó 50 ���� �� ��� ���� �θ�
                    m_EnemyManager.CallNearZombie(m_PlayerTr, 50f);
                    mbIsCallZombie = true;
                }

                if (!mbIsUnlock && mUnlockTimer >= 8)
                {
                    Debug.Log("Ż��");
                    // ������ �ִϸ��̼� �־�� ��
                    StartCoroutine(m_Door.OpenDoor());

                    m_LockHead.transform.localPosition = new Vector3(0.0245f, 0.07f, 0f);
                    m_LockHead.transform.localRotation = Quaternion.Euler(0f, 45f, 0f);
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
        float maxIntensity = 0.4f;
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
            m_DirectionalLight.intensity += (0.002f * sign);

            yield return null;
        }
    }
}
