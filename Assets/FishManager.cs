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
}
