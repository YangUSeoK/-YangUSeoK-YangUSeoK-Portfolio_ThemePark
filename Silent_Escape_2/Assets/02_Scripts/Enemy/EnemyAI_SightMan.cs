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

            // ���ݻ�Ÿ� �����̸�
            if (dist <= m_Enemy.AttackRange)
            {
                //��ä�� ������ ������ �����Ѵ�
                {
                    mState = EState.Attack;
                }
            }

            // ���ݻ�Ÿ� ������ �ƴϸ�
            else
            {
                if (!mbIsTrace)
                {
                    // ���Ծȿ� ���� ���¸� (= ���� ���̸�) 
                    if (m_EnemyFOV.IsInFOV(m_Enemy.LookPlayerRange, m_PlayerTr, LayerMask.NameToLayer("PLAYER"))
                        && m_EnemyFOV.IsLookTarget(m_Enemy.LookPlayerRange, m_PlayerTr))
                    {
                        mState = EState.Trace;
                    }
                    // ���� ���ٸ� 
                    // 20221104 ��켮 : ���� Transform�� �޾ƿ� ����� �����ؾ���.
                    else if (m_EnemyFOV.IsInFOV(m_SightMan.LookLightRange, m_LightTr, LayerMask.NameToLayer("LIGHT"))
                        && m_EnemyFOV.IsLookTarget(m_SightMan.LookLightRange, m_LightTr))
                    {
                        mState = EState.Alert;
                    }
                    // �ƹ��͵� �ƴϸ�
                    else
                    {
                        mState = EState.Patrol;
                    }
                }



            }













            //        //// (���� �� �ִ� ���� + �߼Ҹ� ũ��)���� ������ ������ || ������ ���ٸ�      �߰��ؾ��� 20221026
            //        //else if (dist <= mSoundDetectRange + m_PlayerTr.GetComponent<PlayerMove>().GetNoise())
            //        //{
            //        //    mState = EState.Tracer;
            //        //    mbIsTracer = true;
            //        //}

            //        //else// �ƹ��͵� �ƴϸ� Default�� ���ư���.
            //        //{
            //        //    if (!mbIsTracer)
            //        //    {
            //        //        // switch�� �� �������� default ����
            //        //        mState = EState.Patrol;
            //        //    }
            //        //}

            //        // 0.1�ʸ��� üũ
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
