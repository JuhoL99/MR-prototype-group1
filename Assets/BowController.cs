using Oculus.Interaction;
using UnityEngine;
using System;

public class BowController : MonoBehaviour
{
    [Header("Grab components")]
    [SerializeField] private GrabInteractable bowInteractable;
    [SerializeField] private GrabInteractable stringInteractable;
    [Header("String")]
    [SerializeField] private Transform topStringAnchor;
    [SerializeField] private Transform bottomStringAnchor;
    [SerializeField] private Transform stringGrabPoint;
    [SerializeField] private Transform stringPullPoint;
    [SerializeField] private LineRenderer topStringLine;
    [SerializeField] private LineRenderer bottomStringLine;
    [SerializeField] private float lineThickness = 0.025f;
    [SerializeField] private Transform stringRestPos;
    [SerializeField] private float maxPullDistance = 0.75f;
    [Header("Arrow")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawn;
    private GameObject currentArrow;
    private Vector3[] topStringPoints = new Vector3[2];
    private Vector3[] bottomStringPoints = new Vector3[2];

    private bool isBowHeld;
    private bool isStringPulled;
    private float pullState;
    

    private void Awake()
    {
        SetupLineRenderer();
        bowInteractable.WhenStateChanged += OnBowInteractableStateChange;
        stringInteractable.WhenStateChanged += OnStringInteractableStateChange;
    }
    private void OnDestroy()
    {
        bowInteractable.WhenStateChanged -= OnBowInteractableStateChange;
        stringInteractable.WhenStateChanged -= OnStringInteractableStateChange;
    }
    private void SetupLineRenderer()
    {
        topStringLine.positionCount = 2;
        bottomStringLine.positionCount = 2;
        topStringLine.startWidth = lineThickness;
        topStringLine.endWidth = lineThickness;
        bottomStringLine.startWidth = lineThickness;
        bottomStringLine.endWidth = lineThickness;
        UpdateLineVisuals();
    }
    private void UpdateLineVisuals()
    {
        topStringPoints[0] = topStringAnchor.position;
        topStringPoints[1] = stringPullPoint.position;
        bottomStringPoints[0] = bottomStringAnchor.position;
        bottomStringPoints[1] = stringPullPoint.position;
        topStringLine.SetPositions(topStringPoints);
        bottomStringLine.SetPositions(bottomStringPoints);
    }
    private void Update()
    {
        if (isStringPulled) UpdateStringPull();
        UpdateLineVisuals();
    }
    private void OnBowInteractableStateChange(InteractableStateChangeArgs args)
    {
        switch (args.NewState)
        {
            case InteractableState.Select:
                HandleBowGrabbed();
                break;
            case InteractableState.Normal:
                HandleBowReleased();
                break;
        }
    }
    private void HandleBowGrabbed()
    {
        isBowHeld = true;
    }
    private void HandleBowReleased()
    {
        isBowHeld = false;
    }
    private void OnStringInteractableStateChange(InteractableStateChangeArgs args)
    {
        switch (args.NewState)
        {
            case InteractableState.Select:
                HandleStringGrabbed();
                break;
            case InteractableState.Normal:
                HandleStringReleased();
                break;
        }
    }
    private void HandleStringGrabbed()
    {
        if(isBowHeld) StartStringPull();
    }
    private void HandleStringReleased()
    {
        if (isStringPulled) ReleaseArrow();
    }
    private void StartStringPull()
    {
        isStringPulled = true;
        pullState = 0f;
        currentArrow = Instantiate(arrowPrefab, arrowSpawn.position, arrowSpawn.rotation);
        currentArrow.transform.parent = transform;
    }
    private void ReleaseArrow()
    {
        isStringPulled = false;
        if(currentArrow != null)
        {
            //arrow logic here?
        }
        stringGrabPoint.position = stringRestPos.position;
        stringPullPoint.position = stringRestPos.position;
        pullState = 0;
    }
    private void UpdateStringPull()
    {
        Vector3 pullDirection = stringGrabPoint.position - stringRestPos.position;
        float pullDistance = Mathf.Clamp(Vector3.Dot(pullDirection, transform.forward), 0, maxPullDistance);
        pullState = pullDistance;
        Vector3 newStringPosition = stringRestPos.position - (-transform.forward * pullDistance); //temp fix rotated wrong
        stringPullPoint.position = newStringPosition;
        Debug.Log(pullState);
        if (currentArrow != null)
        {
            currentArrow.transform.position = newStringPosition;
            currentArrow.transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0); //temp fix rotated wrong
        }
    }
}
