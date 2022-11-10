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
                                    Debug.Log("코루틴 정지");
                                    StopCoroutine(mCoroutine);
                                }
                                Debug.Log("코루틴 시작");
                                mCoroutine = CountTimeAndBoolOffCoroutine(mbIsAlert);
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
                    Debug.Log("is Trace!");
                    // if(시야에서 플레이어가 5초간 사라진다면)
                    if (!(m_EnemyFOV.IsInFOV(m_Enemy.LookPlayerRange, m_PlayerTr, LayerMask.NameToLayer("PLAYER"))))
                    {
                        if (mCoroutine != null)
                        {
                            Debug.Log("코루틴 정지");
                            StopCoroutine(mCoroutine);
                        }
                        Debug.Log("코루틴 시작");
                        mCoroutine = CountTimeAndBoolOffCoroutine(mbIsTrace);
                        StartCoroutine(mCoroutine);
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

    private IEnumerator CountTimeAndBoolOffCoroutine(bool _state)
    {
        yield return m_AggroTime;
        
                _state = false;
          
            yield return null;
        
    }
}
