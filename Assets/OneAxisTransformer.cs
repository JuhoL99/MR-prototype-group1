using UnityEngine;
using Oculus.Interaction;

public class OneAxisTransformer : MonoBehaviour, ITransformer
{
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private bool useSnapRotation = false;
    [SerializeField] private float snapRotationDegrees = 45f;
    [SerializeField] private Vector3 rotationAxis = Vector3.right;

    private IGrabbable grabbable;
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    private Vector3 grabCenter;
    private Vector3 lastGrabDirection;
    private float accumulatedRotation = 0f;
    private bool isInitialFrame = true;

    public void Initialize(IGrabbable _grabbable)
    {
        grabbable = _grabbable;
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
        rotationAxis = rotationAxis.normalized;
    }
    public void BeginTransform()
    {
        if (grabbable.GrabPoints.Count > 0)
        {
            grabCenter = transform.position;
            lastGrabDirection = GetGrabDirectionVector(grabbable.GrabPoints[0].position);
            accumulatedRotation = 0f;
            isInitialFrame = true;
        }
    }
    private Vector3 GetGrabDirectionVector(Vector3 grabPosition)
    {
        Vector3 directionToGrab = grabPosition - grabCenter;
        Vector3 worldRotationAxis = transform.TransformDirection(rotationAxis);
        return Vector3.ProjectOnPlane(directionToGrab, worldRotationAxis).normalized;
    }
    public void UpdateTransform()
    {
        if (grabbable.GrabPoints.Count == 0) return;
        transform.localPosition = initialLocalPosition;
        Vector3 currentGrabDirection = GetGrabDirectionVector(grabbable.GrabPoints[0].position);
        if (!isInitialFrame)
        {
            float deltaAngle = Vector3.SignedAngle(lastGrabDirection, currentGrabDirection, transform.TransformDirection(rotationAxis));
            deltaAngle *= rotationSpeed;
            accumulatedRotation += deltaAngle;
            if (useSnapRotation)
            {
                float snappedRotation = Mathf.Round(accumulatedRotation / snapRotationDegrees) * snapRotationDegrees;
                accumulatedRotation = snappedRotation;
            }
            transform.localRotation = initialLocalRotation * Quaternion.AngleAxis(accumulatedRotation, rotationAxis);
        }
        lastGrabDirection = currentGrabDirection;
        isInitialFrame = false;
    }

    public void EndTransform()
    {

    }
}