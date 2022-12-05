using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionUI : MonoBehaviour
{//22 12 02 김준우
    Item[] mGrabbale;
    GrabbaleUI[] mGrabbaleImage;

    private void Start()
    {
        mGrabbale = FindObjectsOfType<Item>();//아이템 컴포넌트 있는 오브젝트 찾기
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
            if(mGrabbale[mCnt].GetComponent<Rigidbody>().isKinematic==true)//리지드바디 키네마틱 검사
            {
                mGrabbaleImage[mCnt].gameObject.SetActive(false);//잡고있으면 UI제거
            }
            else
            {
                mGrabbaleImage[mCnt].gameObject.SetActive(true);//UI 활성화
            }
        }
    }
}