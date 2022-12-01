using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMiniMap : MonoBehaviour
{
    public Camera MainCamera;
    public Camera MiniMapCam;
    bool mbIsMiniMapCam = false;

    private void Start()
    {
        MiniMapCam.enabled = false;
    }
    void Update()
    {
        //ø©±‚º≠ ¿Œ«≤
        if(Input.GetKeyDown("j"))
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
            MainCamera.enabled = false;
            MiniMapCam.enabled = true;

        }
        else
        {
            //MiniMapCam.rect = new Rect(0.05f, 0.85f, 0.1f, 0.1f);
            MainCamera.enabled = true;
            MiniMapCam.enabled = false;
        }
    }
}
