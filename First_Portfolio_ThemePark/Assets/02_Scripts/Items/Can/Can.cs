using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : Item
{
    EItemType eItemtype = EItemType.Can;
    [SerializeField] LayerMask m_ListenerLayer;
    
    public override void Action()
    {
        // ĵ�� �浹������ ������ �Լ�����
        // �Ҹ��� ����ϰų�, ����Ʈ�� �����ϰų�
        // ȿ������ ������ �ӵ�(�������) �� ���� �����ؼ� ���ϰ� ���������� ���ϰ� �������� �Ҹ��� �ٸ��� ���� ��
        m_ItemAudio[0].volume = m_Speed * 0.05f;
        m_ItemAudio[0].PlayOneShot(m_ItemAudio[0].clip);
    }

    // �浹�ҽ� �Ҹ��� ����ϰ�, ��ġ�� �˸���, �ݻ簢���� ��ü�� ƨ�ܳ�����
    // ���߿� ������ ������� �ݶ��̴��� ��ġ�ԵǸ� ������ ����� ����
    // ���� ����� ������� �������� �ݶ��̴��� ��Ȱ��ȭ �ϰų� ���� �ݶ��̴��� ��Ȱ��ȭ �Ұ�
    private void OnCollisionEnter(Collision coll)
    {
        if (m_Speed > 1f)
        {
            Action();
            inform();
            HitObject(coll);
        }
    }

    // �浹������ ��ġ�� �˸��� ���� �Լ�(�ӽ÷� ���������Ŷ� ���߿� �����ʿ�)
    void inform()
    {
        Collider[] Listeners = Physics.OverlapSphere(transform.position, m_ItemAudio[0].maxDistance * m_ItemAudio[0].volume, m_ListenerLayer);

        foreach (Collider listener in Listeners)
        {
            listener.GetComponent<Enemy_Listener>().Listen(transform, transform.position, m_ItemAudio[0].maxDistance * m_ItemAudio[0].volume);
        }
    }
}
