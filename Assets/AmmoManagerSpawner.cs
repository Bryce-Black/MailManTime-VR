using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AmmoManagerSpawner : MonoBehaviour
{
    public GameObject arrow;
    public GameObject notch;

    private XRGrabInteractable _bow;
    public bool _arrowNotched = false;
    private GameObject _currentArrow = null;
    public Transform _holsterPosition;
    public GameObject _bowGameObject;

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
    }
    private void NotchEmpty(float value)
    {
        _arrowNotched = false;
        _currentArrow = null;
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(1f);
        _currentArrow = Instantiate(arrow, notch.transform);
    }

    public void SpawnSpecificItemInNotch(string itemName)
    {
        Debug.Log("attempting to instantiate on notch: " + itemName);
        _currentArrow = Instantiate(Resources.Load<GameObject>(itemName), notch.transform);
    }

    public void AttachBowToHip()
    {
        Debug.Log("bow has been let go of, to the holster!");
        _bowGameObject.transform.SetParent(_holsterPosition);
        _bowGameObject.transform.position = _holsterPosition.transform.position;
        _bowGameObject.transform.rotation = _holsterPosition.transform.localRotation;
    }
}
