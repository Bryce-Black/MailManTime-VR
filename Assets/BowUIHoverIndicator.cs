using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowUIHoverIndicator : MonoBehaviour
{
    public List<GameObject> keysList;
    public List<GameObject> mailList;
    public GameObject hoverIndicator;
    public void ChangeHoverKey(int index)
    {
        hoverIndicator.transform.position = keysList[index].gameObject.transform.position;

    }

    public void ChangeHoverMail(int index)
    {
        hoverIndicator.transform.position = mailList[index].gameObject.transform.position;

    }
}
