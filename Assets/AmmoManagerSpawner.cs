using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AmmoManagerSpawner : MonoBehaviour
{
    public GameObject arrow;
    public GameObject notch;

    private XRGrabInteractable _bow;
    public bool _arrowNotched = false;
    public GameObject _currentArrow = null;

    public GameObject holster;
    public GameObject _BowAndArrowGameObject;
    private bool bowInHand = false;
    // Start is called before the first frame update
    void Start()
    {
        _bow = GetComponent<XRGrabInteractable>();
        PullInteraction.PullActionReleased += NotchEmpty;
    
    }
    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= NotchEmpty;
    }

    // Update is called once per frame
    void Update()
    {
        if(_bow.isSelected && _arrowNotched == false)
        {
            _arrowNotched = true;
            //StartCoroutine("DelayedSpawn");

        }
        if (!_bow.isSelected && _currentArrow != null)
        {
            Destroy(_currentArrow);
            NotchEmpty(1f);
        }
        if(!bowInHand)
        {
            _BowAndArrowGameObject.gameObject.transform.SetParent(holster.transform);
            _BowAndArrowGameObject.transform.localPosition = new Vector3(0, 0, 0);
            
        }
    }
    private void NotchEmpty(float value)
    {
        _arrowNotched = false;
        _currentArrow = null;
    }

    public void SpawnSpecificItemInNotch(string itemName)
    {
        Debug.Log("attempting to instantiate on notch: " + itemName);
        _currentArrow = Instantiate(Resources.Load<GameObject>(itemName), notch.transform);
    }

    public void ReholsterBow()
    {
        //_BowAndArrowGameObject.gameObject.transform.position = holster.gameObject.transform.position;
        _BowAndArrowGameObject.transform.localRotation = holster.transform.rotation;
        bowInHand = false;

    }

    public void GrabBow()
    {
        _BowAndArrowGameObject.transform.SetParent(null);
        bowInHand = true;
    }

    public void ReplaceCurrentItemInNotch(string itemName)
    {
        Destroy(_currentArrow);
        if(_currentArrow == null)
        {
            _currentArrow = Instantiate(Resources.Load<GameObject>(itemName), notch.transform);

        }
    }

}
