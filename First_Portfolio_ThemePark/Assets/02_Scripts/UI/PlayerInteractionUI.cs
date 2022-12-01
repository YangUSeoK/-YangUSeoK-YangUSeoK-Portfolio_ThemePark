using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionUI : MonoBehaviour
{//22 11 29 ���ؿ�
    public Image mItemPos;
    private Item[] mGrabbableItem;
    public Canvas mCanvas;
    RectTransform mRectCanvas;
    Camera mUICamera;
    void Start()
    {
        mItemPos.enabled = false;
        mGrabbableItem = FindObjectsOfType<Item>();
        mUICamera = mCanvas.worldCamera;
        mRectCanvas = mCanvas.GetComponent<RectTransform>();
    }

    void Update()
    {
        for(int mCnt=0; mCnt<mGrabbableItem.Length; mCnt++)//221130 ���ؿ�
        {
            if(mGrabbableItem[mCnt].GetComponent<Item>().mbUICatch==false)
            {   
                mItemPos.GetComponent<Image>().enabled = true;//�̰� �ʿ��Ѱ�?
                mGrabbableItem[mCnt].GetComponent<Item>().mbUICatch = true;
                Instantiate(mItemPos,mCanvas.transform);
            }
            //if(mGrabbableItem[mCnt].GetComponent<Item>().mbIsHanded == true ||
            //    mGrabbableItem[mCnt].GetComponent<Item>().mbIsInPocket == true)//��� �ְų� �κ��丮�� �������
            //{
            //    this.mItemPos.enabled= false;
            //}
        }
    }
}