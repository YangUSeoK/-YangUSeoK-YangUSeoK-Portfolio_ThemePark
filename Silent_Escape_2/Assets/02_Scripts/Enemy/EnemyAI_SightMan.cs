using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_SightMan : EnemyAI
{
    // 빛의 트랜스폼 값 받아올 방업 생각해봐야 함
    [SerializeField] private Transform m_LightTr = null;
    private Enemy_SightMan m_SightMan = null;


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
                    if (m_EnemyFOV.IsInFOV(m_Enemy.LookPlayerRange, m_PlayerTr, LayerMask.NameToLayer("PLAYER"))
                        && m_EnemyFOV.IsLookTarget(m_Enemy.LookPlayerRange, m_PlayerTr))
                    {
                        mState = EState.Trace;
                    }
                    // 빛을 본다면 
                    // 20221107 양우석 : 빛의 Transform을 받아올 방법을 생각해야함. 지금은 인스팩터
                    else if (m_EnemyFOV.IsInFOV(m_SightMan.LookLightRange, m_LightTr, LayerMask.NameToLayer("LIGHT"))
                        && m_EnemyFOV.IsLookTarget(m_SightMan.LookLightRange, m_LightTr))
                    {
                        mState = EState.Alert;
                    }
                    // 아무것도 아니면
                    else
                    {
                        mState = EState.Patrol;
                    }
                }
                else
                {
                    // if(시야에서 적이 5초간 사라진다면)
                    // mState = EState.Alert;
                    // mbIsTrace = false;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
