using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionUI : MonoBehaviour
{//22 11 29 ���ؿ�
    public Image mItemPos;
    //GameObject mGrabbedItem;
    void Start()
    {
        mItemPos.enabled = false;
        //mGrabbedItem.GetComponent<ICatch>();
    }

    void Update()
    {
        mItemPos.transform.position = FindObjectOfType<ICatch>().transform.position + Vector3.up;
        mItemPos.enabled = true;
        //if(mGrabbedItem.mbIsHanded==true)//�ƾ��� �׷� ���� ��� �Ǵ��Ұ����� ��� �� �ϼ�
        //{
        //    mItemPos.enabled = false;
        //}
    }
}
