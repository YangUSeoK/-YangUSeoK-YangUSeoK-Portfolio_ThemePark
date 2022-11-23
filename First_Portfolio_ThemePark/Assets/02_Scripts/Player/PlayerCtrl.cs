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
        //��ǻ�� �׽�Ʈ ���̹Ƿ� VR�׽�Ʈ �� �ش� �ڵ� �ּ� ó�� ��
        //h = Input.GetAxis("Horizontal");
        //v= Input.GetAxis("Vertical");
        ToggleSit();
        PlayerState();
        //Move();
        PlayerAndCameraRelationRotate();//ī�޶��� �ð����� �÷��̾� ȸ��
        PlayerTrRotate();//�÷��̾��� �ð��� ȸ��
    }
    void ToggleSit()//�ɱ� ��� �Լ�
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick) || Input.GetKeyDown("f"))
        {                                         //���� ��ŧ�������� �ɱ� ����� �ȵǴ� ��찡 ����ٸ� �̰� ���� ������
            if (mbIsSquat == false)//221123 ���ؿ� �̰� +1-1�ݺ��ϴٺ��� �Ҽ��� ���� ���� �̰� ���߿� ó��
            {
                mPlayerInitPos.y = 1;
                Debug.Log("�ɱ� ����");
                mbIsSquat = true;
                playerTr.position = playerTr.position - mPlayerInitPos;
                playerIdle = PlayerIdle.Squat;
            }
            else
            {
                Debug.Log("���� ����");
                mbIsSquat = false;
                playerTr.position = playerTr.position + mPlayerInitPos;
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
                playerTr.position = playerTr.position + mPlayerInitPos;
            }
            mCurrSpeed = mRunSpeed;
            playerIdle = PlayerIdle.Run;
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
        Debug.Log(camera.forward);
        Debug.Log(transform.forward);

        Vector3 cameraForward = new Vector3(camera.forward.x, 0f, camera.forward.z).normalized;
        Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward).normalized;
        Vector3 moveDir = ((cameraForward * h) + (cameraRight * v)).normalized;

        transform.Translate(moveDir * mCurrSpeed * Time.deltaTime);
        //rb.((moveDir * mCurrSpeed * Time.deltaTime));
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
        if(OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight)||Input.GetKeyDown("l"))//���� ���� ������
        {
            body.eulerAngles += new Vector3(0f, mRotAngle, 0f);
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft) || Input.GetKeyDown("j"))//���� ���� ����
        {
            body.eulerAngles += new Vector3(0f, -mRotAngle, 0f);
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp)
            ||OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickDown) || Input.GetKeyDown("i"))//���� ���� ����
        {
            body.eulerAngles += new Vector3(0f, 180f, 0f);
        }
    }
    IEnumerator VibrateController(float _waitTime, float _vibTime, float _vibPower, OVRInput.Controller _controller)
    {                           //���ð�,�����ð�,��������,��/�� ��Ʈ�ѷ�
        OVRInput.SetControllerVibration(_vibTime, _vibPower, _controller);
        yield return new WaitForSeconds(_waitTime);
        OVRInput.SetControllerVibration(0f, 0f, _controller);
    }
}
