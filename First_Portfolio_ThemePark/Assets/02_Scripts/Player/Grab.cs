using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//2022 11 08 ���ؿ�
public class Grab : MonoBehaviour
{
    public struct SInventory//�κ��丮 ����ü
    {
        public GameObject Inven;
        public int currItemIndex;
        public bool isGetItem;
    }
    public Transform lHand;
    public Transform rHand;

    bool mbLeftIsGrabbing = false;
    bool mbRightIsGrabbing = false;
    bool mbOnColl=false;
    bool isDoingGrabbingAnimation = false;
    GameObject grabbedObject;
    public LayerMask grabbedLayer;
    public float grabRange = 0.5f;
    Item.EItemType eItemType;
    
    public float remoteGrabDistance = 5;
    [Header("�������ε���")]
    public GameObject[] ItemIndex;
    int currItemIndex;

    [Header("������ ����")]
    Vector3 prevPos;
    public float throwPower = 1f;
    Quaternion prevRot;
    public float rotPower = 1f;

    SInventory[] msLRInven;
    private void Start()
    {
        msLRInven = new SInventory[2];
        msLRInven[0].isGetItem = false;//���� �޼�
        msLRInven[1].isGetItem = false;//���� ������
    }
    void Update()
    {
        if (mbOnColl == false)
        {
            //22 11 09 ���ؿ�
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            {//�޼� Ʈ���Ÿ� ���� �ƹ��͵� ��� ���� ���� ��
                TryLongGrab(OVRInput.Controller.LTouch, lHand);
            }
            if(OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
            {
                TryGrab(OVRInput.Controller.LTouch, lHand, mbLeftIsGrabbing);
            }
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
            {   //�޼� Ʈ���Ÿ� ���� ���𰡸� ��� ���� ��
                //�����ų� ���� �������� �� �̻��� ���� ����� ���⼭ �������� �޾ƾ� �� ��?
                TryUnGrab(OVRInput.Controller.LTouch, OVRInput.Button.PrimaryHandTrigger, lHand, mbLeftIsGrabbing);
            }
            if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
            {
                TryLongGrab(OVRInput.Controller.RTouch, rHand);
            }
            if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
            {
                TryGrab(OVRInput.Controller.RTouch, rHand, mbRightIsGrabbing);
            }
            else if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
            {
                TryUnGrab(OVRInput.Controller.RTouch, OVRInput.Button.SecondaryHandTrigger, rHand, mbRightIsGrabbing);
            }
        }
    }
    void TryLongGrab(OVRInput.Controller _controller ,Transform _hand)//22 11 25
    {
        Ray ray = new Ray(_hand.position, _hand.rotation * Vector3.forward);
        RaycastHit hitInfo;
        if (Physics.SphereCast(ray, 0.5f/*(��ü �ݰ�)*/, out hitInfo, remoteGrabDistance, grabbedLayer))
        {
            grabbedObject = hitInfo.transform.gameObject;
            //��ü�� �������� �ڷ�ƾ �Լ� ȣ��
            StartCoroutine(GrabbingAnimation(_controller, _hand));
            
        }
    }
    private void TryGrab(OVRInput.Controller _controller, Transform _hand, bool _isGrab)//���
    {
        if (_isGrab == true)
        {
            //_isGrab = false;
            return;
        }
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
            //�տ� ��� ���� �� ������� ����
            grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObject.GetComponent<BoxCollider>().enabled = false;


            grabbedObject = hitObjects[closest].gameObject;
            grabbedObject.transform.SetParent(_hand.transform, false);
            grabbedObject.transform.position = _hand.position;//��ġ ����
            //grabbedObject.transform.localPosition += new Vector3(0f, 0f, 0.5f);
            grabbedObject.transform.rotation = _hand.rotation;
            //������ ���� ��� ���� ������ġ ����
            prevPos = OVRInput.GetLocalControllerPosition(_controller);
            prevRot = _hand.rotation;

            //������ �׷� ���� �˷���
            _isGrab = true;
            grabbedObject.GetComponent<Item>().SetIsGrabed(true);
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    private void TryUnGrab(OVRInput.Controller _controller, OVRInput.Button _button, Transform _hand, bool _isGrab)
    {
        Debug.Log("�׷��� �ȸԴ°� ������?");
        //���� ����(��ġ - ��ġ = ������ ����)
        Vector3 throwDirection = _hand.position - prevPos;
        prevPos = _hand.position;

        Quaternion deltaRotation = _hand.rotation * Quaternion.Inverse(prevRot);
        //Inverse ��ŧ���� ����Ʈ ��Ʈ�ѷ��� ���ư� ������ ����ϴ°�
        prevRot = _hand.rotation;

        grabbedObject.GetComponent<BoxCollider>().enabled = true;
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        grabbedObject.transform.parent = null;
        grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;

        //�� �ӵ� = (1/dt)* dCeta(Ư�� �� ���� ���� ����=����� ����)
        float angle;
        Vector3 axis;
        deltaRotation.ToAngleAxis(out angle, out axis);
        Vector3 angularVelocity = (1f / Time.deltaTime) * angle * axis;
        grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity * rotPower;

        grabbedObject.GetComponent<Item>().SetIsGrabed(false);
        grabbedObject = null;
        _isGrab = false;
    }
    IEnumerator GrabbingAnimation(OVRInput.Controller _controller, Transform _hand)//��ü�� ������ ������
    {
        if (isDoingGrabbingAnimation == true)
        {
            yield return null;
        }
        else
        {
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            prevPos = _hand.position;
            prevRot = _hand.rotation;

            Vector3 startLocation = grabbedObject.transform.position;
            Vector3 targetLocation = prevPos + _hand.rotation * Vector3.forward * 0.1f;

            float currentTime = 0f;
            float finishTime = 0.2f;
            float elapsedRate = currentTime / finishTime;
            while (elapsedRate < 1)
            {
                currentTime += Time.deltaTime;
                elapsedRate = currentTime / finishTime;
                grabbedObject.transform.position = Vector3.Lerp(startLocation, targetLocation, elapsedRate);

                yield return null;
            }
            grabbedObject.transform.position = _hand.position;
            grabbedObject.transform.parent = _hand;
            //grabbedObject.transform.localPosition += new Vector3(0f, 0f, 0.5f);
            //grabbedObject.GetComponent<BoxCollider>().enabled = true;   // ������ �տ� ����� �ݶ��̴� �Ѿ���.
            isDoingGrabbingAnimation = false;
        }
    }
    void InvenSave(SInventory _LRInven, bool _isGrab)//2022 11 25 ���ؿ�
    {
        //���⿡ �κ��丮�� ������ �ε��� �ְ� ��Ʈ����
        if (grabbedObject.GetComponent<Item>() != null)//22 11 28 ���ؿ�
        {//�׷� ������Ʈ�� ������������Ʈ�� �ִ��� �˻�
            if (grabbedObject.Equals(Item.EItemType.Flashlight))//������ ������Ʈ�� EItemType���� �з�
            {
                _LRInven.currItemIndex = 0;
            }
            else if (grabbedObject.Equals(Item.EItemType.Bottle))
            {
                _LRInven.currItemIndex = 1;
            }
            else if (grabbedObject.Equals(Item.EItemType.Can))
            {
                _LRInven.currItemIndex = 2;
            }
            _LRInven.isGetItem = true;
        }
        else return;//������ ������Ʈ�� ���ٸ� �׳� ����
        grabbedObject = null;
        Destroy(grabbedObject);
        _isGrab = true;
    }
    void GetInvenItem(SInventory _LRInven, Transform _hand, bool _isGrab)
    {
        if (_isGrab == false)
        {
            //���⿡ ���� ������ �ε����� �ν��Ͻÿ���Ʈ �ϴ� ���
            grabbedObject = Instantiate(ItemIndex[currItemIndex], _hand);
            _isGrab = true;
        }
    }
    private void OnTriggerStay(Collider other)//�κ��丮
    {
        if (other.CompareTag("INVEN")/*||grabbedObject.CompareTag("ITEM")*/)
        {
            mbOnColl = true;
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) && mbLeftIsGrabbing == false)
            {//�޼� Ʈ���Ÿ� ���� �κ��丮 �ݶ��̴��� ������ ��
                GetInvenItem(msLRInven[0], lHand, mbLeftIsGrabbing);
            }
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
            {   //�޼� Ʈ���Ÿ� ���� ���𰡸� ��� �ְ�, �κ��丮 �ݶ��̴��� ������ ��
                InvenSave(msLRInven[0], mbLeftIsGrabbing);
            }
            if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))//������ Ʈ����
            {
                GetInvenItem(msLRInven[1], rHand, mbRightIsGrabbing);
            }
            else if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))//������ �κ����� ��������
            {
                InvenSave(msLRInven[1], mbRightIsGrabbing);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("INVEN"))
            mbOnColl = false;
    }
}