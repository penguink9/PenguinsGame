using UnityEngine;
using System.Collections;

public class BombPenguinController : PlayerBase
{
    [SerializeField] Transform bombSpawnPoint;
    [SerializeField] private GameObject bombProjectilePrefab;
    private float attackCooldown = 0.8f;
    private float lastAttackTime = 0f;

    private void Start()
    {
    }
    public override void Attack()
    {
        if ((Time.time - lastAttackTime >= attackCooldown) && !isAttacking)
        {
            isAttacking = true;
            rb.linearVelocity = Vector2.zero;
            myAnimator.SetTrigger("Attack");
            Vector3 spawnPos = bombSpawnPoint.position;
            spawnPos.x += facingLeft ? -1.5f : 0f;
            Instantiate(bombProjectilePrefab, transform.position, Quaternion.identity);
            // Spawn projectile và set direction
            lastAttackTime = Time.time;
            StartCoroutine(AttackCooldownCoroutine());
        }

    }

    private IEnumerator AttackCooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}
