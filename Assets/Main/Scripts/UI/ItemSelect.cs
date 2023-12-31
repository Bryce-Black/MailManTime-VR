using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelect : MonoBehaviour
{
    public FirstPersonController fpController;
    private AmmoManagerSpawner ammoManagerSpawner;
    private bool _arrowNotched = false;
    private bool gameStarted = false;
    private GameObject hoverIndicator;
    private GameObject rightHandModelExample;
    public GameObject bowRightHandDrawExample;
    private void Start()
    {
        Debug.Log("Start() Grab *item* name is " + this.gameObject.name);
        ammoManagerSpawner = GameObject.FindAnyObjectByType<AmmoManagerSpawner>();
        //PullInteraction.PullActionReleased += NotchEmpty;
        hoverIndicator = GameObject.FindGameObjectWithTag("HoverIndicator");
    }
    public void GrabKey()
    {
        //fpController.UpdateKeyName(this.gameObject.name);
        //ammoManagerSpawner.SpawnSpecificItemInNotch(this.gameObject.name);
        //Debug.Log("Grab key name is " + this.gameObject.name);
        rightHandModelExample = GameObject.FindGameObjectWithTag("RightHandExample");

        if (rightHandModelExample != null)
        {
            rightHandModelExample.SetActive(false);
            bowRightHandDrawExample.SetActive(true);
        }
        bool Notched = ammoManagerSpawner.CheckForArrowInNotch();

        if (!Notched)
        {
            _arrowNotched = false;
        }
        else
        {
            _arrowNotched = true;
        }

        if (!_arrowNotched)
        {
            ammoManagerSpawner.SpawnSpecificItemInNotch(this.gameObject.name);
            
            Debug.Log("Grab key name is " + this.gameObject.name);
            _arrowNotched = true;
        }
        else
        {
            string modifiedNameWithoutClone;
            modifiedNameWithoutClone = ammoManagerSpawner._currentArrow.gameObject.name.Replace("(Clone)", "");
            Debug.Log("ammoMan arrow name: " + modifiedNameWithoutClone + " item gameobject name: " + this.gameObject.name);

            if (modifiedNameWithoutClone == this.gameObject.name)
            {
                //do nothing as it's the same key name
            }
            else
            {
                ammoManagerSpawner.ReplaceCurrentItemInNotch(this.gameObject.name);
                _arrowNotched = true;
            }

            
        }
    }

    public void GrabMail()
    {
        //fpController.UpdateKeyName(this.gameObject.name);
        //ammoManagerSpawner.SpawnSpecificItemInNotch(this.gameObject.name);
        //Debug.Log("Grab key name is " + this.gameObject.name);
        bool Notched = ammoManagerSpawner.CheckForArrowInNotch();

        if (!Notched)
        {
            _arrowNotched = false;
        }
        else
        {
            _arrowNotched = true;
        }



        if (!_arrowNotched)
        {
            ammoManagerSpawner.SpawnSpecificItemInNotch(this.gameObject.name);
            Debug.Log("Grab mail name is " + this.gameObject.name);
            _arrowNotched = true;
        }
        else
        {
            string modifiedNameWithoutClone;
            modifiedNameWithoutClone = ammoManagerSpawner._currentArrow.gameObject.name.Replace("(Clone)", "");
            Debug.Log("ammoMan arrow name: " + modifiedNameWithoutClone + " item gameobject name: " + this.gameObject.name);

            if (modifiedNameWithoutClone == this.gameObject.name)
            {
                //do nothing as it's the same key name
            }
            else
            {
                ammoManagerSpawner.ReplaceCurrentItemInNotch(this.gameObject.name);
                _arrowNotched = true;
            }
        }

        
    }
    //need to remove from on hover method
    //public void ChangeHoverIndicatorPosition()
    //{
    //    if(hoverIndicator != null && this.gameObject != null)
    //    {
    //        hoverIndicator.transform.position = this.gameObject.transform.position;
    //    }
        

    //}

    //private void NotchEmpty(float value)
    //{
    //    float holder;
    //    holder = value;
    //    _arrowNotched = false;
    //}
    //private void OnDestroy()
    //{
    //    PullInteraction.PullActionReleased -= NotchEmpty;
    //}
}
