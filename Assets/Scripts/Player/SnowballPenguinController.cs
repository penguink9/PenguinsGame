using UnityEngine;
using System.Collections;
public class SnowballPenguinController : PlayerBase
{
    [SerializeField] Transform snowballSpawnPoint;
    [SerializeField] private GameObject snowballProjectilePrefab;
    private float attackCooldown = 0.8f;
    private float lastAttackTime = 0f;

    private void Start()
    {
    }

    public override void Attack()
    {
        if ((Time.time - lastAttackTime >= attackCooldown) && !(playerState.CurrentState == PlayerState.State.Attacking))
        {
            playerState.CurrentState = PlayerState.State.Attacking;
            rb.linearVelocity = Vector2.zero;
            myAnimator.SetTrigger("Attack");
            AudioManager.Instance.PlaySFX("Throwing");
            Vector3 spawnPos = snowballSpawnPoint.position;
            spawnPos.x += facingLeft ? -1.5f : 0f;
            //Instantiate(snowballProjectilePrefab, transform.position, Quaternion.identity);
            Instantiate(snowballProjectilePrefab, spawnPos, Quaternion.identity);

            // Spawn projectile và set direction
            lastAttackTime = Time.time;
            StartCoroutine(AttackCooldownCoroutine());
        }

    }

    private IEnumerator AttackCooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        playerState.CurrentState = PlayerState.State.Idle;
    }
}
