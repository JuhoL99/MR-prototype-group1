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
    private Rigidbody rb;
    public FishSpecies Species => species;
    public FishSpecies species;
    public float Weight => weight;
    private float weight;
    public float Length => length;
    private float length;

    public GameObject InventoryElement;
    public InventoryManager inventoryManager;

    private CharacterJoint joint;
    private void Start()
    {
        fishingSystem = FishingSystem.instance;
        rb = GetComponent<Rigidbody>();
        InventoryElement = GameObject.Find("LeftHandOverlay");
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        weight = Random.Range(species.minWeight, species.maxWeight);
        length = Random.Range(species.minLength, species.maxLength);

    }

    private void Update()
    {
        if(isGrabbed){
           return;
        }
        else{
            if(hookTransform == null) return;
            FollowHook();
        }
        
        
    }

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Overlay"))
        {
            Debug.Log("Fish collided with UI element: " + other.name);
            inventoryManager.AddFish(species);
            Destroy(this.gameObject);
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
        //rb.isKinematic = false;
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
