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
        hoverIndicator.gameObject.transform.SetParent(keysList[index].transform);
        hoverIndicator.transform.localPosition = new Vector3(0, 0, 0);
        //hoverIndicator.transform.position = keysList[index].gameObject.transform.position;

    }

    public void ChangeHoverMail(int index)
    {
        hoverIndicator.gameObject.transform.SetParent(mailList[index].transform);
        hoverIndicator.transform.localPosition = new Vector3(0, 0, 0);

    }

   
}
