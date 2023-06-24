using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;             // The target to follow (character's transform)
    public float distance = 10f;         // The distance between the camera and the target
    public float height = 2f;            // The height of the camera above the target
    public float sensitivity = 2f;       // The sensitivity of the mouse rotation
    public float zoomSpeed = 2f;         // The speed of zooming in and out
    public float minDistance = 2f;       // The minimum allowed distance between the camera and the target
    public float maxDistance = 15f;      // The maximum allowed distance between the camera and the target
    public float minVerticalAngle = -60f;   // The minimum allowed vertical angle of the camera
    public float maxVerticalAngle = 60f;    // The maximum allowed vertical angle of the camera

    private float currentX = 0f;         // The current rotation around the target on the X-axis
    private float currentY = 0f;         // The current rotation on the Y-axis
    private bool isCursorHidden = false; // Flag to track if the cursor is hidden

    void LateUpdate()
    {
        // Check if the target is assigned
        if (target == null)
        {
            Debug.LogWarning("CameraController: No target assigned.");
            return;
        }

        // Get the mouse input for rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the camera around the target based on mouse input
        if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
        {
            currentX += mouseX * sensitivity;
            currentY -= mouseY * sensitivity;

            // Limit the vertical angle within the specified range
            currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);

            // Hide the cursor
            if (!isCursorHidden && Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isCursorHidden = true;
            }
        }
        else
        {
            // Show the cursor
            if (isCursorHidden)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isCursorHidden = false;
            }
        }

        // Get the mouse scroll wheel input for zooming in and out
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the distance between the camera and the target based on scroll wheel input
        distance -= scrollWheelInput * zoomSpeed;

        // Clamp the distance to stay within the minDistance and maxDistance range
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Calculate the camera rotation based on the target's rotation, currentX, and currentY
        Quaternion cameraRotation = Quaternion.Euler(currentY, currentX, 0f);

        // Calculate the camera position based on the target's position, distance, height, and rotation
        Vector3 cameraPosition = target.position - cameraRotation * Vector3.forward * distance;
        cameraPosition.y += height;

        // Update the camera's position and rotation
        transform.position = cameraPosition;
        transform.LookAt(target.position + Vector3.up * height);
    }
}
