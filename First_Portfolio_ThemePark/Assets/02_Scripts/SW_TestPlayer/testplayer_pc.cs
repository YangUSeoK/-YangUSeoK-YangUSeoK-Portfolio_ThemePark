using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testplayer_pc : MonoBehaviour
{

    public Controls controls;

    Vector2 inputs;
    float rotation;
    Vector3 velocity;

    public float baseSpeed = 2.0f, runSpeed = 6.0f, rotateSpeed = 0.4f;

    [SerializeField]
    bool isRun = true;

    CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        GetInputs();
        Locomotion();
    }

    void Locomotion()
    {
        Vector2 inputNormalized = inputs;
        //rotating
        Vector3 characterRotation = transform.eulerAngles + new Vector3(0, rotation * rotateSpeed, 0);
        transform.eulerAngles = characterRotation;
        //running and walknig
        float currSpeed = baseSpeed;

        if (isRun)
        {
            currSpeed *= runSpeed;
            if (inputNormalized.y < 0)
                currSpeed = currSpeed / 2;
        }

        //applying input
        velocity = (transform.forward * inputNormalized.y + transform.right * inputNormalized.x) * currSpeed;
        //moving controller
        controller.Move(velocity * Time.deltaTime);
    }
    void GetInputs()
    {
        //FORWARDS BACKWARDS
        //forwards
        if (Input.GetKey(controls.forwards))
            inputs.y = 1;
        //backwards
        if (Input.GetKey(controls.backwards))
        {
            if (Input.GetKey(controls.forwards))
                inputs.y = 0;
            else
                inputs.y = -1;
        }
        //FW nothing
        if (!Input.GetKey(controls.forwards) && !Input.GetKey(controls.backwards))
            inputs.y = 0;

        //STRAFE LEFT, RIGHT
        //strafe left
        if (Input.GetKey(controls.strafeRight))
            inputs.x = 1;
        //strafe right
        if (Input.GetKey(controls.strafeLeft))
        {
            if (Input.GetKey(controls.strafeRight))
                inputs.x = 0;
            else
                inputs.x = -1;
        }
        //strafe LR nothing
        if (!Input.GetKey(controls.strafeLeft) && !Input.GetKey(controls.strafeRight))
            inputs.x = 0;

        //ROTATE LEFT RIGHT
        //rotate left
        if (Input.GetKey(controls.rotateRight))
            rotation = 1;
        //rotate right
        if (Input.GetKey(controls.rotateLeft))
        {
            if (Input.GetKey(controls.rotateRight))
                rotation = 0;
            else
                rotation = -1;
        }
        //rotate LR nothing
        if (!Input.GetKey(controls.rotateLeft) && !Input.GetKey(controls.rotateRight))
            rotation = 0;

        //toggle
        if (Input.GetKeyDown(controls.walkRun))
            isRun = !isRun;
    }

}

