using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using Unity.Jobs;


public class PolarBear : MonoBehaviour
{
    protected enum State
    {
        Roaming,
        ReturningToStart,
        ChasingPlayer,

    }

    protected State state;
    [Header("ThrowEnemy AI Settings")]
    [SerializeField] private float attackCooldown = 2f;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject weaponProjectilePrefab;
    //[SerializeField] private GameObject slashAnimPrefab;
    //[SerializeField] private Transform slashAnimSpawnPoint;
    private float lastAttackTime;
    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;

    [SerializeField] protected float roamingRange = 5f;
    [SerializeField] protected float trackingRange = 3f;
    [SerializeField] protected float returnSpeedMultiplier = 2f;
    [SerializeField] protected float chasingSpeedMultiplier = 1.5f;
    [SerializeField] protected int damage = 1;
    [SerializeField] private Transform attackCollider;
    [SerializeField] private float attackRange = 1.5f;


    protected Transform playerTransform;
    protected Vector2 startingPosition;
    protected BossPathing enemyPathfinding;

    public float defaultMoveSpeed { get; set; }
    protected EnemyHealth health;

    public int currentHealth;
    public int startingHealth;
    [SerializeField] protected float HealthPoint = 0.6f;

    protected virtual void Awake()
    {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        enemyPathfinding = GetComponent<BossPathing>();

        health = GetComponent<EnemyHealth>();


        state = State.Roaming;
        startingPosition = transform.position;

        defaultMoveSpeed = enemyPathfinding.moveSpeed; 
       
    }

    protected virtual void Start()
    {
        currentHealth = health.currentHealth;
        startingHealth = health.startingHealth;
        StartCoroutine(StateRoutine());
        attackCollider.gameObject.SetActive(false);  // Tắt collider khi bắt đầu

    }
    public int GetDamage()
    {
        return damage;
    }

    protected virtual IEnumerator StateRoutine()
    {
        while (true)
        {

            playerTransform = EnemyTargetProvider.Instance.GetTarget();
            //Debug.Log("Positon: " + playerTransform.position);
            //yield return UpdateHealthStatus();
            Debug.Log($"Enemy: {transform.position}, Player: {playerTransform.position}");


            switch (state)
            {
                case State.Roaming:
                    enemyPathfinding.allowFlipByDirection = true;
                    yield return StartCoroutine(RoamingRoutine(playerTransform));
                    break;

                case State.ReturningToStart:
                    enemyPathfinding.allowFlipByDirection = true;
                    enemyPathfinding.moveSpeed = defaultMoveSpeed * returnSpeedMultiplier;
                    enemyPathfinding.MoveTo(startingPosition - (Vector2)transform.position);
                    if (Vector2.Distance(transform.position, startingPosition) < 0.2f)
                    {
                        enemyPathfinding.moveSpeed = defaultMoveSpeed;
                        state = State.Roaming;
                    }
                    break;

                case State.ChasingPlayer:
                    enemyPathfinding.allowFlipByDirection= false;
                    yield return StartCoroutine(ChasingPlayer(playerTransform));
                    break;

                //case State.RangeAttack:
                //    yield return StartCoroutine(RangeAttack(playerTransform));
                //    break;
            }

            yield return null;
        }
    }
    //private IEnumerator UpdateHealthStatus()
    //{
    //    if (health != null && playerTransform != null   )
    //    {
    //        int current = health.currentHealth;
    //        int starting = health.startingHealth;
    //        float distance = Vector2.Distance(transform.position, playerTransform.position);

    //        if (current > 0  && current <= (starting * HealthPoint)  && state != State.ChasingPlayer)
    //        {
    //            state = State.ChasingPlayer;
    //            yield break; // chuyển trạng thái ngay, không chờ hết vòng lặp
    //        }
    //    }
    //    yield return null;
    //}

    protected virtual IEnumerator RoamingRoutine(Transform playerTransform)
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


