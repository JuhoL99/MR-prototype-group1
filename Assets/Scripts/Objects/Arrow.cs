using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float destroyTime = 15f;
    [SerializeField] private float stickVelocity = 10f;
    [SerializeField] private string collisionTag;

    private Rigidbody rb;
    private bool isStuck = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
    public void Launch(float shootForce)
    {
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.linearVelocity = transform.forward * shootForce;
        Destroy(gameObject, destroyTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag != collisionTag) return;
        if(!isStuck && rb.linearVelocity.magnitude > stickVelocity)
        {
            StickToObject(collision);
        }
    }
    private void StickToObject(Collision collision)
    {
        Debug.Log("tried to stick");
        isStuck = true;
        rb.isKinematic = true;
        transform.parent = collision.transform;
    }
}
