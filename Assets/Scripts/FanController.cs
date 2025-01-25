using UnityEngine;

public class FanController : MonoBehaviour
{
    [SerializeField] private Bubble target; // Reference to the object to point to
    [SerializeField] private float forceAmount = 10f; // Amount of force to apply
    [SerializeField] private float maxDistance = 10f; // Maximum distance for force application
    [SerializeField] private float minForce = 5f; // Minimum force to apply
    [SerializeField] private float maxForce = 15f; // Maximum force to apply

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget.GetComponent<Bubble>();
    }

    private void RotateTowardsTarget()
    {
        if (target == null) return;
        transform.LookAt(target.transform);
    }

    private void FixedUpdate()
    {
        if (gameManager.currentState == GameState.GameOn)
        {
            transform.position = GetMouseWorldPosition();
            RotateTowardsTarget();

            if (Input.GetMouseButton(0) && target != null) // left mouse button
                AddFanForce();
        }
    }


    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);
        Vector3 worldPosition = Vector3.zero;

        if (xyPlane.Raycast(ray, out float enter))
        {
            worldPosition = ray.GetPoint(enter);
        }
        return worldPosition;
    }

    private void AddFanForce()
    {
        // Calculate distance to target
        float distance = Vector3.Distance(transform.position, target.transform.position);

        // Calculate force based on distance (further = stronger force)
        float normalizedDistance = Mathf.Clamp01(distance / maxDistance);
        float currentForce = Mathf.Lerp(maxForce, minForce, normalizedDistance); // Swapped maxForce and minForce

        // Apply force in the object's forward direction (toward the target)
        Vector3 forceDirection = transform.forward;
        target.AddForce(forceDirection * currentForce);
    }
}
