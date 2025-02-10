using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private Transform cameraTransform;
    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    private void Update()
    {
        transform.LookAt(cameraTransform);
        transform.Rotate(0f, 180f, 0f);
    }
}
