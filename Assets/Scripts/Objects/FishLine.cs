using UnityEngine;
using UnityEngine.InputSystem;

public class FishLine : MonoBehaviour
{
    [Header("Line")]
    [SerializeField] private float maxLength = 6f;
    [SerializeField] private float minLength = 0.25f;
    [SerializeField] private float extensionSpeed = 2f;
    [SerializeField] private float retractionSpeed = 2f;
    [SerializeField] private float lineWidth = 0.01f;

    [Header("Segments")]
    [SerializeField] private int segmentCount = 15;
    [SerializeField] private float segmentMass = 0.075f;
    [SerializeField] private float springForce = 2000f;
    [SerializeField] private float damping = 50f;
    [SerializeField] private float segmentDrag = 0.5f;

    [Header("Other")]
    [SerializeField] private Transform attach;
    [SerializeField] private GameObject bobber;
    private GameObject bobberObject;

    private LineRenderer lineRenderer;
    private GameObject[] segments;
    private SpringJoint[] joints;
    private bool isExtending = false;
    private bool isRetracting = false;
    private float currentLength;
    
    private void Start()
    {
        InitializeLine();
        GenerateSegments();
        ConnectSegments();
    }
    private void Update()
    {
        HandleInput();
        UpdateLineLength();
        UpdateLineRenderer();
        segments[0].transform.position = attach.position;
    }
    private void InitializeLine()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = segmentCount + 1;
        currentLength = minLength;
    }
    private void GenerateSegments()
    {
        segments = new GameObject[segmentCount];
        joints = new SpringJoint[segmentCount];

        for (int i = 0; i < segmentCount; i++)
        {
            segments[i] = new GameObject($"Segment_{i}");
            segments[i].transform.parent = transform;
            Rigidbody rigidbody = segments[i].AddComponent<Rigidbody>();
            Buoyancy byc = segments[i].AddComponent<Buoyancy>();
            byc.SetWaterLevelAndBuoyancy(1f, 70f);
            rigidbody.mass = segmentMass;
            rigidbody.useGravity = true;
            rigidbody.linearDamping = segmentDrag;
            SphereCollider collider = segments[i].AddComponent<SphereCollider>();
            collider.radius = lineWidth / 2;
            float t = i / (float)(segmentCount - 1);
            segments[i].transform.position = transform.position + -transform.up * (minLength * t); //transform.forward?
        }
    }
    private void ConnectSegments()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            joints[i] = segments[i].AddComponent<SpringJoint>();
            if(i != 0) joints[i].connectedBody = segments[i - 1].GetComponent<Rigidbody>();
            joints[i].spring = springForce;
            joints[i].damper = damping;
            joints[i].autoConfigureConnectedAnchor = false;
            joints[i].anchor = Vector3.zero;
            joints[i].connectedAnchor = Vector3.zero;
        }
        segments[0].GetComponent<Rigidbody>().isKinematic = true;
        segments[0].transform.position = transform.position;
        bobberObject = Instantiate(bobber, segments[segmentCount-1].transform.position, Quaternion.identity);
        bobberObject.GetComponent<FixedJoint>().connectedBody = segments[segmentCount-1].GetComponent<Rigidbody>();
    }
    private void HandleInput()
    {
        isExtending = Input.GetKey(KeyCode.E);
        isRetracting = Input.GetKey(KeyCode.R);
    }
    private void UpdateLineLength()
    {
        if (isExtending && currentLength < maxLength)
        {
            currentLength += extensionSpeed * Time.deltaTime;
        }
        else if (isRetracting && currentLength > minLength)
        {
            currentLength -= retractionSpeed * Time.deltaTime;
        }
        currentLength = Mathf.Clamp(currentLength, minLength, maxLength);
        float segmentLength = currentLength / (segmentCount - 1);
        for (int i = 0; i < joints.Length; i++)
        {
            joints[i].minDistance = segmentLength * 0.9f;
            joints[i].maxDistance = segmentLength * 1.1f;
        }
    }
    private void UpdateLineRenderer()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            lineRenderer.SetPosition(i, segments[i].transform.position);
        }
        lineRenderer.SetPosition(segmentCount, bobberObject.transform.position);
    }
    public Vector3 GetEndPoint()
    {
        return segments[segmentCount - 1].transform.position;
    }
    public void SetStartPoint(Vector3 position)
    {
        transform.position = position;
        segments[0].transform.position = position;
    }
}