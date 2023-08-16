using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ControllerInputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButton("Oculus_XR_RTouch_AButton"))
        {
            Debug.Log("A button (One) pressed on the right hand controller.");
        }

        if (Input.GetButton("Oculus_XR_RTouch_BButton"))
        {
            Debug.Log("B button (Two) pressed on the right hand controller.");
        }

        if (Input.GetButton("Oculus_XR_RTouch_XButton"))
        {
            Debug.Log("X button (Three) pressed on the right hand controller.");
        }

        if (Input.GetButton("Oculus_XR_RTouch_YButton"))
        {
            Debug.Log("Y button (Four) pressed on the right hand controller.");
        }

        if (Input.GetButton("Oculus_XR_RTouch_StartButton"))
        {
            Debug.Log("Start button pressed on the right hand controller.");
        }

        if (Input.GetButton("Oculus_XR_RTouch_ThumbstickButton"))
        {
            Debug.Log("Thumbstick button pressed on the right hand controller.");
        }

        if (Input.GetButton("Oculus_XR_RTouch_IndexTrigger"))
        {
            Debug.Log("Index trigger pressed on the right hand controller.");
        }

        if (Input.GetButton("Oculus_XR_RTouch_HandTrigger"))
        {
            Debug.Log("Hand trigger pressed on the right hand controller.");
        }

        #region LeftHand
        if (Input.GetButton("Oculus_XR_LTouch_AButton"))
        {
            Debug.Log("A button (One) pressed on the left hand controller.");
        }

        if (Input.GetButton("Oculus_XR_LTouch_BButton"))
        {
            Debug.Log("B button (Two) pressed on the left hand controller.");
        }

        if (Input.GetButton("Oculus_XR_LTouch_XButton"))
        {
            Debug.Log("X button (Three) pressed on the left hand controller.");
        }

        if (Input.GetButton("Oculus_XR_LTouch_YButton"))
        {
            Debug.Log("Y button (Four) pressed on the left hand controller.");
        }

        if (Input.GetButton("Oculus_XR_LTouch_StartButton"))
        {
            Debug.Log("Start button pressed on the left hand controller.");
        }

        if (Input.GetButton("Oculus_XR_LTouch_ThumbstickButton"))
        {
            Debug.Log("Thumbstick button pressed on the left hand controller.");
        }

        if (Input.GetButton("Oculus_XR_LTouch_IndexTrigger"))
        {
            Debug.Log("Index trigger pressed on the left hand controller.");
        }

        if (Input.GetButton("Oculus_XR_LTouch_HandTrigger"))
        {
            Debug.Log("Hand trigger pressed on the left hand controller.");
        }
        #endregion LeftHand
    }
}