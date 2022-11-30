using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionUI : MonoBehaviour
{//22 11 29 ���ؿ�
    public Image mItemPos;
    private ICatch[] mGrabbableItem;

    void Start()
    {
        mItemPos.enabled = false;
        mGrabbableItem = FindObjectsOfType<ICatch>();
    }

    void Update()
    {
        for(int mCnt=0; mCnt<mGrabbableItem.Length; mCnt++)//221130 ���ؿ�
        {
            if(mGrabbableItem[mCnt].GetComponent<Item>().mbUICatch==false)
            {
                Instantiate(mItemPos);
                mItemPos.transform.SetParent(mGrabbableItem[mCnt].transform);
                mItemPos.transform.position = mGrabbableItem[mCnt].transform.position + Vector3.up;
                mItemPos.enabled = true;//�̰� �ʿ��Ѱ�?
                mGrabbableItem[mCnt].GetComponent<Item>().mbUICatch = true;
            }
            //if(mGrabbableItem[mCnt].GetComponent<Item>().mbIsHanded == true ||
            //    mGrabbableItem[mCnt].GetComponent<Item>().mbIsInPocket == true)//��� �ְų� �κ��丮�� �������
            //{
            //    this.mItemPos.enabled= false;
            //}
        }
    }
}
