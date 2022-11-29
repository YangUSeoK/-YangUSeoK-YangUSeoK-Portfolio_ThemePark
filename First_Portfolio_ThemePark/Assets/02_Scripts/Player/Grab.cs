using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//2022 11 08 김준우
public class Grab : MonoBehaviour
{
    public struct SInventory//인벤토리 구조체
    {
        public GameObject Inven;
        public int currItemIndex;
        public bool isGetItem;
    }
    public Transform lHand;
    public Transform rHand;

    [SerializeField] bool mbLeftIsGrabbing = false;     // 디버그
    [SerializeField] bool mbRightIsGrabbing = false;    
    bool mbOnColl=false;
    bool isDoingGrabbingAnimation = false;
    GameObject m_GrabbedObject = null;
    [SerializeField] GameObject m_RightGrabbedObject = null;    // 디버그
    [SerializeField] GameObject m_LeftGrabbedObject = null;
    public LayerMask grabbedLayer;
    public float grabRange = 0.5f;
    Item.EItemType eItemType;
    
    public float remoteGrabDistance = 5;
    [Header("아이템인덱스")]
    public GameObject[] ItemIndex;
    int currItemIndex;

    [Header("던지기 관련")]
    Vector3 prevPos;
    public float throwPower = 1f;
    Quaternion prevRot;
    public float rotPower = 1f;

    SInventory[] msLRInven;
    private void Start()
    {
        msLRInven = new SInventory[2];
        msLRInven[0].isGetItem = false;//이하 왼손
        msLRInven[1].isGetItem = false;//이하 오른손
    }
    void Update()
    {
        if (mbOnColl == false)
        {
            //22 11 09 김준우
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            {//왼손 트리거를 당기고 아무것도 들고 있지 않을 때
                TryLongGrab(OVRInput.Controller.LTouch, lHand);
            }
            if(OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
            {
                TryGrab(OVRInput.Controller.LTouch, lHand, mbLeftIsGrabbing);
            }
            else if (!mbOnColl && OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
            {   //왼손 트리거를 떼고 무언가를 들고 있을 때
                //던지거나 집는 과정에서 뭔 이상한 버그 생기면 여기서 예외조건 달아야 할 듯?
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
        if (Physics.SphereCast(ray, 0.5f/*(구체 반경)*/, out hitInfo, remoteGrabDistance, grabbedLayer))
        {
            if (_hand == lHand)
            {
                m_LeftGrabbedObject = hitInfo.transform.gameObject;
            }
            else
            {
                m_RightGrabbedObject = hitInfo.transform.gameObject;
            }
            //물체가 끌려오는 코루틴 함수 호출
            StartCoroutine(GrabbingAnimation(_controller, _hand));
            
        }
    }
    private void TryGrab(OVRInput.Controller _controller, Transform _hand, bool _isGrab)//잡기
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
            //앞 뒤 인덱스에 있는 오브젝트 거리 비교하여 다음 인덱스의 거리가 현재 저장된 최단거리보다 짧으면 인덱스 변경

            if (nextDistance < closestDistance)
                closest = i;
        }
        if (hitObjects.Length > 0)//쥐는게 결정되는 시점
        {
            //손에 쥐고 있을 때 물리기능 해제
            _hand.GetComponentInChildren<ICatch>().gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _hand.GetComponentInChildren<ICatch>().gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            _hand.GetComponentInChildren<ICatch>().gameObject.GetComponent<Rigidbody>().isKinematic = true;
            _hand.GetComponentInChildren<ICatch>().gameObject.GetComponent<BoxCollider>().enabled = false;


            //_hand.GetComponentInChildren<ICatch>().gameObject = hitObjects[closest].gameObject;
            _hand.GetComponentInChildren<ICatch>().gameObject.transform.SetParent(_hand.transform, false);
            _hand.GetComponentInChildren<ICatch>().gameObject.transform.position = _hand.position;//위치 설정
            //grabbedObject.transform.localPosition += new Vector3(0f, 0f, 0.5f);
            _hand.GetComponentInChildren<ICatch>().gameObject.transform.rotation = _hand.rotation;
            //던지기 위해 쥐고 나서 현재위치 설정
            prevPos = OVRInput.GetLocalControllerPosition(_controller);
            prevRot = _hand.rotation;

            //아이템 그랩 여부 알려줌
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
        

        //던질 방향(위치 - 위치 = 던지는 방향)
        Vector3 throwDirection = _hand.position - prevPos;
        prevPos = _hand.position;

        Quaternion deltaRotation = _hand.rotation * Quaternion.Inverse(prevRot);
        //Inverse 오큘러스 퀘스트 컨트롤러가 돌아간 각도를 계산하는거
        prevRot = _hand.rotation;

        _hand.GetComponentInChildren<ICatch>().GetComponent<BoxCollider>().enabled = true;
        _hand.GetComponentInChildren<ICatch>().GetComponent<Rigidbody>().isKinematic = false;
        _hand.GetComponentInChildren<ICatch>().transform.parent = null;
        _hand.GetComponentInChildren<ICatch>().GetComponent<Rigidbody>().velocity = throwDirection * throwPower;

        //각 속도 = (1/dt)* dCeta(특정 축 기준 변위 각도=변경된 각도)
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
    IEnumerator GrabbingAnimation(OVRInput.Controller _controller, Transform _hand)//물체가 손으로 끌려옴
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
            //grabbedObject.GetComponent<BoxCollider>().enabled = true;   // 손전등 손에 들고나면 콜라이더 켜야함.
            isDoingGrabbingAnimation = false;
        }
    }
   
    private void OnTriggerStay(Collider other)//인벤토리
    {
        if (other.CompareTag("INVEN")/*||grabbedObject.CompareTag("ITEM")*/)
        {
            mbOnColl = true;
            if (mbLeftIsGrabbing == false && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            {//왼손 트리거를 당기고 인벤토리 콜라이더랑 겹쳤을 때
                Debug.Log("왼손 아이템 꺼내기");
                m_LeftGrabbedObject = other.GetComponent<ItemSlot>().OutputItem(lHand).gameObject;
                mbLeftIsGrabbing = true;
            }

            if (mbLeftIsGrabbing && OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
            {   //왼손 트리거를 떼고 무언가를 들고 있고, 인벤토리 콜라이더랑 겹쳤을 때

                Debug.Log("왼손 아이템 넣기");
                if (m_LeftGrabbedObject.GetComponent<Item>())
                {
                    Debug.Log("넣을게");
                    other.GetComponent<ItemSlot>().InSlotItem = m_LeftGrabbedObject;
                    m_LeftGrabbedObject = null;
                    mbLeftIsGrabbing = false;
                }
            }
            
            if (mbRightIsGrabbing == false && OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))//오른손 트리거
            {
                Debug.Log("오른손 아이템 꺼내기");
                m_RightGrabbedObject = other.GetComponent<ItemSlot>().OutputItem(rHand);
                mbRightIsGrabbing = true;
            }

            if (mbRightIsGrabbing && OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))//오른손 인벤에서 가져오기
            {
                if (m_RightGrabbedObject.GetComponent<Item>())
                {
                    Debug.Log("넣을게2");
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