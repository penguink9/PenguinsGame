

using System.Collections;
using UnityEngine;

public class ThrowEnemyAI : EnemyAI
{
    [Header("Monkey AI Settings")]
    [SerializeField] private float attackCooldown = 2f;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject weaponProjectilePrefab;
    private float lastAttackTime;

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake(); // Gọi phương thức Awake của lớp cha (EnemyAI)
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastAttackTime = -attackCooldown;
    }

    protected override IEnumerator ChasingPlayer(Transform playerTransform)
    {
        if (playerTransform != null)
        {
            Vector2 dirToPlayer = playerTransform.position - transform.position;
            if (Vector2.Distance(transform.position, playerTransform.position) <= trackingRange)
            {
                enemyPathfinding.moveSpeed = 0;
                TryAttack();
                
            }
            else
            {
                enemyPathfinding.moveSpeed = defaultMoveSpeed ;
                enemyPathfinding.MoveTo(dirToPlayer);
            }
        }

        if (Vector2.Distance(transform.position, startingPosition) > roamingRange)
        {
            // vẫn tiếp tục đuổi thêm 2s
            yield return new WaitForSeconds(2f);
            state = State.ReturningToStart;
        }

        

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

            StartCoroutine(DelayedProjectile());

            lastAttackTime = Time.time;
        }
    }
    private IEnumerator DelayedProjectile()
    {
        yield return new WaitForSeconds(0.5f);

        Instantiate(weaponProjectilePrefab, transform.position, Quaternion.identity);
    }
}