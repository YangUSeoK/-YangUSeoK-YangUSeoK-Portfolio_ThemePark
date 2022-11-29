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

    [SerializeField] bool mbLeftIsGrabbing = false;     // �����
    [SerializeField] bool mbRightIsGrabbing = false;    
    bool mbOnColl=false;
    bool isDoingGrabbingAnimation = false;
    GameObject m_GrabbedObject = null;
    [SerializeField] GameObject m_RightGrabbedObject = null;    // �����
    [SerializeField] GameObject m_LeftGrabbedObject = null;
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
            else if (!mbOnColl && OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
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
            else if (!mbOnColl && OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
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
            if (_hand == lHand)
            {
                m_LeftGrabbedObject = hitInfo.transform.gameObject;
            }
            else
            {
                m_RightGrabbedObject = hitInfo.transform.gameObject;
            }
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
            _hand.GetComponentInChildren<ICatch>().gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _hand.GetComponentInChildren<ICatch>().gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            _hand.GetComponentInChildren<ICatch>().gameObject.GetComponent<Rigidbody>().isKinematic = true;
            _hand.GetComponentInChildren<ICatch>().gameObject.GetComponent<BoxCollider>().enabled = false;


            //_hand.GetComponentInChildren<ICatch>().gameObject = hitObjects[closest].gameObject;
            _hand.GetComponentInChildren<ICatch>().gameObject.transform.SetParent(_hand.transform, false);
            _hand.GetComponentInChildren<ICatch>().gameObject.transform.position = _hand.position;//��ġ ����
            //grabbedObject.transform.localPosition += new Vector3(0f, 0f, 0.5f);
            _hand.GetComponentInChildren<ICatch>().gameObject.transform.rotation = _hand.rotation;
            //������ ���� ��� ���� ������ġ ����
            prevPos = OVRInput.GetLocalControllerPosition(_controller);
            prevRot = _hand.rotation;

            //������ �׷� ���� �˷���
            _hand.GetComponentInChildren<ICatch>().gameObject.GetComponent<Item>().SetIsGrabed(true);
            _hand.GetComponentInChildren<ICatch>().transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

            if(_hand == lHand)
            {
                mbLeftIsGrabbing = true;
            }
            else
            {
                mbRightIsGrabbing = true;
            }
        }
    }
    private void TryUnGrab(OVRInput.Controller _controller, OVRInput.Button _button, Transform _hand, bool _isGrab)
    {
        

        //���� ����(��ġ - ��ġ = ������ ����)
        Vector3 throwDirection = _hand.position - prevPos;
        prevPos = _hand.position;

        Quaternion deltaRotation = _hand.rotation * Quaternion.Inverse(prevRot);
        //Inverse ��ŧ���� ����Ʈ ��Ʈ�ѷ��� ���ư� ������ ����ϴ°�
        prevRot = _hand.rotation;

        _hand.GetComponentInChildren<ICatch>().GetComponent<BoxCollider>().enabled = true;
        _hand.GetComponentInChildren<ICatch>().GetComponent<Rigidbody>().isKinematic = false;
        _hand.GetComponentInChildren<ICatch>().transform.parent = null;
        _hand.GetComponentInChildren<ICatch>().GetComponent<Rigidbody>().velocity = throwDirection * throwPower;

        //�� �ӵ� = (1/dt)* dCeta(Ư�� �� ���� ���� ����=����� ����)
        float angle;
        Vector3 axis;
        deltaRotation.ToAngleAxis(out angle, out axis);
        Vector3 angularVelocity = (1f / Time.deltaTime) * angle * axis;
        _hand.GetComponentInChildren<ICatch>().GetComponent<Rigidbody>().angularVelocity = angularVelocity * rotPower;

        _hand.GetComponentInChildren<ICatch>().GetComponent<Item>().SetIsGrabed(false);

        if (_hand == lHand)
        {
            mbLeftIsGrabbing = false;
            m_LeftGrabbedObject = null;
        }
        else
        {
            mbRightIsGrabbing = false;
            m_RightGrabbedObject = null;
        }
    }
    IEnumerator GrabbingAnimation(OVRInput.Controller _controller, Transform _hand)//��ü�� ������ ������
    {
        if (isDoingGrabbingAnimation == true)
        {
            yield return null;
        }
        else
        {
            m_GrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            prevPos = _hand.position;
            prevRot = _hand.rotation;

            Vector3 startLocation = m_GrabbedObject.transform.position;
            Vector3 targetLocation = prevPos + _hand.rotation * Vector3.forward * 0.1f;

            float currentTime = 0f;
            float finishTime = 0.2f;
            float elapsedRate = currentTime / finishTime;
            while (elapsedRate < 1)
            {
                currentTime += Time.deltaTime;
                elapsedRate = currentTime / finishTime;
                m_GrabbedObject.transform.position = Vector3.Lerp(startLocation, targetLocation, elapsedRate);

                yield return null;
            }
            m_GrabbedObject.transform.position = _hand.position;
            m_GrabbedObject.transform.parent = _hand;
            //grabbedObject.transform.localPosition += new Vector3(0f, 0f, 0.5f);
            //grabbedObject.GetComponent<BoxCollider>().enabled = true;   // ������ �տ� ����� �ݶ��̴� �Ѿ���.
            isDoingGrabbingAnimation = false;
        }
    }
   
    private void OnTriggerStay(Collider other)//�κ��丮
    {
        if (other.CompareTag("INVEN")/*||grabbedObject.CompareTag("ITEM")*/)
        {
            mbOnColl = true;
            if (mbLeftIsGrabbing == false && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            {//�޼� Ʈ���Ÿ� ���� �κ��丮 �ݶ��̴��� ������ ��
                Debug.Log("�޼� ������ ������");
                m_LeftGrabbedObject = other.GetComponent<ItemSlot>().OutputItem(lHand).gameObject;
                mbLeftIsGrabbing = true;
            }

            if (mbLeftIsGrabbing && OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
            {   //�޼� Ʈ���Ÿ� ���� ���𰡸� ��� �ְ�, �κ��丮 �ݶ��̴��� ������ ��

                Debug.Log("�޼� ������ �ֱ�");
                if (m_LeftGrabbedObject.GetComponent<Item>())
                {
                    Debug.Log("������");
                    other.GetComponent<ItemSlot>().InSlotItem = m_LeftGrabbedObject;
                    m_LeftGrabbedObject = null;
                    mbLeftIsGrabbing = false;
                }
            }
            
            if (mbRightIsGrabbing == false && OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))//������ Ʈ����
            {
                Debug.Log("������ ������ ������");
                m_RightGrabbedObject = other.GetComponent<ItemSlot>().OutputItem(rHand);
                mbRightIsGrabbing = true;
            }

            if (mbRightIsGrabbing && OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))//������ �κ����� ��������
            {
                if (m_RightGrabbedObject.GetComponent<Item>())
                {
                    Debug.Log("������2");
                    other.GetComponent<ItemSlot>().InSlotItem = m_RightGrabbedObject;
                    m_RightGrabbedObject = null;
                    mbRightIsGrabbing = false;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("INVEN"))
            mbOnColl = false;
    }
}