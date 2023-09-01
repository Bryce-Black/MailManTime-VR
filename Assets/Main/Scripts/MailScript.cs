using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailScript : MonoBehaviour
{
    public MailScriptableObject mailScript;
    private bool pointsGiven = false;

    private string MailName;
    private float MailMass;
    private float MailSpeed;
    private int MailPoints;
    MailBoxContoller mailBoxController;

    FirstPersonController firstPersonController;
    private Rigidbody rb;
    private IEnumerator delayDestroy;
    private MailScript mScript;
    private void Start()
    {
        mailBoxController = GameObject.FindGameObjectWithTag("MailBoxController").GetComponent<MailBoxContoller>();
        MailName = mailScript.mailName;
        MailMass = mailScript.mailMass;
        MailSpeed = mailScript.mailSpeed;
        MailPoints = mailScript.mailPoints;
        firstPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        firstPersonController.ChangeMailInfo(MailName, MailSpeed);
        rb = GetComponent<Rigidbody>();
        Vector3 spinDirection = transform.up;
        rb.AddTorque(spinDirection * 10f);
        PullInteraction.PullActionReleased += BeginMailDestroyCountDown;
        mScript = GetComponent<MailScript>();
    }

    public void BeginMailDestroyCountDown(float pullStrength)
    {
        if(mScript != null)
        {
            float swag = pullStrength;
            delayDestroy = DelayDestroo(2.2f);
            StartCoroutine(delayDestroy);
        }
        
    }
    private IEnumerator DelayDestroo(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        mailBoxController.MailHasFailed();
        if(delayDestroy != null)
        {
            StopCoroutine(delayDestroy);

        }
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Boundry")
        {
            mailBoxController.MailHasFailed();
            if (delayDestroy != null)
            {
                StopCoroutine(delayDestroy);

            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == this.gameObject.tag)
        {
            mailBoxController.MailHasBeenDelivered(MailPoints);
            Debug.Log("SCORE! +" + MailPoints);
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
                mailBoxController.MailHasFailed();
                if (delayDestroy != null)
                {
                    StopCoroutine(delayDestroy);

                }
                Destroy(this.gameObject);
            }
        }
    }

}
