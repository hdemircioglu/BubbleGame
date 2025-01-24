using UnityEngine;

public class FanController : MonoBehaviour
{
    private Camera mainCamera;
    private float zOffset; // Distance between object and camera

    [SerializeField]
    private Transform target; // Reference to the object to point to

    void Start()
    {
        // Cache the main camera
        mainCamera = Camera.main;

        // Store the z offset between object and camera
        zOffset = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
    }

    void OnMouseDrag()
    {
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
}
