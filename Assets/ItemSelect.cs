using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelect : MonoBehaviour
{
    public FirstPersonController fpController;
    private AmmoManagerSpawner ammoManagerSpawner;
    private void Start()
    {
        //Debug.Log("Grab *item* name is " + this.gameObject.name);
        ammoManagerSpawner = GameObject.FindAnyObjectByType<AmmoManagerSpawner>();
    }
    public void GrabKey()
    {
        fpController.UpdateKeyName(this.gameObject.name);
        ammoManagerSpawner.SpawnSpecificItemInNotch(this.gameObject.name);
        Debug.Log("Grab key name is " + this.gameObject.name);
    }

    public void GrabMail()
    {
        fpController.UpdateMailName(this.gameObject.name);
        ammoManagerSpawner.SpawnSpecificItemInNotch(this.gameObject.name);
        Debug.Log("Grab mail name is " + this.gameObject.name);
    }
}
