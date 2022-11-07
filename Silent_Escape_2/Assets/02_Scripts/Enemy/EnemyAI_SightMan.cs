using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_SightMan : EnemyAI
{
    // ���� Ʈ������ �� �޾ƿ� ��� �����غ��� ��
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
                    if (m_EnemyFOV.IsInFOV(m_Enemy.LookPlayerRange, m_PlayerTr, LayerMask.NameToLayer("PLAYER"))
                        && m_EnemyFOV.IsLookTarget(m_Enemy.LookPlayerRange, m_PlayerTr))
                    {
                        mState = EState.Trace;
                    }
                    // ���� ���ٸ� 
                    // 20221107 ��켮 : ���� Transform�� �޾ƿ� ����� �����ؾ���. ������ �ν�����
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
                else
                {
                    // if(�þ߿��� ���� 5�ʰ� ������ٸ�)
                    // mState = EState.Alert;
                    // mbIsTrace = false;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
