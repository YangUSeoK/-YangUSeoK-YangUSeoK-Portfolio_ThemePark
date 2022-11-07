using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_SightMan : EnemyAI
{
    // 빛의 트랜스폼 값 받아올 방업 생각해봐야 함
    [SerializeField] private Transform m_LightTr = null;
    private Enemy_SightMan m_SightMan = null;


    // 코루틴 변수
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

            // 공격사거리 안쪽이면
            if (dist <= m_Enemy.AttackRange)
            {
                //부채꼴 범위에 있으면 공격한다
                {
                    mState = EState.Attack;
                    yield break;
                }
            }

            // 공격사거리 안쪽이 아니면
            else
            {
                if (!mbIsTrace)
                {
                    // 플레이어를 본다면 (= 눈에 보이면) 
                    if (m_EnemyFOV.IsInFOV(m_Enemy.LookPlayerRange, m_PlayerTr, LayerMask.NameToLayer("PLAYER")))                        
                    {
                        mState = EState.Trace;
                        mbIsAlert = false;
                        mbIsTrace = true;
                    }

                    // 플레이어를 못봤다면
                    else
                    {
                        if (!mbIsAlert)
                        {
                            // 빛을 본다면 
                            // 20221107 양우석 : 빛의 Transform을 받아올 방법을 생각해야함. 지금은 인스팩터
                            if (m_EnemyFOV.IsInFOV(m_SightMan.LookLightRange, m_LightTr, LayerMask.NameToLayer("LIGHT")))
                                
                            {
                                mState = EState.Alert;
                                mbIsAlert = true;
                            }
                            // 빛을 못봤다면
                            else
                            {
                                mState = EState.Patrol;
                            }
                        }

                        // 경계상태라면 (mbIsAlert = true)
                        else
                        {
                            // 시야에서 빛이 5초간 사라진다면
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
                    // if(시야에서 적이 5초간 사라진다면)
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
        Debug.Log("코루틴");
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
