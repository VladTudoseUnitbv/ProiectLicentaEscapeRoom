using Photon.Pun;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f; // Adjust the rotation speed
    [HideInInspector] public Camera playerCamera; // Reference to the player's camera
    public Animator animator; // Reference to the Animator component
    public GameObject model; // Reference to the separate model object

    private Quaternion originalModelRotation;
    private Quaternion targetModelRotation; // Store the target rotation

    private PhotonView photonView;

    Rigidbody rb;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        originalModelRotation = model.transform.localRotation;
        targetModelRotation = originalModelRotation;
        playerCamera = Camera.main;
        //if (!photonView.IsMine) rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        //if (photonView.IsMine && PopupManager.Instance.activePanel == null)
        //{ 
            HandleMovementInput();
            HandleRotation();
            UpdateAnimation();
        //}
    }

    private void HandleMovementInput()
    {
        // Get input values for horizontal and vertical axis
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement vector based on input
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

        // Check if both right click and left click are held
        if (Input.GetMouseButton(1) && Input.GetMouseButton(0))
        {
            // Move forward
            movement.z = 1f;

            // Reset the Y rotation to 0 when moving forward with right click and left click
            Vector3 resetEulerAngles = model.transform.localRotation.eulerAngles;
            resetEulerAngles.y = 0f;
            model.transform.localRotation = Quaternion.Euler(resetEulerAngles);
        }

        // Normalize the movement vector
        movement.Normalize();

        // Multiply the normalized movement vector by the moveSpeed and deltaTime
        movement *= moveSpeed * Time.deltaTime;

        // Move the character
        rb.velocity = transform.TransformDirection(movement);
    }

    private void HandleRotation()
    {
        // Get input values for horizontal and vertical axis
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Rotate the character based on the horizontal input
        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            // Calculate the target rotation
            Vector3 newEulerAngles = originalModelRotation.eulerAngles;
            newEulerAngles.y = Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg;
            targetModelRotation = Quaternion.Euler(newEulerAngles);
        }

        // Smoothly rotate the character towards the target rotation
        model.transform.localRotation = Quaternion.Slerp(model.transform.localRotation, targetModelRotation, rotationSpeed * Time.deltaTime);

        // Rotate the character along with the camera while holding the right mouse button
        if (Input.GetMouseButton(1))
        {
            transform.rotation = Quaternion.Euler(0f, playerCamera.transform.eulerAngles.y, 0f);
        }
    }

    private void UpdateAnimation()
    {
        // Check if the character is moving
        bool isMoving = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

        // Check if both right click and left click are held
        bool isMovingWithMouseButtons = Input.GetMouseButton(1) && Input.GetMouseButton(0);

        // Set the movement animation parameter in the Animator
        animator.SetBool("IsMoving", isMoving || isMovingWithMouseButtons);
    }
}
