using System.Collections;
using UnityEngine;
public class MonkeyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        ReturningToStart
    }

    private State state;

    [Header("AI Settings")]
    [SerializeField] private float roamingRange = 5f;
    [SerializeField] private float trackingRange = 8f;
    [SerializeField] private float returnSpeedMultiplier = 2f;
    [SerializeField] private float attackCooldown = 2f;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject bananaProjectilePrefab;

    private Vector2 startingPosition;
    private EnemyPathing enemyPathfinding;
    private Transform playerTransform;
    private float defaultMoveSpeed;
    private float lastAttackTime;

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathing>();
        state = State.Roaming;
        startingPosition = transform.position;
        defaultMoveSpeed = enemyPathfinding.moveSpeed;

        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastAttackTime = -attackCooldown;
    }

    private void Start()
    {
        StartCoroutine(StateRoutine());
    }

    private IEnumerator StateRoutine()
    {
        while (true)
        {
            playerTransform = EnemyTargetProvider.Instance.GetTarget();
            switch (state)
            {
                case State.Roaming:
                    yield return StartCoroutine(RoamingRoutine());
                    break;

                case State.ReturningToStart:
                    enemyPathfinding.moveSpeed = defaultMoveSpeed * returnSpeedMultiplier;
                    enemyPathfinding.MoveTo(startingPosition - (Vector2)transform.position);
                    if (Vector2.Distance(transform.position, startingPosition) < 0.2f)
                    {
                        enemyPathfinding.moveSpeed = defaultMoveSpeed;
                        state = State.Roaming;
                    }
                    break;
            }

            yield return null;
        }
    }


    private IEnumerator RoamingRoutine()
    {
        Vector2 roamPosition = GetRoamingPosition();

        if (Vector2.Distance(startingPosition, roamPosition) > roamingRange)
            yield break;

        Vector2 moveDir = roamPosition - (Vector2)transform.position;
        enemyPathfinding.moveSpeed = defaultMoveSpeed;
        enemyPathfinding.MoveTo(moveDir);

        float timer = 0f;
        while (timer < 2f)
        {
            timer += Time.deltaTime;

            if (playerTransform != null &&
                Vector2.Distance(transform.position, playerTransform.position) <= trackingRange)
            {
                TryAttack();
            }

            yield return null;
        }
    }

    private Vector2 GetRoamingPosition()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float radius = Random.Range(0.5f, roamingRange);

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        return startingPosition + offset;
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            myAnimator.SetTrigger("Attack");

            // Flip hướng ném
            if (transform.position.x - playerTransform.position.x < 0)
                spriteRenderer.flipX = false;
            else
                spriteRenderer.flipX = true;

            //Instantiate(bananaProjectilePrefab, transform.position, Quaternion.identity);
            StartCoroutine(DelayedProjectile());

            lastAttackTime = Time.time;
        }
    }

    private IEnumerator DelayedProjectile()
    {
        yield return new WaitForSeconds(0.5f);

        Instantiate(bananaProjectilePrefab, transform.position, Quaternion.identity);
    }

    //public void SpawnProjectileAnimEvent()
    //{
    //    Instantiate(bananaProjectilePrefab, transform.position, Quaternion.identity);
    //}

}
