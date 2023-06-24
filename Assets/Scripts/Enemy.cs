using UnityEngine;

public class Enemy : Character
{
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] bool isPatrolling;
    float rotationSpeed = 15f;

    private Rigidbody enemyRigidbody;
    private int currentPointIndex = 0;

    private void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();

        anim.SetBool("isIdle", !isPatrolling);

        if (patrolPoints.Length < 2)
        {
            Debug.LogWarning("Not enough patrol points.");
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isPatrolling)
            return;

        Vector3 targetPosition = patrolPoints[currentPointIndex].position;
        Vector3 movementDirection = (targetPosition - transform.position).normalized;
        enemyRigidbody.MovePosition(transform.position + movementDirection * moveSpeed * Time.fixedDeltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.3f)
        {
            currentPointIndex++;
            if (currentPointIndex >= patrolPoints.Length)
            {
                currentPointIndex = 0;
            }
        }
    }
}
