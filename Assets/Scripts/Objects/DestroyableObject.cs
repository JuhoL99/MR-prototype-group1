using UnityEngine;
using UnityEngine.Events;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] private GameObject fractured;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float breakVelocity = 10f;
    [SerializeField] private UnityEvent<Vector3> onHitEvent;
    private Vector3 previousVelocity;
    private Rigidbody rb;
    private bool destroyed = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        CheckArrowCollision(collision);
    }
    private void ExplodeObject(Collision collision)
    {
        destroyed = true;
        GameObject go = Instantiate(fractured, transform.position, transform.rotation); //* Quaternion.Euler(90, 0, 0)); //-90 x rotation from blender
        FracturedObject script = go.GetComponent<FracturedObject>();
        Vector3 explosionPosition = collision.GetContact(0).otherCollider.transform.position;
        script.SetValues(explosionForce, explosionRadius, explosionPosition);
        onHitEvent?.Invoke(transform.position);
        Destroy(gameObject);
    }
    private void CheckArrowCollision(Collision collision)
    {
        if (!(collision.collider.gameObject.tag == "Arrow")) return;
        if (collision.relativeVelocity.magnitude < breakVelocity) return;
        if (!destroyed) ExplodeObject(collision);
    }
}
