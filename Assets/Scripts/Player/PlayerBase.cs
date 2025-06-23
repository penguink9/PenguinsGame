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
    protected PlayerState playerState;
    protected bool facingLeft = false;
    protected float dashSpeed = 4f;
    protected Vector2 movement;
    protected bool canDash = true;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        playerState = GetComponent<PlayerState>();
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
        if (playerState.CurrentState != PlayerState.State.Attacking &&
            playerState.CurrentState != PlayerState.State.TakingDamage &&
            playerState.CurrentState != PlayerState.State.Dead)
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
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
        playerState.CurrentState = movement != Vector2.zero ? PlayerState.State.Moving : PlayerState.State.Idle;
    }

    public virtual void Dash()
    {
        if (!canDash || playerState.CurrentState != PlayerState.State.Moving)
            return;
        canDash = false;
        playerState.CurrentState = PlayerState.State.Dashing;
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
        playerState.CurrentState = PlayerState.State.Idle;
        canDash = true;
    }

    public virtual void AdjustPlayerFacingDirection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        mySpriteRender.flipX = mousePos.x < playerScreenPoint.x;
        facingLeft = mySpriteRender.flipX;
    }

    public virtual void Attack()
    {
        if (playerState.CurrentState == PlayerState.State.Dead ||
            playerState.CurrentState == PlayerState.State.Attacking)
            return;

        playerState.CurrentState = PlayerState.State.Attacking;
        // Additional attack behavior here

    }
}

