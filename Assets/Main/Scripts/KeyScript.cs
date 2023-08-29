using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    MailBoxContoller mailBoxController;

    FirstPersonController firstPersonController;
    private IEnumerator delayDestroy;
    private bool failedKey = false;
    private AmmoManagerSpawner ammoManagerSpawner;

    private void Start()
    {
        ammoManagerSpawner = GameObject.FindGameObjectWithTag("Bow").GetComponent<AmmoManagerSpawner>();
        mailBoxController = GameObject.FindGameObjectWithTag("MailBoxController").GetComponent<MailBoxContoller>();
        firstPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        delayDestroy = DelayDestroo(5f);
        StartCoroutine(delayDestroy);
    }

    private IEnumerator DelayDestroo(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        MissedShot();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Boundry")
        {
            MissedShot();
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
            //if (other.gameObject.tag == "Boundry")
            //{
            //    failedKey = true;
            //    //mailBoxController.MailHasFailed();
            //    firstPersonController.ScreenInfoActivate("Miss!");
            //    Destroy(this.gameObject);
            //}
        }
    }

    private void MissedShot()
    {
        failedKey = true;
        firstPersonController.ScreenInfoActivate("Miss!");
        ammoManagerSpawner.SpawnSpecificItemInNotch(this.gameObject.name);
        Debug.Log("Arrow destroyed name is: " + this.gameObject.name);
        Destroy(this.gameObject);
    }

}

