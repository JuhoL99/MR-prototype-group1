using Oculus.Interaction;
using UnityEngine;

public class FishingReel : MonoBehaviour
{
    [SerializeField] private Transform reelHandle;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float snapRotationDegrees = 45f;
    [SerializeField] private bool useSnapRotation = false;
    [SerializeField] private float maxAngularVelocity = 1000f;
    [SerializeField] private float dragMultiplier = 1f;
    [SerializeField] private PointableUnityEventWrapper pointableEvents;
    private FishingSystem fishingSystem;
    private Rigidbody rb;
    private Vector3 previousPointerPosition;
    private bool isSelected = false;
    private float currentRotation = 0f;
    private float reelInput = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition |
                        RigidbodyConstraints.FreezeRotationX |
                        RigidbodyConstraints.FreezeRotationY;
        rb.maxAngularVelocity = maxAngularVelocity;
    }
    private void Start()
    {
        fishingSystem = FishingSystem.instance;
        fishingSystem.reel = this;
    }
    private void OnEnable()
    {
        if (pointableEvents != null)
        {
            pointableEvents.WhenSelect.AddListener(OnSelect);
            pointableEvents.WhenUnselect.AddListener(OnUnselect);
            pointableEvents.WhenMove.AddListener(OnMove);
        }
    }
    private void OnDisable()
    {
        if (pointableEvents != null)
        {
            pointableEvents.WhenSelect.RemoveListener(OnSelect);
            pointableEvents.WhenUnselect.RemoveListener(OnUnselect);
            pointableEvents.WhenMove.RemoveListener(OnMove);
        }
    }
    private void OnSelect(PointerEvent evt)
    {
        isSelected = true;
        previousPointerPosition = evt.Pose.position;
    }
    private void OnUnselect(PointerEvent evt)
    {
        isSelected = false;
        reelInput = 0f;
    }
    private void OnMove(PointerEvent evt)
    {
        if (!isSelected) return;
        Vector3 currentPosition = evt.Pose.position;
        Vector3 pointerDelta = currentPosition - previousPointerPosition;
        float rotationAmount = Vector3.Dot(pointerDelta, transform.right) * rotationSpeed;
        float deadzone = 0.01f;
        if (Mathf.Abs(rotationAmount) > deadzone)
        {
            reelInput = Mathf.Sign(rotationAmount);
        }
        else
        {
            reelInput = 0f;
        }
        if (useSnapRotation)
        {
            currentRotation += rotationAmount;
            float snappedRotation = Mathf.Round(currentRotation / snapRotationDegrees) * snapRotationDegrees;
            rotationAmount = snappedRotation - rb.rotation.eulerAngles.z;
        }
        rb.AddTorque(transform.forward * rotationAmount, ForceMode.VelocityChange);
        rb.angularDamping = rb.angularVelocity.magnitude * dragMultiplier;
        previousPointerPosition = currentPosition;
    }
    public float GetCurrentRotation()
    {
        return rb.rotation.eulerAngles.z;
    }
    public void AddResistance(float amount)
    {
        rb.angularDamping += amount;
    }
    public float GetReelInput()
    {
        return reelInput;
    }
}
