using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class HandAnimator : MonoBehaviour
{

    public InputActionProperty rightTriggerPull;
    public InputActionProperty rightSqueezePull;
    public InputActionProperty rightJoystickAxis;

    public InputActionProperty leftTriggerPull;
    public InputActionProperty leftSqueezePull;
    public InputActionProperty leftJoystickAxis;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    private bool rightTriggerSqueezed = false, rightGrabSqueezed = false, leftGrabSqueezed = false;
    // Update is called once per frame
    void Update()
    {
        float rightTriggerValue = rightTriggerPull.action.ReadValue<float>();
        //float leftTriggerValue = leftTriggerPull.action.ReadValue<float>();
        if (rightTriggerValue > 0 && !rightTriggerSqueezed)
        {
            rightHandAnimator.SetFloat("Trigger", 1);
            rightTriggerSqueezed = true;
        }
        if (rightTriggerValue == 0)
        {
            rightHandAnimator.SetFloat("Trigger", 0);
            rightTriggerSqueezed = false;
        }

        float rightGrabValuetemp = rightSqueezePull.action.ReadValue<float>();
        if (rightGrabValuetemp > 0 && !rightGrabSqueezed)
        {
            rightHandAnimator.SetFloat("Grip", 1);
            rightTriggerSqueezed = true;
        }
        if (rightGrabValuetemp == 0)
        {
            rightHandAnimator.SetFloat("Grip", 0);
            rightTriggerSqueezed = false;
        }

        float leftGrabValuetemp = leftSqueezePull.action.ReadValue<float>();
        if (leftGrabValuetemp > 0 && !leftGrabSqueezed)
        {
            leftHandAnimator.SetFloat("Grip", 1);
            leftGrabSqueezed = true;
        }
        if (leftGrabValuetemp == 0)
        {
            leftHandAnimator.SetFloat("Grip", 0);
            leftGrabSqueezed = false;
        }
    }
}
