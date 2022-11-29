using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionUI : MonoBehaviour
{//22 11 29 김준우
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
        //if(mGrabbedItem.mbIsHanded==true)//아아템 그랩 여부 어떻게 판단할것인지 고민 좀 하셈
        //{
        //    mItemPos.enabled = false;
        //}
    }
}
