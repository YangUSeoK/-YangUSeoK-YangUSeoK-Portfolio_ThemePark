using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform PlayerCamera;
    [Header("MaxDist to door.")]
    public float MaxDistance = 3;

    private bool Opened = false;
    private Animator anim;



    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.F))
        {
            Pressed();

        }
    }

    void Pressed()
    {
        //This will name the Raycasthit and came information of which object the raycast hit.
        RaycastHit doorhit;

        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out doorhit, MaxDistance))
        {

            // if raycast hits, then it checks if it hit an object with the tag Door.
            if (doorhit.transform.tag == "DOOR")
            {

                //This line will get the Animator from  Parent of the door that was hit by the raycast.
                anim = doorhit.transform.GetComponentInParent<Animator>();

                //This will set the bool the opposite of what it is.
                Opened = !Opened;

                //This line will set the bool true so it will play the animation.
                anim.SetBool("Opened", !Opened);
            }
        }
    }
}
