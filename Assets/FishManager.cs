using Oculus.Interaction;
using Oculus.Platform.Models;
using UnityEditor.Rendering;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    private Transform hookTransform;
    private FishingSystem fishingSystem;
    [SerializeField] private GrabInteractable fishGrabInteractable;
    private bool isGrabbed = false;
    public FishSpecies Species => species;
    private FishSpecies species;
    public float Weight => weight;
    private float weight;
    public float Length => length;
    private float length;

    public GameObject InventoryElement;
    public LayerMask uiLayerMask;
    public InventoryManager inventoryManager;

    private CharacterJoint joint;
    private void Start()
    {
        fishingSystem = FishingSystem.instance;
    }
    private void Update()
    {
        if(isGrabbed) return;
        if(hookTransform == null) return;
        FollowHook();
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, uiLayerMask))
        {
            // Check if the hit object is the target UI element
            if (hit.collider.gameObject == InventoryElement)
            {
                Debug.Log("GameObject is in contact with the UI element!");
                InventoryManager.instance.AddFish(species);
                Destroy(gameObject);
            }
        }
    }
    public void GenerateStats(FishSpecies _species)
    {
        species = _species;
        weight = Random.Range(species.minWeight, species.maxWeight);
        length = Random.Range(species.minLength, species.maxLength);
        Debug.Log($"caught {species.name}, weighing: {weight} of {length} meters");
    }
    public void HandleFishGrabbed()
    {
        isGrabbed = true;
        fishingSystem.FishGrabbedEvent(this);
    }
    public void HandleFishReleased()
    {
        fishingSystem.FishDroppedEvent();
    }
    public void SetHook(Transform hook)
    {
        hookTransform = hook;
    }
    private void FollowHook()
    {
        transform.position = hookTransform.position;
        transform.rotation = hookTransform.rotation * Quaternion.Euler(0f, 0f, -90f);
    }
    void OnDrawGizmos()
    {
        // Draw the ray for visualization in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 10);
    }
}
