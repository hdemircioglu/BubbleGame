using UnityEngine;

public class FanController : MonoBehaviour
{
    private Camera mainCamera;
    private float zOffset; // Distance between object and camera

    [SerializeField]
    private Transform target; // Reference to the object to point to

    [SerializeField]
    private float forceAmount = 10f; // Amount of force to apply

    private Rigidbody rb; // Reference to Rigidbody

    [SerializeField]
    private float maxDistance = 10f; // Maximum distance for force application

    [SerializeField]
    private float minForce = 5f; // Minimum force to apply

    [SerializeField]
    private float maxForce = 15f; // Maximum force to apply

    void Start()
    {
        // Cache the main camera
        mainCamera = Camera.main;

        // Store the z offset between object and camera
        zOffset = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);

        // Get the Rigidbody component
        rb = target.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody attached! Please attach a Rigidbody component.");
        }
    }

    void OnMouseDrag()
    {
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }

        // Get the mouse position in world space
        Vector3 mousePosition = GetMouseWorldPosition();

        // Set the object's position to follow the mouse
        transform.position = mousePosition;

        // Rotate to look at the target
        RotateTowardsTarget();
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Get the mouse position in screen space
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Set the z coordinate to match the object's z position
        mouseScreenPosition.z = zOffset;

        // Convert screen space to world space
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

    private void RotateTowardsTarget()
    {
        if (target == null) return;

        // Make the object face the target
        transform.LookAt(target);
    }

    private void FixedUpdate()
    {
        if (target == null || rb == null) return;

        // Calculate distance to target
        float distance = Vector3.Distance(transform.position, target.position);
        
        // Calculate force based on distance (further = stronger force)
        float normalizedDistance = Mathf.Clamp01(distance / maxDistance);
        float currentForce = Mathf.Lerp(minForce, maxForce, normalizedDistance); // Swapped maxForce and minForce

        // Apply force in the object's forward direction (toward the target)
        Vector3 forceDirection = transform.forward;
        rb.AddForce(forceDirection * currentForce, ForceMode.Force);
    }
}
