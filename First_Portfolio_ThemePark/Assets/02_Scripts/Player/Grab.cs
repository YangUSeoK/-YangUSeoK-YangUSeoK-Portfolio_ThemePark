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

    bool mbLeftIsGrabbing = false;
    bool mbRightIsGrabbing = false;
    bool mbOnColl=false;
    bool isDoingGrabbingAnimation = false;
    GameObject grabbedObject;
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
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
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
        if (Physics.SphereCast(ray, 0.5f/*(구체 반경)*/, out hitInfo, remoteGrabDistance, grabbedLayer))
        {
            grabbedObject = hitInfo.transform.gameObject;
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
            grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObject.GetComponent<BoxCollider>().enabled = false;


            grabbedObject = hitObjects[closest].gameObject;
            grabbedObject.transform.SetParent(_hand.transform, false);
            grabbedObject.transform.position = _hand.position;//위치 설정
            //grabbedObject.transform.localPosition += new Vector3(0f, 0f, 0.5f);
            grabbedObject.transform.rotation = _hand.rotation;
            //던지기 위해 쥐고 나서 현재위치 설정
            prevPos = OVRInput.GetLocalControllerPosition(_controller);
            prevRot = _hand.rotation;

            //아이템 그랩 여부 알려줌
            _isGrab = true;
            grabbedObject.GetComponent<Item>().SetIsGrabed(true);
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    private void TryUnGrab(OVRInput.Controller _controller, OVRInput.Button _button, Transform _hand, bool _isGrab)
    {
        Debug.Log("그랩이 안먹는거 같은데?");
        //던질 방향(위치 - 위치 = 던지는 방향)
        Vector3 throwDirection = _hand.position - prevPos;
        prevPos = _hand.position;

        Quaternion deltaRotation = _hand.rotation * Quaternion.Inverse(prevRot);
        //Inverse 오큘러스 퀘스트 컨트롤러가 돌아간 각도를 계산하는거
        prevRot = _hand.rotation;

        grabbedObject.GetComponent<BoxCollider>().enabled = true;
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        grabbedObject.transform.parent = null;
        grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;

        //각 속도 = (1/dt)* dCeta(특정 축 기준 변위 각도=변경된 각도)
        float angle;
        Vector3 axis;
        deltaRotation.ToAngleAxis(out angle, out axis);
        Vector3 angularVelocity = (1f / Time.deltaTime) * angle * axis;
        grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity * rotPower;

        grabbedObject.GetComponent<Item>().SetIsGrabed(false);
        grabbedObject = null;
        _isGrab = false;
    }
    IEnumerator GrabbingAnimation(OVRInput.Controller _controller, Transform _hand)//물체가 손으로 끌려옴
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
            //grabbedObject.GetComponent<BoxCollider>().enabled = true;   // 손전등 손에 들고나면 콜라이더 켜야함.
            isDoingGrabbingAnimation = false;
        }
    }
    void InvenSave(SInventory _LRInven, bool _isGrab)//2022 11 25 김준우
    {
        //여기에 인벤토리에 아이템 인덱스 넣고 디스트로이
        if (grabbedObject.GetComponent<Item>() != null)//22 11 28 김준우
        {//그랩 오브젝트에 아이템컴포넌트가 있는지 검사
            if (grabbedObject.Equals(Item.EItemType.Flashlight))//아이템 컴포넌트의 EItemType으로 분류
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
        else return;//아이템 컴포넌트가 없다면 그냥 종료
        grabbedObject = null;
        Destroy(grabbedObject);
        _isGrab = true;
    }
    void GetInvenItem(SInventory _LRInven, Transform _hand, bool _isGrab)
    {
        if (_isGrab == false)
        {
            //여기에 넣은 아이템 인덱스로 인스턴시에이트 하는 기능
            grabbedObject = Instantiate(ItemIndex[currItemIndex], _hand);
            _isGrab = true;
        }
    }
    private void OnTriggerStay(Collider other)//인벤토리
    {
        if (other.CompareTag("INVEN")/*||grabbedObject.CompareTag("ITEM")*/)
        {
            mbOnColl = true;
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) && mbLeftIsGrabbing == false)
            {//왼손 트리거를 당기고 인벤토리 콜라이더랑 겹쳤을 때
                GetInvenItem(msLRInven[0], lHand, mbLeftIsGrabbing);
            }
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
            {   //왼손 트리거를 떼고 무언가를 들고 있고, 인벤토리 콜라이더랑 겹쳤을 때
                InvenSave(msLRInven[0], mbLeftIsGrabbing);
            }
            if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))//오른손 트리거
            {
                GetInvenItem(msLRInven[1], rHand, mbRightIsGrabbing);
            }
            else if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))//오른손 인벤에서 가져오기
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