using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMiniMap : MonoBehaviour
{
    public Camera MiniMapCam;
    bool mbIsMiniMapCam = false;

    private void Start()
    {
        MiniMapCam.enabled = false;
    }
    void Update()
    {
        //���⼭ ��ǲ
        if(Input.GetKeyDown("j"))//�̰� �ٲܶ� ���߿� InvenUI���ǵ� ���� �ٲ�ߵ�
        {
            MapChange();
            mbIsMiniMapCam = !mbIsMiniMapCam;
        }
    }
    void MapChange()
    {
        if (mbIsMiniMapCam == false)
        {
            //MiniMapCam.rect = new Rect(0f, 0f, 1f, 1f);
            MiniMapCam.enabled = true;

        }
        else
        {
            //MiniMapCam.rect = new Rect(0.05f, 0.85f, 0.1f, 0.1f);
            MiniMapCam.enabled = false;
        }
    }
}
