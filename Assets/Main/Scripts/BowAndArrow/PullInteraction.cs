using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class PullInteraction : XRBaseInteractable
{
    public static event Action<float> PullActionReleased;

    public Transform start, end;
    public GameObject notch;

    public float PullAmount { get; private set; } = 0.0f;

    private LineRenderer _lineRenderer;
    private IXRSelectInteractor pullingInteractor = null;
    public AmmoManagerSpawner ammoManagerSpawner;
    private AudioSource _soundEffect;
    protected override void Awake()
    {
        base.Awake();
        _lineRenderer = GetComponent<LineRenderer>();
        _soundEffect = GetComponent<AudioSource>();
    }

    public void SetPullInteractor(SelectEnterEventArgs args)
    {
        pullingInteractor = args.interactorObject;
    }

    public void Release()
    {
        PullActionReleased?.Invoke(PullAmount);
        pullingInteractor = null;
        PullAmount = 0f;
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, notch.transform.localPosition.z);
        UpdateString();
        if(_soundEffect != null)
        {
            if(ammoManagerSpawner._arrowNotched)
            {
                _soundEffect.Play();
            }

        }
        
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if(isSelected)
            {
                if(pullingInteractor != null)
                {
                    Vector3 pullPosition = pullingInteractor.transform.position;
                    PullAmount = CalculatePull(pullPosition);
                    UpdateString();
                }
            }
               
        }
    }

    private float CalculatePull(Vector3 pullPosition)
    {
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;
        float maxLength = targetDirection.magnitude;

        targetDirection.Normalize();
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        return Mathf.Clamp(pullValue, 0, 1);
    }

    private void UpdateString()
    {
        Vector3 linePosition = Vector3.forward * Mathf.Lerp(start.transform.localPosition.z, end.transform.localPosition.z, PullAmount);
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, linePosition.z + .2f);
        _lineRenderer.SetPosition(1, linePosition);
    }

    private void HapticFeedBack()
    {
        if(pullingInteractor != null)
        {
            ActionBasedController currentController = pullingInteractor.transform.gameObject.GetComponent<ActionBasedController>();
            currentController.SendHapticImpulse(PullAmount, .1f);
        }
    }
}
