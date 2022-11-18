using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        // target�� EnemyFOV���� �ݹ��Լ��� �޾ƿ�
        EnemyFOV fov = (EnemyFOV)target;


        // �Ͼ�� ���� �׸�.
        Handles.color = Color.white;
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.m_ViewLightRange);

        // ������ ���� �׸�(��ä��, �þ߹��� ǥ���� ��)
        Handles.color = new Color(1f, 1f, 1f, 0.2f);

        // enemyFOV���� �� ���� �� ���� ��Ƽ� ��ä�� ���������� ����
        Vector3 fromAnglePos = fov.CirclePoint(-fov.mViewAngle * 0.5f);

        // ��ä�� �׸� -         ���� ����,             �븻����,       ������,      ��ä�� ����,    ��ä�� ������
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePos, fov.mViewAngle, fov.m_ViewLightRange);

        // �� ����              ������               ������ ������ 2               �� ����(string)
        Handles.Label(fov.transform.position + (fov.transform.forward * 2f), fov.mViewAngle.ToString());


        // �÷��̾� Ž������
        Handles.color = new Color(1f, 1f, 1f, 0.3f);
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePos, fov.mViewAngle, fov.m_ViewPlayerRange);



    }
}
