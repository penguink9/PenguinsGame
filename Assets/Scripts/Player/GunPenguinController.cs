using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunPenguinController : PlayerBase
{
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject bulletPrefab;
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
            Vector3 spawnPos = bulletSpawnPoint.position;
            spawnPos.x += facingLeft ? -1.5f : 0f;
            // Spawn projectile và set direction
            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            Projectile projectileScript = bullet.GetComponent<Projectile>();
            projectileScript.SetFireDirection(facingLeft);
            AudioManager.Instance.PlaySFX("Shooting");
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
