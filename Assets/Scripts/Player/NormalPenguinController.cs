using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using static UnityEngine.InputSystem.DefaultInputActions;

public class NormalPenguinController : PlayerBase
{

    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform attackCollider;
    private float attackCooldown = 0.5f;
    private float lastAttackTime = 0f;
    private GameObject slashAnim;

    private void Start()
    {
        attackCollider.gameObject.SetActive(false);  // Tắt collider khi bắt đầu
    }
    public override void Attack()
    {
        if ((Time.time - lastAttackTime >= attackCooldown) && !(playerState.CurrentState == PlayerState.State.Attacking))
        {
            playerState.CurrentState = PlayerState.State.Attacking;
            rb.linearVelocity = Vector2.zero;
            AdjustPlayerFacingDirection();
            myAnimator.SetTrigger("Attack");
            AudioManager.Instance.PlaySFX("NormalPenguin Attacking");
            attackCollider.gameObject.SetActive(true);

            Vector3 spawnPosition = slashAnimSpawnPoint.position;
            spawnPosition.x += facingLeft ? -1f : 0.5f;
            slashAnim = Instantiate(slashAnimPrefab, spawnPosition, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;
            FlipSlashAnim();

            lastAttackTime = Time.time;
            StartCoroutine(AttackCooldownCoroutine());
        }
    }

    private IEnumerator AttackCooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        playerState.CurrentState = PlayerState.State.Idle;
        attackCollider.gameObject.SetActive(false);
    }

    public void FlipSlashAnim()
    {
        if (slashAnim != null)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = facingLeft;
        }
    }
    public void DoneAttackingAnimEvent()
    {
        attackCollider.gameObject.SetActive(false);
    }

    public override void AdjustPlayerFacingDirection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
            facingLeft = true;
            attackCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            mySpriteRender.flipX = false;
            facingLeft = false;
            attackCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}

