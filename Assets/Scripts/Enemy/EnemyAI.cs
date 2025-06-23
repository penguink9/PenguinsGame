using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        ReturningToStart,
        ChasingPlayer
    }

    private State state;

    [SerializeField] private float roamingRange = 5f;
    [SerializeField] private float trackingRange = 3f;
    [SerializeField] private float returnSpeedMultiplier = 2f;
    [SerializeField] private float chasingSpeedMultiplier = 1.5f;
    [SerializeField] private int damage = 1;

    private Vector2 startingPosition;
    private EnemyPathing enemyPathfinding;
    public float defaultMoveSpeed { get; set; }

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathing>();
        state = State.Roaming;
        startingPosition = transform.position;
        

        defaultMoveSpeed = enemyPathfinding.moveSpeed; // cần khai báo public get trong EnemyPathing
    }

    private void Start()
    {
        StartCoroutine(StateRoutine());
    }
    public int GetDamage()
    {
        return damage;
    }
    private IEnumerator StateRoutine()
    {
        while (true)
        {
            Transform playerTransform = EnemyTargetProvider.Instance.GetTarget();
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
                    if (playerTransform != null)
                    {
                        Vector2 dirToPlayer = playerTransform.position - transform.position;
                        enemyPathfinding.moveSpeed = defaultMoveSpeed*chasingSpeedMultiplier;
                        enemyPathfinding.MoveTo(dirToPlayer);
                    }

                    if (Vector2.Distance(transform.position, startingPosition) > roamingRange)
                    {
                        // vẫn tiếp tục đuổi thêm 2s
                        yield return new WaitForSeconds(2f);
                        state = State.ReturningToStart;
                    }
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

    private IEnumerator RoamingRoutine(Transform playerTransform)
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


    private Vector2 GetRoamingPosition()
    {
        // Tạo một hướng ngẫu nhiên (góc)
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float radius = Random.Range(0.5f, roamingRange);

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        return startingPosition + offset;
    }

}
