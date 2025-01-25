using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask winLayer;
    [SerializeField] private GameObject PopPartcileObject;
    [SerializeField] private GameObject PopWinPartcileObject;

    private Rigidbody rb; // Reference to Rigidbody
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;

        // Get the Rigidbody component
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody attached! Please attach a Rigidbody component.");
        }
    }
    private void Update()
    {
        if (gameManager.currentState == GameState.GameOn)
        {
            rb.isKinematic = false;
        }

        if (gameManager.currentState == GameState.Fail)
        {

        }
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((obstacleLayer & (1 << collision.gameObject.layer)) != 0)
        {
            GameManager.Instance.FailBubble();
            Instantiate(PopPartcileObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if ((winLayer & (1 << collision.gameObject.layer)) != 0)
        {
            GameManager.Instance.WinBubble();
            Instantiate(PopWinPartcileObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
