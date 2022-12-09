using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailDoor : MonoBehaviour
{
    //Rigidbody m_DoorRigid;
    Collider m_DoorCollider;
    Animation m_DoorAnim;
    bool mbIsOpen = false;

    void Start()
    {
        //m_DoorRigid = GetComponent<Rigidbody>();
        m_DoorCollider = GetComponent<Collider>();
        m_DoorAnim = GetComponent<Animation>();
    }

    public IEnumerator OpenDoor()
    {
        if (mbIsOpen == false)
        {
            m_DoorCollider.enabled = false;
            m_DoorAnim.Play("DoorAnim");

            yield return new WaitForSeconds(m_DoorAnim.clip.length);

            m_DoorAnim.Stop();
            m_DoorCollider.enabled = true;
            mbIsOpen = true;
        }
        yield return null;
    }
}
