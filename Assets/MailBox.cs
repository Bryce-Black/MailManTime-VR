using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBox : MonoBehaviour
{
    private MailBoxContoller mbController;
    public void MailBoxDoneSpawningEvent()
    {
        mbController = GameObject.FindGameObjectWithTag("MailBoxController").GetComponent<MailBoxContoller>();
        mbController.MailBoxHasFinishedSpawning();
    }
}
