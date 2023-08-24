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
    private void Start()
    {
        Debug.Log("Start() Grab *item* name is " + this.gameObject.name);
        ammoManagerSpawner = GameObject.FindAnyObjectByType<AmmoManagerSpawner>();
        PullInteraction.PullActionReleased += NotchEmpty;
        hoverIndicator = GameObject.FindGameObjectWithTag("HoverIndicator");
    }
    public void GrabKey()
    {
        //fpController.UpdateKeyName(this.gameObject.name);
        //ammoManagerSpawner.SpawnSpecificItemInNotch(this.gameObject.name);
        //Debug.Log("Grab key name is " + this.gameObject.name);
        
        if (!_arrowNotched)
        {
            fpController.UpdateKeyName(this.gameObject.name);
            ammoManagerSpawner.SpawnSpecificItemInNotch(this.gameObject.name);
            Debug.Log("Grab key name is " + this.gameObject.name);
            _arrowNotched = true;
        }
    }

    public void GrabMail()
    {
        //fpController.UpdateKeyName(this.gameObject.name);
        //ammoManagerSpawner.SpawnSpecificItemInNotch(this.gameObject.name);
        //Debug.Log("Grab key name is " + this.gameObject.name);
        
        if (!_arrowNotched)
        {
            fpController.UpdateMailName(this.gameObject.name);
            ammoManagerSpawner.SpawnSpecificItemInNotch(this.gameObject.name);
            Debug.Log("Grab mail name is " + this.gameObject.name);
            _arrowNotched = true;
        }
        
    }
    public void ChangeHoverIndicatorPosition()
    {
        if(hoverIndicator != null && this.gameObject != null)
        {
            hoverIndicator.transform.position = this.gameObject.transform.position;
        }
        

    }

    private void NotchEmpty(float value)
    {
        float holder;
        holder = value;
        _arrowNotched = false;
    }
    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= NotchEmpty;
    }
}
