using UnityEngine;
using System.Collections;

public class PolarBear : EnemyAI
{
    [Header("Boss Settings")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private GameObject weaponProjectilePrefab;
    [SerializeField] private Transform attackCollider;
    [SerializeField] private float attackRange = 1.5f;

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;

    private float lastAttackTime;
    private BossHealth health;
    public int currentHealth;
    public int startingHealth;

    protected override void Awake()
    {
        base.Awake();
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<BossHealth>();
    }

    protected override void Start()
    {
        base.Start();
        currentHealth = health.currentHealth;
        startingHealth = health.startingHealth;
        attackCollider.gameObject.SetActive(false); // Tắt collider khi bắt đầu
    }

    protected override IEnumerator StateRoutine()
    {
        while (true)
        {
            playerTransform = EnemyTargetProvider.Instance.GetTarget();

            switch (state)
            {
                case State.Roaming:
                    SetFlipDirection(true);
                    yield return StartCoroutine(RoamingRoutine(playerTransform));
                    break;

                case State.ReturningToStart:
                    SetFlipDirection(true);
                    enemyPathfinding.moveSpeed = defaultMoveSpeed * returnSpeedMultiplier;
                    enemyPathfinding.MoveTo(startingPosition - (Vector2)transform.position);
                    if (Vector2.Distance(transform.position, startingPosition) < 0.2f)
                    {
                        enemyPathfinding.moveSpeed = defaultMoveSpeed;
                        state = State.Roaming;
                    }
                    break;

                case State.ChasingPlayer:
                    SetFlipDirection(false);
                    yield return StartCoroutine(ChasingPlayer(playerTransform));
                    break;
            }

            yield return null;
        }
    }
    protected override IEnumerator RoamingRoutine(Transform playerTransform)
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
                // Hiện thanh máu khi bắt đầu đuổi
                health.DisplayHPContainer();
                state = State.ChasingPlayer;
                yield break;
            }

            yield return null;
        }
    }


    protected override IEnumerator ChasingPlayer(Transform playerTransform)
    {
        if (playerTransform == null)
            yield break;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            if (distanceToPlayer <= attackRange)
            {
                enemyPathfinding.moveSpeed = 0;
                Attack();
            }
            else if (distanceToPlayer <= trackingRange)
            {
                TryRangeAttack();
            }
        }
        else
        {
            Vector2 dirToPlayer = playerTransform.position - transform.position;
            enemyPathfinding.moveSpeed = defaultMoveSpeed * chasingSpeedMultiplier;
            enemyPathfinding.MoveTo(dirToPlayer);
        }

        if (Vector2.Distance(transform.position, startingPosition) > roamingRange)
        {
            yield return new WaitForSeconds(2f);
            state = State.ReturningToStart;
        }

        yield return null;
    }

    public virtual void Attack()
    {
        FacePlayer();
        myAnimator.SetTrigger("Scratch");
        attackCollider.gameObject.SetActive(true);
        lastAttackTime = Time.time;
        StartCoroutine(DisableAttackCollider());
    }

    protected virtual void TryRangeAttack()
    {
        FacePlayer();
        myAnimator.SetTrigger("Stomp");
        StartCoroutine(DelayedProjectile());
        lastAttackTime = Time.time;
    }

    private IEnumerator DisableAttackCollider()
    {
        yield return new WaitForSeconds(0.3f);
        attackCollider.gameObject.SetActive(false);
    }

    private IEnumerator DelayedProjectile()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(weaponProjectilePrefab, playerTransform.position, Quaternion.identity);
    }

    private void FacePlayer()
    {
        bool shouldFaceLeft = playerTransform.position.x < transform.position.x;
        Vector3 scale = transform.localScale;
        scale.x = shouldFaceLeft ? -1 : 1;
        transform.localScale = scale;
    }

    // Gán allowFlipByDirection nếu enemyPathfinding là BossPathing
    private void SetFlipDirection(bool allow)
    {
        if (enemyPathfinding is BossPathing bossPath)
        {
            bossPath.allowFlipByDirection = allow;
        }
    }
}
