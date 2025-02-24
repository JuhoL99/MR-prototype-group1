using UnityEngine;

public class LeftHandOverlayOculus : MonoBehaviour
{
    public GameObject leftHand;
    public Canvas overlayCanvas;
    public Vector3 offset = new Vector3(0, 0.1f, 0.1f); // Adjust offset as needed

    void Update()
    {
        if (leftHand != null)
        {
            overlayCanvas.transform.position = leftHand.transform.position + leftHand.transform.rotation * offset;
            overlayCanvas.transform.rotation = leftHand.transform.rotation;
        }
    }
}
