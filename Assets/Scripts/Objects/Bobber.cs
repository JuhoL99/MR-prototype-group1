using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;

public class Bobber : MonoBehaviour
{
    [SerializeField] private float floatTime = 1;
    [SerializeField] private float stabilityThreshold = 0.3f;
    [SerializeField] private float velocityThreshold = 0.1f;
    private float waterLevel;
    private LineRenderer lineRenderer;
    private Transform[] lineRenderPositions;
    private GameManager gameManager;
    private Rigidbody rb;
    private bool isCheckingDepth = false;
    private bool isLocked = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GameManager.instance;
        waterLevel = gameManager.waterLevel;
        lineRenderer = GetComponent<LineRenderer>();
        SetupLineRenderer();
        gameManager.onDetachedFromWater.AddListener(UnlockBobber);
    }
    private void Update()
    {
        UpdateLineRenderer();
        if(isLocked && Input.GetKeyDown(KeyCode.Space))
        {
            UnlockBobber();
        }
        if (!isLocked && !gameManager.holdingFish)
        {
            CheckIfFloating();
        }
    }
    private void SetupLineRenderer()
    {
        List<Transform> positions = new List<Transform>();
        positions.Add(transform);
        positions.AddRange(transform.GetComponentsInChildren<Transform>().Where(t => t != transform));
        lineRenderPositions = positions.ToArray();
        lineRenderer.positionCount = lineRenderPositions.Length;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
    }

    private void UpdateLineRenderer()
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, lineRenderPositions[i].position);
        }
    }
    private void CheckIfFloating()
    {
        if(transform.position.y < waterLevel && !isCheckingDepth)
        {
            StartCoroutine(WaterChecker());
        }
    }
    private IEnumerator WaterChecker()
    {
        isCheckingDepth = true;
        float timer = 0f;
        while (timer < floatTime)
        {
            float distanceToWater = Mathf.Abs(transform.position.y - waterLevel);
            float currentVelocity = rb.linearVelocity.magnitude;
            if (distanceToWater > stabilityThreshold || currentVelocity > velocityThreshold)
            {
                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }
            yield return null;
        }
        LockBobber();
        isCheckingDepth = false;
    }
    private void LockBobber()
    {
        isLocked = true;
        rb.isKinematic = true;
        Vector3 lockedPosition = transform.position;
        lockedPosition.y = waterLevel;
        transform.position = lockedPosition;
        gameManager.AttachEvent(true);
    }
    public void UnlockBobber()
    {
        isLocked = false;
        isCheckingDepth = false;
        rb.isKinematic = false;
    }
    public Transform HookPos()
    {
        return transform.GetChild(transform.childCount - 1).transform;
    }
}
