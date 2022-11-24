using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//2022 11 08 ���ؿ�
public class Grab : MonoBehaviour
{
    public Transform lHand;
    public Transform rHand;

    bool mbLeftIsGrabbing = false;
    bool mbRightIsGrabbing = false;
    GameObject grabbedObject;
    public LayerMask grabbedLayer;
    public float grabRange = 5f;

    public float remoteGrabDistance = 25;

    [Header("������ ����")]
    Vector3 prevPos;
    public float throwPower = 1f;
    Quaternion prevRot;
    public float rotPower = 1f;

    void Update()
    {
        //22 11 09 ���ؿ�
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {//�޼� Ʈ���Ÿ� ���� �ƹ��͵� ��� ���� ���� ��
            TryGrab(OVRInput.Controller.LTouch, lHand, mbLeftIsGrabbing);
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
        {   //�޼� Ʈ���Ÿ� ���� ���𰡸� ��� ���� ��
            //�����ų� ���� �������� �� �̻��� ���� ����� ���⼭ �������� �޾ƾ� �� ��?
            Debug.Log("����?");
            TryUnGrab(OVRInput.Controller.LTouch, OVRInput.Button.PrimaryHandTrigger, lHand, mbLeftIsGrabbing);
        }
        if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            TryGrab(OVRInput.Controller.RTouch, rHand, mbRightIsGrabbing);
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
        {
            TryUnGrab(OVRInput.Controller.RTouch, OVRInput.Button.SecondaryHandTrigger, rHand,mbRightIsGrabbing);
        }
    }
    private void TryGrab(OVRInput.Controller _controller,Transform _hand,bool _isGrab)//���
    {
        if(_isGrab==true)
        {
            //_isGrab = false;
            return;
        }
        //���� ���
        Ray ray = new Ray(_hand.position, _hand.rotation * Vector3.forward);
        RaycastHit hitInfo;
        if (Physics.SphereCast(ray, 0.5f/*(��ü �ݰ�)*/, out hitInfo, remoteGrabDistance, grabbedLayer))
        {
            grabbedObject = hitInfo.transform.gameObject;
            //��ü�� �������� �ڷ�ƾ �Լ� ȣ��
            StartCoroutine(GrabbingAnimation(_controller, _hand));
        }
        _isGrab = true;
        Collider[] hitObjects = Physics.OverlapSphere(_hand.position, grabRange, grabbedLayer);
        int closest = 0;

        for (int i = 1; i < hitObjects.Length; i++)
        {
            Vector3 closestPos = hitObjects[closest].transform.position;
            float closestDistance = Vector3.Distance(closestPos, _hand.position);

            Vector3 nextPos = hitObjects[i].transform.position;
            float nextDistance = Vector3.Distance(nextPos, _hand.position);
            //�� �� �ε����� �ִ� ������Ʈ �Ÿ� ���Ͽ� ���� �ε����� �Ÿ��� ���� ����� �ִܰŸ����� ª���� �ε��� ����

            if (nextDistance < closestDistance)
                closest = i;
        }
        if (hitObjects.Length > 0)//��°� �����Ǵ� ����
        {
            grabbedObject = hitObjects[closest].gameObject;
            grabbedObject.transform.SetParent(_hand.transform, false);
            grabbedObject.transform.position = _hand.position;
            grabbedObject.transform.rotation = _hand.rotation;
            //�տ� ��� ���� �� ������� ����
            grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            // ��� ���� �ʱ���ġ ����
            prevPos = OVRInput.GetLocalControllerPosition(_controller);
            prevRot = _hand.rotation;

            // ������ �׷� �˷���
            grabbedObject.GetComponent<Item>().SetIsGrabed(true);
        }
    }
    private void TryUnGrab(OVRInput.Controller _controller, OVRInput.Button _button, Transform _hand,bool _isGrab)
    {
        Debug.Log("��ü ����?");
        //���� ����(��ġ - ��ġ = ������ ����)
        Vector3 throwDirection = _hand.position - prevPos;
        prevPos = _hand.position;

        Quaternion deltaRotation = _hand.rotation * Quaternion.Inverse(prevRot);
        //Inverse ��ŧ���� ����Ʈ ��Ʈ�ѷ��� ���ư� ������ ����ϴ°�
        prevRot = _hand.rotation;
        
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        grabbedObject.transform.parent = null;
        grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;

        //�� �ӵ� = (1/dt)* dCeta(Ư�� �� ���� ���� ����=����� ����)
        float angle;
        Vector3 axis;
        deltaRotation.ToAngleAxis(out angle, out axis);
        Vector3 angularVelocity = (1f / Time.deltaTime) * angle * axis;
        grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity * rotPower;

        grabbedObject = null;
        _isGrab = false;
    }
    IEnumerator GrabbingAnimation(OVRInput.Controller _controller, Transform _hand)//��ü�� ������ ������
    {
        grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
        prevPos = _hand.position;
        prevRot = _hand.rotation;

        Vector3 startLocation = grabbedObject.transform.position;
        Vector3 targetLocation = prevPos + _hand.rotation * Vector3.forward * 0.1f;

        float currentTime = 0f;
        float finishTime = 0.2f;
        float elapsedRate = currentTime / finishTime;
        while(elapsedRate<1)
        {
            currentTime += Time.deltaTime;
            elapsedRate = currentTime / finishTime;
            grabbedObject.transform.position = Vector3.Lerp(startLocation, targetLocation, elapsedRate);

            yield return null;
        }
        grabbedObject.transform.position = targetLocation;
        grabbedObject.transform.parent = _hand;
        grabbedObject.transform.localPosition += new Vector3(-1f, 2f, 0f);

    }
}
