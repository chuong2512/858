using UnityEngine;

public static class RigidbodyExtensions
{
    public static void ResetVelocity(this Rigidbody rigidbody)
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    public static void ResetTransform(this Rigidbody rigidbody)
    {
        rigidbody.position = Vector3.zero;
        rigidbody.rotation = Quaternion.identity;
    }
}

