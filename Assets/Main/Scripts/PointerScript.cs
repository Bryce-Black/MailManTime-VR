using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerScript : MonoBehaviour
{
    private Transform targetPosition;
    public GameObject pointerObject;
    private bool objectActivate = false;
    // Update is called once per frame
    void Update()
    {
        if(targetPosition != null)
        {
            if(!objectActivate)
            {
                pointerObject.SetActive(true);
                objectActivate = true;
            }
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition.position - transform.position),
            100f * Time.deltaTime);
        }
        else
        {
            objectActivate = false;
            pointerObject.SetActive(false);
        }
        
    }
    public void UpdateTargetPosition(Transform newPos)
    {
        targetPosition = newPos;
    }
}
