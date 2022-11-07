using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_SightMan : EnemyAI
{
    // ���� Ʈ������ �� �޾ƿ� ��� �����غ��� ��
    [SerializeField] private Transform m_LightTr = null;
    private Enemy_SightMan m_SightMan = null;


    // �ڷ�ƾ ����
    IEnumerator mCoroutine = null;

    protected override void Awake()
    {
        base.Awake();
        m_SightMan = GetComponent<Enemy_SightMan>();
    }


    protected override IEnumerator ActionCoroutine()
    {
        while (true)
        {



            yield return new WaitForSeconds(0.5f);
        }
    }

    protected override IEnumerator CheckStateCoroutine()
    {
        while (true)
        {
            float dist = Vector3.Distance(m_PlayerTr.position, transform.position);

            // ���ݻ�Ÿ� �����̸�
            if (dist <= m_Enemy.AttackRange)
            {
                //��ä�� ������ ������ �����Ѵ�
                {
                    mState = EState.Attack;
                    yield break;
                }
            }

            // ���ݻ�Ÿ� ������ �ƴϸ�
            else
            {
                if (!mbIsTrace)
                {
                    // �÷��̾ ���ٸ� (= ���� ���̸�) 
                    if (m_EnemyFOV.IsInFOV(m_Enemy.LookPlayerRange, m_PlayerTr, LayerMask.NameToLayer("PLAYER")))                        
                    {
                        mState = EState.Trace;
                        mbIsAlert = false;
                        mbIsTrace = true;
                    }

                    // �÷��̾ ���ôٸ�
                    else
                    {
                        if (!mbIsAlert)
                        {
                            // ���� ���ٸ� 
                            // 20221107 ��켮 : ���� Transform�� �޾ƿ� ����� �����ؾ���. ������ �ν�����
                            if (m_EnemyFOV.IsInFOV(m_SightMan.LookLightRange, m_LightTr, LayerMask.NameToLayer("LIGHT")))
                                
                            {
                                mState = EState.Alert;
                                mbIsAlert = true;
                            }
                            // ���� ���ôٸ�
                            else
                            {
                                mState = EState.Patrol;
                            }
                        }

                        // �����¶�� (mbIsAlert = true)
                        else
                        {
                            // �þ߿��� ���� 5�ʰ� ������ٸ�
                            if (!(m_EnemyFOV.IsInFOV(m_SightMan.LookLightRange, m_LightTr, LayerMask.NameToLayer("LIGHT"))))
                            {
                                if (mCoroutine != null)
                                {
                                    StopCoroutine(mCoroutine);
                                }
                                mCoroutine = CountTimeAndToggleBoolCoroutine(mbIsAlert);
                                StartCoroutine(mCoroutine);
                            }

                            if (!mbIsAlert)
                            {
                                mState = EState.Patrol;
                            }
                        }
                    }
                }
                //(mbIsTrace = true)
                else
                {
                    // if(�þ߿��� ���� 5�ʰ� ������ٸ�)
                    if (!(m_EnemyFOV.IsInFOV(m_Enemy.LookPlayerRange, m_PlayerTr, LayerMask.NameToLayer("PLAYER"))))
                    {
                        // CountTimeAndToggleBool(mbIsTrace);
                    }

                    if (!mbIsTrace)
                    {
                        mState = EState.Alert;
                        mbIsAlert = true;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void InitializeCoroutine(IEnumerator _coroutine)
    {

    }

    private IEnumerator CountTimeAndToggleBoolCoroutine(bool _state)
    {
        Debug.Log("�ڷ�ƾ");
        float i = 0;
        i += Time.deltaTime;

        if (i >= 5f)
        {
            _state = false;
            yield break;
        }

        yield return null;
    }
}
