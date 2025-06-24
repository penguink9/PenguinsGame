using System.Collections;
using UnityEngine;

public class SlowEffect : MonoBehaviour
{
    public bool isSlowed { get; private set; }

    [SerializeField] private float slowDuration = 5f; // Duration of slow
    [SerializeField] private float slowFactor = 0.5f; // Amount of slow (e.g. 0.5 means 50% speed)

    private float originalSpeed;
    private EnemyAI enemyMovement;
    private Coroutine slowCoroutine; // To store the current coroutine
    private float restoreStuntime = 0.5f;
    [SerializeField] private Material BlueStun;
    private SpriteRenderer spriteRenderer;
    private Material defaultMat;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
        enemyMovement = GetComponent<EnemyAI>(); // Assumes you have an EnemyMovement script with speed control

    }
   

    public void ApplySlow()
    {
        StartCoroutine(StunRoutine());
        if (!isSlowed)
        {
            // Apply the slow only if it's not already slowed
            isSlowed = true;
            originalSpeed = enemyMovement.defaultMoveSpeed; // Store the original speed
            enemyMovement.defaultMoveSpeed *= slowFactor; // Reduce the speed
            slowCoroutine = StartCoroutine(SlowRoutine());

        }
        else
        {
            StopCoroutine(slowCoroutine);
            slowCoroutine = StartCoroutine(SlowRoutine());  
            
        }
    }


    private IEnumerator SlowRoutine()
    {
        yield return new WaitForSecondsRealtime(slowDuration); // Wait for the slow duration to finish
        enemyMovement.defaultMoveSpeed = originalSpeed; // Restore the original speed
        isSlowed = false; // Reset the slow state


    }

    public IEnumerator StunRoutine()
    {
        spriteRenderer.material = BlueStun;
        yield return new WaitForSeconds(restoreStuntime);
        spriteRenderer.material = defaultMat;
    }
}