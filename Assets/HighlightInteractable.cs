using UnityEngine;

public class HighlightInteractable : MonoBehaviour
{
    private Material material;
    private static readonly int InteractionState = Shader.PropertyToID("_InteractionState");
    //0 default highlight, 1 hover highlight, 2 nothing
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }
    public void OnHoverEnter()
    {
        material.SetFloat(InteractionState, 1);
    }
    public void OnHoverExit()
    {
        material.SetFloat(InteractionState, 0); //0 for always highlight, 2 for only highlight hovered
    }
    public void OnGrabEnter()
    {
        material.SetFloat(InteractionState, 2);
    }
    public void OnGrabExit()
    {
        material.SetFloat(InteractionState, 1);
    }
}
