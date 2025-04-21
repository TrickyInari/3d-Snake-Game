using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;              // Reference to the snake's transform
    public Vector3 offset = new Vector3(0, 5, -8); // Height and distance behind the snake
    public float followSpeed = 8f;        // Camera follow speed
    public float lookAtHeight = 1.0f;     // Height above snake's position to look at

    void LateUpdate()
    {
        if (target == null) return;

        // Desired camera position
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        // Smoothly interpolate position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Look at the target, slightly above the snake's origin
        Vector3 lookAtPoint = target.position + Vector3.up * lookAtHeight;
        transform.LookAt(lookAtPoint);
    }
}