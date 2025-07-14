using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    protected enum State
    {
        Roaming,
        ReturningToStart,
        ChasingPlayer
    }

    protected State state;

    [SerializeField] protected float roamingRange = 5f;
    [SerializeField] protected float trackingRange = 3f;
    [SerializeField] protected float returnSpeedMultiplier = 2f;
    [SerializeField] protected float chasingSpeedMultiplier = 1.5f;
    [SerializeField] protected int damage = 1;

    protected Transform playerTransform;
    protected Vector2 startingPosition;
    protected EnemyPathing enemyPathfinding;
    public float defaultMoveSpeed { get; set; }

    protected virtual void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathing>();
        state = State.Roaming;
        startingPosition = transform.position;
        

        defaultMoveSpeed = enemyPathfinding.moveSpeed; // cần khai báo public get trong EnemyPathing
    }

    protected virtual void Start()
    {
        StartCoroutine(StateRoutine());
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
            switch (state)
            {
                case State.Roaming:
                    yield return StartCoroutine(RoamingRoutine(playerTransform));
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

                case State.ChasingPlayer:
                    yield return StartCoroutine(ChasingPlayer(playerTransform));
                    break;
            }

            if (playerTransform != null && state == State.Roaming)
            {
                if (Vector2.Distance(transform.position, playerTransform.position) <= trackingRange)
                {
                    state = State.ChasingPlayer;
                }
            }

            yield return null;
        }
    }

    protected virtual IEnumerator ChasingPlayer(Transform playerTransform)
    {
        if (playerTransform != null)
        {
            Vector2 dirToPlayer = playerTransform.position - transform.position;
            enemyPathfinding.moveSpeed = defaultMoveSpeed * chasingSpeedMultiplier;
            enemyPathfinding.MoveTo(dirToPlayer);
        }

        if (Vector2.Distance(transform.position, startingPosition) > roamingRange)
        {
            // vẫn tiếp tục đuổi thêm 2s
            yield return new WaitForSeconds(2f);
            state = State.ReturningToStart;
        }
        
    }

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

            if (playerTransform != null &&
                Vector2.Distance(transform.position, playerTransform.position) <= trackingRange)
            {
                state = State.ChasingPlayer;
                yield break;
            }

            yield return null;
        }
    }


    protected Vector2 GetRoamingPosition()
    {
        // Tạo một hướng ngẫu nhiên (góc)
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float radius = Random.Range(0.5f, roamingRange);

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        return startingPosition + offset;
    }

}
