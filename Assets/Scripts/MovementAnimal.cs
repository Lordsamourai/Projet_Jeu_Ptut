using System.Collections;
using UnityEngine;

public class MovementAnimal : MonoBehaviour
{
    public float speed;
    public float randomX;
    public float randomZ;
    public float minWaitTime;
    public float maxWaitTime;
    public GameObject player; // Reference to the player as a GameObject

    private const float detectionRadius = 20.0f; // Fixed detection radius
    private Vector3 currentRandomPos;
    private bool isMovingAwayFromPlayer = false; // To prevent multiple calls to MoveAwayFromPlayer

    void Start()
    {
        PickPosition();
    }

    void PickPosition()
    {
        currentRandomPos = new Vector3(Random.Range(-randomX, randomX), 0.5f, Random.Range(-randomZ, randomZ)); // Keep y at 0
        StartCoroutine(MoveToRandomPos());
    }

    IEnumerator MoveToRandomPos()
    {
        float i = 0.0f;
        float rate = 1.0f / speed;
        Vector3 currentPos = transform.position;

        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;

            // Check if the player is too close during movement
            if (!isMovingAwayFromPlayer && Vector3.Distance(transform.position, player.transform.position) < detectionRadius)
            {
                isMovingAwayFromPlayer = true;
                StopCoroutine(MoveToRandomPos()); // Safely stop this coroutine
                MoveAwayFromPlayer(); // React to the player
                yield break; // Exit coroutine
            }

            // Move smoothly while keeping y at 0
            transform.position = Vector3.Lerp(currentPos, this.currentRandomPos, i);
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z); // Keep y fixed at 0
            yield return null;
        }

        isMovingAwayFromPlayer = false; // Reset flag
        float randomFloat = Random.Range(0.0f, 1.0f); // Create %50 chance to wait
        if (randomFloat < 0.5f)
            StartCoroutine(WaitForSomeTime());
        else
            PickPosition();
    }

    IEnumerator WaitForSomeTime()
    {
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        PickPosition();
    }

    void MoveAwayFromPlayer()
    {
        // Calculate opposite direction
        Vector3 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;
        currentRandomPos = transform.position + directionAwayFromPlayer * detectionRadius;

        // Keep the target position y at 0
        currentRandomPos = new Vector3(currentRandomPos.x, 0.5f, currentRandomPos.z);

        // Move to the new position
        StartCoroutine(MoveToRandomPos());
    }
}
