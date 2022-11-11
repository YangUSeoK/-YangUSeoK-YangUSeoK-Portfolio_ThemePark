using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    //2022 11 03 ���ؿ�
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
        PlayerAndCameraRelationRotate();//ī�޶��� �ð����� �÷��̾� ȸ��
        PlayerTrRotate();//�÷��̾��� �ð��� ȸ��
    }
    void Toggle()//�ɱ� ��� �Լ�
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
        {                                         //���� ��ŧ�������� �ɱ� ����� �ȵǴ� ��찡 ����ٸ� �̰� ���� ������
            if (mbIsSquat == false)
            {
                //Debug.Log("�ɱ� ����");
                mbIsSquat = true;
                cc.height = 1f;
                cc.center = new Vector3(0f,0.5f,0f);
                playerIdle = PlayerIdle.Squat;
            }
            else
            {
                //Debug.Log("���� ����");
                mbIsSquat = false;
                cc.height = 2f;
                cc.center = new Vector3(0f, 1f, 0f);
                playerIdle = PlayerIdle.Walk;
            }
        }
    }
    void PlayerState()//2022 11 03 ���ؿ�
    {//2022 11 04 ���ؿ�
        if(OVRInput.Get(OVRInput.Button.One)&&h>=0)//�޸��� A��ư�� ������ �ִ� ���̶��
        {
            //Debug.Log("������ȯ ����");
            if (mbIsSquat == true)
            {
                //Debug.Log("���� ���¿��� �޸��� ����");
                mbIsSquat = false;
            }
            mCurrSpeed = mRunSpeed;
            playerIdle = PlayerIdle.Run;
            StartCoroutine(VibrateController(1f,0.3f,0.3f,OVRInput.Controller.All));
        }
        else if (mbIsSquat==true)//2022 11 03 ���ؿ�,, �޸��� ���� �ƴϰ�, ���� ���°� true �� ��
        {
            mCurrSpeed = mSquatSpeed;
        }
        else if (h >= 0.95f)//������ Walk ����
        {
            mCurrSpeed = mNormalSpeed;
            playerIdle = PlayerIdle.Walk;
        }
        else if (h < 0.95f&&h>0)//������ Slow Walk
        {
            mCurrSpeed = mSlowSpeed;
            playerIdle=PlayerIdle.SlowWalk;
        }
        else if (h<0)//�ڷ� Slow Walk
        {
            mCurrSpeed = mSlowSpeed;
            playerIdle = PlayerIdle.SlowWalk;
        }
    }
    void Move()//���� ������ ���� �Լ�
    {
        Vector3 mov = new Vector3(v * Time.deltaTime * mCurrSpeed, 0f, h * Time.deltaTime * mCurrSpeed);
        mov = camera.TransformDirection(mov);
        cc.Move(mov);
    }
    void PlayerAndCameraRelationRotate()//2022 11 04 ���ؿ� ,, ī�޶�� �÷��̾� �����̼� ���߱� ����, �̰� �ð� ������ ���߿�
    {
        if(OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
        {
            Debug.Log("ȸ�� ���߱�");//�̰� ������ �ȵǳ�;
            playerTr.eulerAngles = new Vector3(0f, 0f, 0f);
            camera.eulerAngles = new Vector3(0f, 0f, 0f);
            playerTr.eulerAngles = new Vector3(playerTr.eulerAngles.x, Camera.main.transform.eulerAngles.y, playerTr.eulerAngles.z);
            camera.eulerAngles = new Vector3(0, -playerTr.eulerAngles.y,0f);
        }
    }
    void PlayerTrRotate()//2022 11 04 ���ؿ�,, ������ ���� ��Ʈ�ѷ��� �����ؼ� �÷��̾� �����̼��� ȸ��
    {
        if(OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight))//���� ���� ������
        {
            playerTr.eulerAngles += new Vector3(0f, mRotSpeed * Time.deltaTime, 0f);
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft))//���� ���� ����
        {
            playerTr.eulerAngles += new Vector3(0f, -mRotSpeed * Time.deltaTime, 0f);
        }
    }
    IEnumerator VibrateController(float _waitTime, float _vibTime, float _vibPower, OVRInput.Controller _controller)
    {                           //���ð�,�����ð�,��������,��/�� ��Ʈ�ѷ�
        OVRInput.SetControllerVibration(_vibTime, _vibPower, _controller);
        yield return new WaitForSeconds(_waitTime);
        OVRInput.SetControllerVibration(0f, 0f, _controller);
    }


}
