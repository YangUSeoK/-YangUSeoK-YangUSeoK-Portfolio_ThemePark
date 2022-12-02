using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionUI : MonoBehaviour
{//22 12 02 ���ؿ�
    Item[] mGrabbale;
    GrabbaleUI[] mGrabbaleImage;

    private void Start()
    {
        mGrabbale = FindObjectsOfType<Item>();//������ ������Ʈ �ִ� ������Ʈ ã��
        mGrabbaleImage = new GrabbaleUI[mGrabbale.Length];
        for(int i=0;i<mGrabbale.Length; i++)
        {
            mGrabbaleImage[i] = mGrabbale[i].GetComponentInChildren<GrabbaleUI>();
        }
    }
    private void Update()
    {
        for(int mCnt=0;mCnt<mGrabbale.Length; mCnt++)
        {
            if(mGrabbale[mCnt].GetComponent<Rigidbody>().isKinematic==true)//������ٵ� Ű�׸�ƽ �˻�
            {
                mGrabbaleImage[mCnt].gameObject.SetActive(false);//��������� UI����
            }
            else
            {
                mGrabbaleImage[mCnt].gameObject.SetActive(true);//UI Ȱ��ȭ
            }
        }
    }
}