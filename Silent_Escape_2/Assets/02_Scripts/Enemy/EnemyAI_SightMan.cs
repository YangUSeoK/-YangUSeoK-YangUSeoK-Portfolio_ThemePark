using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_SightMan : EnemyAI
{
    private float m_LookLightRange = 0;
    // 
    private Transform m_LightTr = null;
    private Enemy_SightMan m_SightMan = null;


    private void Awake()
    {
        base.Awake();
        m_SightMan = GetComponent<Enemy_SightMan>();
    }


    protected override IEnumerator ActionCoroutine()
    {
        while (true)
        {
            Debug.Log("Action");
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
                }
            }

            // 공격사거리 안쪽이 아니면
            else
            {
                if (!mbIsTrace)
                {
                    // 원뿔안에 들어온 상태면 (= 눈에 보이면) 
                    if (m_EnemyFOV.IsInFOV(m_Enemy.LookPlayerRange, m_PlayerTr, LayerMask.NameToLayer("PLAYER"))
                        && m_EnemyFOV.IsLookTarget(m_Enemy.LookPlayerRange, m_PlayerTr))
                    {
                        mState = EState.Trace;
                    }
                    // 빛을 본다면 
                    // 20221104 양우석 : 빛의 Transform을 받아올 방법을 생각해야함.
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



            }













            //        //// (들을 수 있는 범위 + 발소리 크기)보다 가까이 있으면 || 조명을 본다면      추가해야함 20221026
            //        //else if (dist <= mSoundDetectRange + m_PlayerTr.GetComponent<PlayerMove>().GetNoise())
            //        //{
            //        //    mState = EState.Tracer;
            //        //    mbIsTracer = true;
            //        //}

            //        //else// 아무것도 아니면 Default로 돌아간다.
            //        //{
            //        //    if (!mbIsTracer)
            //        //    {
            //        //        // switch로 각 종류마다 default 설정
            //        //        mState = EState.Patrol;
            //        //    }
            //        //}

            //        // 0.1초마다 체크
            //        yield return ws;
            //    }






            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SetLookLightRange(float _lookLightRange)
    {
        m_LookLightRange = _lookLightRange;
    }
}
