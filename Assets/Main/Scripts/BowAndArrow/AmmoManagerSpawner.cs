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
    public UIManager uIManager;
    public MailBoxContoller mbController;
    private bool gameStarted = false;
    public GameObject exampleLeftHandModelSqueeze;
    public GameObject exampleRightHandModelSqueeze;
    public GameObject rightHandBowDrawExample;
    public FirstPersonController firstPersonController;
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



    public void ReholsterBow()
    {
        //_BowAndArrowGameObject.gameObject.transform.position = holster.gameObject.transform.position;
        _BowAndArrowGameObject.transform.localRotation = holster.transform.localRotation;
        bowInHand = false;

        //uIManager.SetRayInteractorAlphaValue(0f, true);
    }

    public void GrabBow()
    {
        _BowAndArrowGameObject.transform.SetParent(null);
        bowInHand = true;
        if(!gameStarted)
        {
            mbController.BeginTheGame();
            exampleLeftHandModelSqueeze.SetActive(false);
            exampleRightHandModelSqueeze.SetActive(true);
            gameStarted = true;
        }
        //set gradient alpha to 1 to activate laser pointer for bow
        uIManager.DisableRayInteractors(false);
        //uIManager.SetRayInteractorAlphaValue(1f, true);
        firstPersonController.BowHasBeenGrabbed();
    }

    public void SpawnSpecificItemInNotch(string itemName)
    {
        Debug.Log("attempting to instantiate on notch: " + itemName);
        AudioSource source = GameObject.FindGameObjectWithTag("BowLoad").GetComponent<AudioSource>();
        source.Play();
        _currentArrow = Instantiate(Resources.Load<GameObject>(itemName), notch.transform);
    }

    public void ReplaceCurrentItemInNotch(string itemName)
    {
        Debug.Log("destroying: " + _currentArrow.name + " relaced with " + itemName);
        AudioSource source = GameObject.FindGameObjectWithTag("BowLoad").GetComponent<AudioSource>();
        source.Play();
        Destroy(_currentArrow);
        _currentArrow = Instantiate(Resources.Load<GameObject>(itemName), notch.transform);
    }

    public bool CheckForArrowInNotch()
    {
        bool notched;
        if (_currentArrow == null)
        {
            notched = false;

        }
        else
        {
            notched = true;
        }
        return notched;
    }

    public void DisableBowDrawExampleHand()
    {
        if(rightHandBowDrawExample != null)
        {
            rightHandBowDrawExample.SetActive(false);

        }
    }

}
