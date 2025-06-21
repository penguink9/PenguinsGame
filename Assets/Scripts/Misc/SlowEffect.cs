using System.Collections;
using UnityEngine;

public class SlowEffect : MonoBehaviour
{
    public bool isSlowed { get; private set; }

    [SerializeField] private float slowDuration = 2f; // Duration of slow
    [SerializeField] private float slowFactor = 0.5f; // Amount of slow (e.g. 0.5 means 50% speed)

    private float originalSpeed;
    private EnemyPathing enemyMovement;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyPathing>(); // Assumes you have an EnemyMovement script with speed control
    }

    public void ApplySlow()
    {
        if (!isSlowed)
        {
            isSlowed = true;
            originalSpeed = enemyMovement.moveSpeed; // Store the original speed
            enemyMovement.moveSpeed *= slowFactor; // Reduce the speed

            StartCoroutine(SlowRoutine());
        }
    }

    private IEnumerator SlowRoutine()
    {
        yield return new WaitForSeconds(slowDuration); // Wait for the slow duration to finish
        enemyMovement.moveSpeed = originalSpeed; // Restore the original speed
        isSlowed = false; // Reset the slow state
    }
}