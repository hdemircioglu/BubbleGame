using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Bubble : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private GameObject PopPartcileObject;

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
            Instantiate(PopPartcileObject, transform.position, Quaternion.identity);
            GameManager.Instance.ChangeState(GameState.Fail);
            Destroy(gameObject);
        }
    }

}
