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
    public Transform playerTr;
    public Transform camera;
    public Transform body;
    public Rigidbody rb;
    PlayerIdle playerIdle;

    Vector3 mPlayerInitPos;
    float mCurrSpeed;
    float mNormalSpeed = 4.2f;
    float mRunSpeed = 12f;
    float mSlowSpeed = 2.4f;
    float mSquatSpeed = 1.3f;
    public float mRotAngle = 30f;
    bool mbIsSquat = false;
    float h;
    float v;

    private void FixedUpdate()
    {
        Move();
    }
    void Update()
    {
        Vector2 mov2d = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        v = mov2d.x;
        h = mov2d.y;
        //컴퓨터 테스트 용이므로 VR테스트 시 해당 코드 주석 처리 ㄱ
        //h = Input.GetAxis("Horizontal");
        //v= Input.GetAxis("Vertical");
        ToggleSit();
        PlayerState();
        //Move();
        PlayerAndCameraRelationRotate();//카메라의 시각으로 플레이어 회전
        PlayerTrRotate();//플레이어의 시각을 회전
    }
    void ToggleSit()//앉기 토글 함수
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick) || Input.GetKeyDown("f"))
        {                                         //만약 오큘러스에서 앉기 토글이 안되는 경우가 생긴다면 이거 조건 문제임
            if (mbIsSquat == false)//221123 김준우 이거 +1-1반복하다보면 소수점 오차 생김 이건 나중에 처리
            {
                mPlayerInitPos.y = 1;
                Debug.Log("앉기 진입");
                mbIsSquat = true;
                playerTr.position = playerTr.position - mPlayerInitPos;
                playerIdle = PlayerIdle.Squat;
            }
            else
            {
                Debug.Log("서기 진입");
                mbIsSquat = false;
                playerTr.position = playerTr.position + mPlayerInitPos;
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
                playerTr.position = playerTr.position + mPlayerInitPos;
            }
            mCurrSpeed = mRunSpeed;
            playerIdle = PlayerIdle.Run;
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
        Debug.Log(camera.forward);
        Debug.Log(transform.forward);

        Vector3 cameraForward = new Vector3(camera.forward.x, 0f, camera.forward.z).normalized;
        Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward).normalized;
        Vector3 moveDir = ((cameraForward * h) + (cameraRight * v)).normalized;

        transform.Translate(moveDir * mCurrSpeed * Time.deltaTime);
        //rb.((moveDir * mCurrSpeed * Time.deltaTime));
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
        if(OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight)||Input.GetKeyDown("l"))//오른 십자 오른쪽
        {
            body.eulerAngles += new Vector3(0f, mRotAngle, 0f);
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft) || Input.GetKeyDown("j"))//오른 십자 왼쪽
        {
            body.eulerAngles += new Vector3(0f, -mRotAngle, 0f);
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp)
            ||OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickDown) || Input.GetKeyDown("i"))//오른 십자 왼쪽
        {
            body.eulerAngles += new Vector3(0f, 180f, 0f);
        }
    }
    IEnumerator VibrateController(float _waitTime, float _vibTime, float _vibPower, OVRInput.Controller _controller)
    {                           //대기시간,진동시간,진동세기,오/왼 컨트롤러
        OVRInput.SetControllerVibration(_vibTime, _vibPower, _controller);
        yield return new WaitForSeconds(_waitTime);
        OVRInput.SetControllerVibration(0f, 0f, _controller);
    }
}
