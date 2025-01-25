using UnityEngine;

public class FanController : MonoBehaviour
{
    [SerializeField] private Bubble target; // Reference to the object to point to
    [SerializeField] private float forceAmount = 10f; // Amount of force to apply
    [SerializeField] private float maxDistance = 10f; // Maximum distance for force application
    [SerializeField] private float minForce = 5f; // Minimum force to apply
    [SerializeField] private float maxForce = 15f; // Maximum force to apply
    [SerializeField] private float fanOffsetRotation;
    [SerializeField] private Transform fanMeshTransform;
    [SerializeField] private ParticleSystem windParticles;
    [SerializeField] private float fanRadius;

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
        transform.LookAt(target.transform, Vector3.up);
        if (transform.position.x > target.transform.position.x)
            fanMeshTransform.localRotation = Quaternion.Euler(0, -fanOffsetRotation, 0);
        else
            fanMeshTransform.localRotation = Quaternion.Euler(0, fanOffsetRotation, 0);
    }

    private void FixedUpdate()
    {
        if (gameManager.currentState == GameState.GameOn)
        {
            transform.position = GetMouseWorldPosition();
            RotateTowardsTarget();

            if (target != null)
            {
                if (Input.GetMouseButton(0)) // left mouse button
                {
                    AddFanForce();
                    windParticles.gameObject.SetActive(true);
                }
                else
                {
                    windParticles.gameObject.SetActive(false);
                }
            }
        }
    }


    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);
        Vector3 targetPosition = transform.position;

        if (xyPlane.Raycast(ray, out float enter))
        {
            targetPosition = ray.GetPoint(enter);
        }

        // Check for obstacles at mouse position using a very small radius
        Collider[] hitColliders = Physics.OverlapSphere(targetPosition, 0.1f, LayerMask.GetMask("Obstacle","Bubble"));
        
        foreach (Collider collider in hitColliders)
        {
            Bounds bounds = collider.bounds;
            float offset = fanRadius;

            if (collider.CompareTag("BottomObstacle"))
            {
                // Snap to top border
                targetPosition.y = bounds.max.y + offset;
                
                // Clamp to side borders
                targetPosition.x = Mathf.Clamp(targetPosition.x, 
                    bounds.min.x + offset, 
                    bounds.max.x - offset);
            }
            else if (collider.CompareTag("TopObstacle"))
            {
                // Snap to bottom border
                targetPosition.y = bounds.min.y - offset;
                
                // Clamp to side borders
                targetPosition.x = Mathf.Clamp(targetPosition.x, 
                    bounds.min.x + offset, 
                    bounds.max.x - offset);
            }
            else //if (collider.CompareTag("VerticalObstacle"))
            {
                // Calculate distances to all borders
                float distanceToLeft = Mathf.Abs(targetPosition.x - bounds.min.x);
                float distanceToRight = Mathf.Abs(targetPosition.x - bounds.max.x);
                float distanceToTop = Mathf.Abs(targetPosition.y - bounds.max.y);
                float distanceToBottom = Mathf.Abs(targetPosition.y - bounds.min.y);

                // Find the minimum distance
                float minDistance = Mathf.Min(distanceToLeft, distanceToRight, distanceToTop, distanceToBottom);

                // Snap to the closest border
                if (minDistance == distanceToLeft)
                {
                    targetPosition.x = bounds.min.x - offset;
                }
                else if (minDistance == distanceToRight)
                {
                    targetPosition.x = bounds.max.x + offset;
                }
                else if (minDistance == distanceToTop)
                {
                    targetPosition.y = bounds.max.y + offset;
                }
                else // bottom is closest
                {
                    targetPosition.y = bounds.min.y - offset;
                }
            }
        }

        return targetPosition;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fanRadius);
    }
}
