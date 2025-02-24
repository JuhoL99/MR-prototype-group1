using UnityEngine;

public class Hook : MonoBehaviour
{
    public void Attach(Rigidbody segment)
    {
        ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = segment;
        joint.anchor = Vector3.zero;
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
}