using Oculus.Interaction;
using UnityEditor.Rendering;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    private Transform hookTransform;
    private GameManager gameManager;
    [SerializeField] private GrabInteractable fishGrabInteractable;
    private bool isGrabbed = false;
    private void Start()
    {
        gameManager = GameManager.instance;
    }
    private void Update()
    {
        if(isGrabbed) return;
        if(hookTransform == null) return;
        transform.position = hookTransform.position;
        transform.rotation = hookTransform.rotation * Quaternion.Euler(0f,0f,-90f);
    }
    public void HandleFishGrabbed()
    {
        Debug.Log("grabbed");
        isGrabbed = true;
        gameManager.FishGrabbedEvent();
    }
    public void HandleFishReleased()
    {
        Debug.Log("released");
    }
    public void SetHook(Transform hook)
    {
        hookTransform = hook;
    }
}
