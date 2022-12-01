using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionUI : MonoBehaviour
{//22 11 29 김준우
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
        for(int mCnt=0; mCnt<mGrabbableItem.Length; mCnt++)//221130 김준우
        {
            if(mGrabbableItem[mCnt].GetComponent<Item>().mbUICatch==false)
            {   
                mItemPos.GetComponent<Image>().enabled = true;//이거 필요한가?
                mGrabbableItem[mCnt].GetComponent<Item>().mbUICatch = true;
                Instantiate(mItemPos,mCanvas.transform);
            }
            //if(mGrabbableItem[mCnt].GetComponent<Item>().mbIsHanded == true ||
            //    mGrabbableItem[mCnt].GetComponent<Item>().mbIsInPocket == true)//쥐고 있거나 인벤토리에 있을경우
            //{
            //    this.mItemPos.enabled= false;
            //}
        }
    }
}