using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    MailBoxContoller mailBoxController;

    FirstPersonController firstPersonController;
    private IEnumerator delayDestroy;
    private bool failedKey = false;
    public AmmoManagerSpawner ammoManagerSpawner;
    private KeyScript kScript;

    private void Start()
    {
        mailBoxController = GameObject.FindGameObjectWithTag("MailBoxController").GetComponent<MailBoxContoller>();
        firstPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        PullInteraction.PullActionReleased += BeginKeyDestroyCountDown;
        kScript = GetComponent<KeyScript>();
        delayDestroy = DelayDestroo(2.2f);
    }

    public void BeginKeyDestroyCountDown(float pullStrength)
    {
        if(kScript != null)
        {
            float swag = pullStrength;
            StartCoroutine(delayDestroy);
        }
        
    }

    private IEnumerator DelayDestroo(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //mailBoxController.MailHasFailed();
        firstPersonController.ScreenInfoActivate("Miss!");
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Boundry")
        {
            failedKey = true;
            firstPersonController.ScreenInfoActivate("Miss!");
            if (delayDestroy != null)
            {
                StopCoroutine(delayDestroy);

            }
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == this.gameObject.tag && !failedKey)
        {
            mailBoxController.KeyHasUnlockedBox();
            if (delayDestroy != null)
            {
                StopCoroutine(delayDestroy);

            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == this.gameObject.tag && !failedKey)
        {
            mailBoxController = GameObject.FindGameObjectWithTag("MailBoxController").GetComponent<MailBoxContoller>();

            Debug.Log("mbconttoller is: " + mailBoxController);
            Debug.Log("key entered trigger of: " + other.gameObject.name);
            mailBoxController.KeyHasUnlockedBox();
            if (delayDestroy != null)
            {
                StopCoroutine(delayDestroy);

            }
            Destroy(this.gameObject);
        }
        else
        {
            if (other.gameObject.tag == "Boundry")
            {
                failedKey = true;
                firstPersonController.ScreenInfoActivate("Miss!");
                if (delayDestroy != null)
                {
                    StopCoroutine(delayDestroy);

                }
                Destroy(this.gameObject);
            }
        }
    }

}

