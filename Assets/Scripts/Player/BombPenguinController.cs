using UnityEngine;
using System.Collections;

public class BombPenguinController : PlayerBase
{


    [SerializeField] Transform bombSpawnPoint;
    [SerializeField] private GameObject bombProjectilePrefab;
    private Vector2 movement;
    private bool isAttacking = false;
    private float attackCooldown = 0.8f;
    private float lastAttackTime = 0f;

    private void Start()
    {
        // Gán sự kiện từ playerControls cho các phương thức Attack và Dash
        playerControls.Combat.Attack.started += _ => Attack();
        playerControls.Combat.Dash.started += _ => Dash();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        // Chỉ cho phép di chuyển nếu không đang tấn công
        if (!isAttacking)
        {
            AdjustPlayerFacingDirection();
            Move(movement);  // Sử dụng phương thức Move từ PlayerBase
        }
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    public void Attack()
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


    public override void Move(Vector2 movement)
    {
        base.Move(movement);
    }


}
