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

    private void Start()
    {
        mailBoxController = GameObject.FindGameObjectWithTag("MailBoxController").GetComponent<MailBoxContoller>();
        firstPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        delayDestroy = DelayDestroo(2.2f);
        StartCoroutine(delayDestroy);
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
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == this.gameObject.tag && !failedKey)
        {
            mailBoxController.KeyHasUnlockedBox();
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
            Destroy(this.gameObject);
        }
        else
        {
            if (other.gameObject.tag == "Boundry")
            {
                failedKey = true;
                firstPersonController.ScreenInfoActivate("Miss!");
                Destroy(this.gameObject);
            }
        }
    }

}

