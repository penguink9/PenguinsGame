using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.DefaultInputActions;

public class NormalPenguinController : PlayerBase
{
    private Vector2 movement;

    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform attackCollider;

    private bool isAttacking = false;
    private float attackCooldown = 0.5f;
    private float lastAttackTime = 0f;
    private GameObject slashAnim;

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
        movement = playerControls.Movement.Move.ReadValue<Vector2>();  // Đọc dữ liệu từ playerControls
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    public void Attack()
    {
        if ((Time.time - lastAttackTime >= attackCooldown) && !isAttacking)
        {
            isAttacking = true;
            rb.linearVelocity = Vector2.zero;
            AdjustPlayerFacingDirection();
            myAnimator.SetTrigger("Attack");
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
        isAttacking = false;
        attackCollider.gameObject.SetActive(false);
    }

    public void FlipSlashAnim()
    {
        if (slashAnim != null)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = facingLeft;
        }
    }

    public override void Move(Vector2 movement)
    {
        base.Move(movement);  // Gọi lại phương thức Move từ PlayerBase
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