            //if (playerTransform != null && currentHealth >= (startingHealth * HealthPoint) &&
            //   Vector2.Distance(transform.position, playerTransform.position) <= trackingRange)
            if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= trackingRange)
                {
                state = State.ChasingPlayer;
                yield break;
            }

            yield return null;
        }
    }

    protected virtual IEnumerator ChasingPlayer(Transform playerTransform)
    {

        //while (playerTransform != null)
        //{
        //    //UpdatePlayerTransform();
        //    float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        //    //Debug.Log($"Enemy: {transform.position}, Player: {playerTransform.position}, Distance: {distanceToPlayer}");

        //    if (Time.time - lastAttackTime >= attackCooldown)
        //    {
        //        if (distanceToPlayer <= attackRange)
        //        {
        //            enemyPathfinding.moveSpeed = 0;
        //            Attack();
        //        }
        //        else if (distanceToPlayer <= trackingRange)
        //        {
        //            TryRangeAttack();
        //        }
        //    }
        //    else 
        //    //if (distanceToPlayer > attackRange)
        //    {
        //        Vector2 dirToPlayer = playerTransform.position - transform.position;
        //        enemyPathfinding.moveSpeed = defaultMoveSpeed * chasingSpeedMultiplier;
        //        enemyPathfinding.MoveTo(dirToPlayer);
        //    }

        //    if (Vector2.Distance(transform.position, startingPosition) > roamingRange)
        //    {
        //        yield return new WaitForSeconds(2f);
        //        state = State.ReturningToStart;
        //        yield break;
        //    }

        //    yield return null;
        //}


        if (playerTransform == null)
        {
            yield return null;
            yield break;
        }
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            //Debug.Log($"Enemy: {transform.position}, Player: {playerTransform.position}, Distance: {distanceToPlayer}");

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
            //if (distanceToPlayer > attackRange)
            {
                Vector2 dirToPlayer = playerTransform.position - transform.position;
                enemyPathfinding.moveSpeed = defaultMoveSpeed * chasingSpeedMultiplier;
                enemyPathfinding.MoveTo(dirToPlayer);
            }


            //if (distanceToPlayer <= attackRange)
            //{
            //    enemyPathfinding.moveSpeed = 0;
            //}

            if (Vector2.Distance(transform.position, startingPosition) > roamingRange)
            {
                yield return new WaitForSeconds(2f);
                state = State.ReturningToStart;
                yield break;
            }

            yield return null;
        

    

}

    //protected virtual IEnumerator RangeAttack(Transform playerTransform)
    //{
    //    if (playerTransform != null)
    //    {
    //        Vector2 dirToPlayer = playerTransform.position - transform.position;
    //        if (Vector2.Distance(transform.position, playerTransform.position) <= trackingRange)
    //        {
    //            enemyPathfinding.moveSpeed = 0;
    //            TryRangeAttack();

    //        }
    //        else
    //        {
    //            enemyPathfinding.moveSpeed = defaultMoveSpeed;
    //            enemyPathfinding.MoveTo(dirToPlayer);
    //        }
    //    }

    //    if (Vector2.Distance(transform.position, startingPosition) > roamingRange)
    //    {
    //        yield return new WaitForSeconds(2f);
    //        state = State.ReturningToStart;
    //    }



    //}

    
    public virtual void Attack()
    {
        //UpdatePlayerTransform();
        //if (playerTransform == null) return;
        //float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        //if (distanceToPlayer > attackRange) return;  // ❗ CHẶN ĐÁNH KHI ĐÃ RA XA


        ShouldFaceLeft();
        myAnimator.SetTrigger("Scratch");
        attackCollider.gameObject.SetActive(true);

        lastAttackTime = Time.time;
        StartCoroutine(AttackCooldownCoroutine());
    }


    protected virtual void TryRangeAttack()
    {
        //UpdatePlayerTransform();
        ////if (playerTransform == null) return;

        //if (Time.time - lastAttackTime < attackCooldown)  
        //    return; 

        //    lastAttackTime = Time.time;
        //    myAnimator.SetTrigger("Stomp");
        //    ShouldFaceLeft();
        //    StartCoroutine(DelayedProjectile());

        //    lastAttackTime = Time.time;
        //    //StartCoroutine(CooldownCoroutine());

        //UpdatePlayerTransform();
        //if (playerTransform == null) return;

        ShouldFaceLeft();
        myAnimator.SetTrigger("Stomp");
        StartCoroutine(DelayedProjectile());

        lastAttackTime = Time.time;

    }

    private void UpdatePlayerTransform()
    {
        playerTransform = EnemyTargetProvider.Instance.GetTarget();
        //Debug.Log(playerTransform.position);

    }

    private IEnumerator AttackCooldownCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        attackCollider.gameObject.SetActive(false);
    }

    //private IEnumerator CooldownCoroutine()
    //{
    //    yield return new WaitForSeconds(attackCooldown);
    //}

    private IEnumerator DelayScratchCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
    }
    private void ShouldFaceLeft()
    {
        bool shouldFaceLeft = playerTransform.position.x < transform.position.x;
        Vector3 scale = transform.localScale;
        scale.x = shouldFaceLeft ? -1 : 1;
        transform.localScale = scale;
    }
    private IEnumerator DelayedProjectile()
    {
        yield return new WaitForSeconds(0.5f);

        Instantiate(weaponProjectilePrefab, playerTransform.position, Quaternion.identity);
    }

    protected Vector2 GetRoamingPosition()
    {
        // T?o m?t h??ng ng?u nhiên (góc)
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float radius = Random.Range(0.5f, roamingRange);

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        return startingPosition + offset;
    }

}




