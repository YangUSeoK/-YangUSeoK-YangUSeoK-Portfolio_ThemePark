using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//2022 11 08 김준우
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

    [Header("던지기 관련")]
    Vector3 prevPos;
    public float throwPower = 1f;
    Quaternion prevRot;
    public float rotPower = 1f;

    void Update()
    {
        //22 11 09 김준우
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {//왼손 트리거를 당기고 아무것도 들고 있지 않을 때
            TryGrab(OVRInput.Controller.LTouch, lHand, mbLeftIsGrabbing);
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
        {   //왼손 트리거를 떼고 무언가를 들고 있을 때
            //던지거나 집는 과정에서 뭔 이상한 버그 생기면 여기서 예외조건 달아야 할 듯?
            Debug.Log("뭐임?");
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
    private void TryGrab(OVRInput.Controller _controller,Transform _hand,bool _isGrab)//잡기
    {
        if(_isGrab==true)
        {
            //_isGrab = false;
            return;
        }
        //레이 쏘기
        Ray ray = new Ray(_hand.position, _hand.rotation * Vector3.forward);
        RaycastHit hitInfo;
        if (Physics.SphereCast(ray, 0.5f/*(구체 반경)*/, out hitInfo, remoteGrabDistance, grabbedLayer))
        {
            grabbedObject = hitInfo.transform.gameObject;
            //물체가 끌려오는 코루틴 함수 호출
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
            //앞 뒤 인덱스에 있는 오브젝트 거리 비교하여 다음 인덱스의 거리가 현재 저장된 최단거리보다 짧으면 인덱스 변경

            if (nextDistance < closestDistance)
                closest = i;
        }
        if (hitObjects.Length > 0)//쥐는게 결정되는 시점
        {
            grabbedObject = hitObjects[closest].gameObject;
            grabbedObject.transform.SetParent(_hand.transform, false);
            grabbedObject.transform.position = _hand.position;
            grabbedObject.transform.rotation = _hand.rotation;
            //손에 쥐고 있을 때 물리기능 해제
            grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            // 쥐고 나서 초기위치 설정
            prevPos = OVRInput.GetLocalControllerPosition(_controller);
            prevRot = _hand.rotation;

            // 아이템 그랩 알려줌
            grabbedObject.GetComponent<Item>().SetIsGrabed(true);
        }
    }
    private void TryUnGrab(OVRInput.Controller _controller, OVRInput.Button _button, Transform _hand,bool _isGrab)
    {
        Debug.Log("대체 뭐임?");
        //던질 방향(위치 - 위치 = 던지는 방향)
        Vector3 throwDirection = _hand.position - prevPos;
        prevPos = _hand.position;

        Quaternion deltaRotation = _hand.rotation * Quaternion.Inverse(prevRot);
        //Inverse 오큘러스 퀘스트 컨트롤러가 돌아간 각도를 계산하는거
        prevRot = _hand.rotation;
        
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        grabbedObject.transform.parent = null;
        grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;

        //각 속도 = (1/dt)* dCeta(특정 축 기준 변위 각도=변경된 각도)
        float angle;
        Vector3 axis;
        deltaRotation.ToAngleAxis(out angle, out axis);
        Vector3 angularVelocity = (1f / Time.deltaTime) * angle * axis;
        grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity * rotPower;

        grabbedObject = null;
        _isGrab = false;
    }
    IEnumerator GrabbingAnimation(OVRInput.Controller _controller, Transform _hand)//물체가 손으로 끌려옴
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
