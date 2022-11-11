using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    //2022 11 03 김준우
    public enum PlayerIdle
    { 
        Squat,
        SlowWalk,
        Walk,
        Run
    }
    Transform playerTr;
    Rigidbody rb;
    CharacterController cc;
    public Transform camera;
    PlayerIdle playerIdle;

    float mCurrSpeed;
    float mNormalSpeed = 4.2f;
    float mRunSpeed = 12f;
    float mSlowSpeed = 2.4f;
    float mSquatSpeed = 1.3f;
    float mRotSpeed = 30f;
    bool mbIsSquat = false;
    float h;
    float v;

    void Start()
    {
        playerTr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        cc= GetComponent<CharacterController>();
    }
    void Update()
    {
        Vector2 mov2d = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        v = mov2d.x;
        h = mov2d.y;
        Toggle();
        PlayerState();
        Move();
        PlayerAndCameraRelationRotate();//카메라의 시각으로 플레이어 회전
        PlayerTrRotate();//플레이어의 시각을 회전
    }
    void Toggle()//앉기 토글 함수
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
        {                                         //만약 오큘러스에서 앉기 토글이 안되는 경우가 생긴다면 이거 조건 문제임
            if (mbIsSquat == false)
            {
                //Debug.Log("앉기 진입");
                mbIsSquat = true;
                cc.height = 1f;
                cc.center = new Vector3(0f,0.5f,0f);
                playerIdle = PlayerIdle.Squat;
            }
            else
            {
                //Debug.Log("서기 진입");
                mbIsSquat = false;
                cc.height = 2f;
                cc.center = new Vector3(0f, 1f, 0f);
                playerIdle = PlayerIdle.Walk;
            }
        }
    }
    void PlayerState()//2022 11 03 김준우
    {//2022 11 04 김준우
        if(OVRInput.Get(OVRInput.Button.One)&&h>=0)//달리기 A버튼을 누르고 있는 중이라면
        {
            //Debug.Log("상태전환 진입");
            if (mbIsSquat == true)
            {
                //Debug.Log("앉은 상태에서 달리기 돌입");
                mbIsSquat = false;
            }
            mCurrSpeed = mRunSpeed;
            playerIdle = PlayerIdle.Run;
            StartCoroutine(VibrateController(1f,0.3f,0.3f,OVRInput.Controller.All));
        }
        else if (mbIsSquat==true)//2022 11 03 김준우,, 달리기 중이 아니고, 앉은 상태가 true 일 때
        {
            mCurrSpeed = mSquatSpeed;
        }
        else if (h >= 0.95f)//앞으로 Walk 상태
        {
            mCurrSpeed = mNormalSpeed;
            playerIdle = PlayerIdle.Walk;
        }
        else if (h < 0.95f&&h>0)//앞으로 Slow Walk
        {
            mCurrSpeed = mSlowSpeed;
            playerIdle=PlayerIdle.SlowWalk;
        }
        else if (h<0)//뒤로 Slow Walk
        {
            mCurrSpeed = mSlowSpeed;
            playerIdle = PlayerIdle.SlowWalk;
        }
    }
    void Move()//실제 움직임 구현 함수
    {
        Vector3 mov = new Vector3(v * Time.deltaTime * mCurrSpeed, 0f, h * Time.deltaTime * mCurrSpeed);
        mov = camera.TransformDirection(mov);
        cc.Move(mov);
    }
    void PlayerAndCameraRelationRotate()//2022 11 04 김준우 ,, 카메라와 플레이어 로테이션 맞추기 포기, 이건 시간 남으면 나중에
    {
        if(OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
        {
            Debug.Log("회전 맞추기");//이게 도저히 안되네;
            playerTr.eulerAngles = new Vector3(0f, 0f, 0f);
            camera.eulerAngles = new Vector3(0f, 0f, 0f);
            playerTr.eulerAngles = new Vector3(playerTr.eulerAngles.x, Camera.main.transform.eulerAngles.y, playerTr.eulerAngles.z);
            camera.eulerAngles = new Vector3(0, -playerTr.eulerAngles.y,0f);
        }
    }
    void PlayerTrRotate()//2022 11 04 김준우,, 오른쪽 십자 컨트롤러를 조작해서 플레이어 로테이션을 회전
    {
        if(OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight))//오른 십자 오른쪽
        {
            playerTr.eulerAngles += new Vector3(0f, mRotSpeed * Time.deltaTime, 0f);
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft))//오른 십자 왼쪽
        {
            playerTr.eulerAngles += new Vector3(0f, -mRotSpeed * Time.deltaTime, 0f);
        }
    }
    IEnumerator VibrateController(float _waitTime, float _vibTime, float _vibPower, OVRInput.Controller _controller)
    {                           //대기시간,진동시간,진동세기,오/왼 컨트롤러
        OVRInput.SetControllerVibration(_vibTime, _vibPower, _controller);
        yield return new WaitForSeconds(_waitTime);
        OVRInput.SetControllerVibration(0f, 0f, _controller);
    }


}
