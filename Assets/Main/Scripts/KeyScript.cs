using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    MailBoxContoller mailBoxController;

    FirstPersonController firstPersonController;
    private IEnumerator delayDestroy;
    private bool failedKey = false;
    private void Start()
    {
        mailBoxController = GameObject.FindGameObjectWithTag("MailBoxController").GetComponent<MailBoxContoller>();
        firstPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        delayDestroy = DelayDestroo(1f);
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
            //mailBoxController.MailHasFailed();
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
            mailBoxController.KeyHasUnlockedBox();
            Destroy(this.gameObject);
        }
        else
        {
            //failedKey = true;
            //mailBoxController.KeyHasFailed();
            //Destroy(this.gameObject);
        }
    }

}

