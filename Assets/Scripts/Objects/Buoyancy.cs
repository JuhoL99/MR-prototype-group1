using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    [Header("Buoyancy Forces")]
    [SerializeField] private float buoyancyForce = 15f;
    [SerializeField] private float damping = 0.5f;
    private FishingSystem fishingSystem;
    private float waterLevel;
    private Rigidbody rb;
    private float initialDrag;
    private float initialAngularDrag;

    private void Start()
    {
        fishingSystem = FishingSystem.instance;
        rb = GetComponent<Rigidbody>();
        initialDrag = rb.linearDamping;
        initialAngularDrag = rb.angularDamping;
        waterLevel = fishingSystem.WaterLevel;
    }
    private void FixedUpdate()
    {
        float currentWaterLevel = waterLevel;
        float submersionDepth = currentWaterLevel - transform.position.y;

        if (submersionDepth > 0)
        {
            float buoyancyMultiplier = Mathf.Clamp01(submersionDepth);
            Vector3 buoyancyForceVector = Vector3.up * buoyancyForce * buoyancyMultiplier;
            rb.AddForce(buoyancyForceVector, ForceMode.Acceleration);
            rb.linearDamping = initialDrag + (damping * buoyancyMultiplier);
            rb.angularDamping = initialAngularDrag + (damping * buoyancyMultiplier);
        }
        else
        {
            rb.linearDamping = initialDrag;
            rb.angularDamping = initialAngularDrag;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0.5f, 1f, 0.3f);
        Vector3 center = transform.position;
        center.y = waterLevel;
        Gizmos.DrawCube(center, new Vector3(1f, 0.01f, 1f));
    }
    public void SetWaterLevelAndBuoyancy(float _waterLevel, float _buoyancyForce)
    {
        waterLevel = _waterLevel;
        buoyancyForce = _buoyancyForce;
    }
}