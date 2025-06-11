using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunPenguinController : PlayerBase
{
    private Vector2 movement;
    private bool isAttacking = false;
    private float attackCooldown = 0.5f;
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
