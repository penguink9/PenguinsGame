using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected TrailRenderer trailRenderer;

    protected Rigidbody2D rb;
    protected Animator myAnimator;
    protected SpriteRenderer mySpriteRender;
    protected KnockBack knockback;
    protected bool facingLeft = false;
    protected bool isDashing = false;
    protected float dashSpeed = 4f;
    protected bool isAttacking = false;
    protected Vector2 movement;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<KnockBack>();
        trailRenderer.emitting = false;
    }

    private void OnEnable()
    {
        PlayerInputManager.Instance.OnAttack += Attack;
        PlayerInputManager.Instance.OnDash += Dash;
    }

    private void OnDisable()
    {
        PlayerInputManager.Instance.OnAttack -= Attack;
        PlayerInputManager.Instance.OnDash -= Dash;
    }

    private void Update()
    {
        movement = PlayerInputManager.Instance.MoveInput;
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            AdjustPlayerFacingDirection();
            Move(movement);
        }
    }

    private void UpdateAnimator()
    {
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    public virtual void Move(Vector2 movement)
    {
        if (knockback.gettingKnockedBack || GetComponent<PlayerHealth>().isDead || isAttacking) return;
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    public virtual void Dash()
    {
        if (isDashing || knockback.gettingKnockedBack || GetComponent<PlayerHealth>().isDead) return;

        isDashing = true;
        moveSpeed *= dashSpeed;
        trailRenderer.emitting = true;

        StartCoroutine(DashCooldownCoroutine());
    }

    private IEnumerator DashCooldownCoroutine()
    {
        float dashDuration = 0.2f;
        float dashCD = 0.5f;
        yield return new WaitForSeconds(dashDuration);
        moveSpeed /= dashSpeed;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    public virtual void AdjustPlayerFacingDirection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySpriteRender.flipX = false;
            facingLeft = false;
        }
    }

    public virtual void Attack()
    {
        Debug.Log($"{gameObject.name} Attack Triggered");
        // Override ở từng class con
    }
}
