using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected TrailRenderer trailRenderer;
    public static PlayerBase Instance;
    protected Rigidbody2D rb;
    protected Animator myAnimator;
    protected SpriteRenderer mySpriteRender;
    protected KnockBack knockback;
    protected bool facingLeft = false;
    protected bool isDashing = false;
    protected float dashSpeed = 4f;
    protected PlayerController playerControls;  

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<KnockBack>();
        trailRenderer.emitting = false;
        playerControls = new PlayerController();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public virtual void Move(Vector2 movement)
    {
        if (knockback.gettingKnockedBack || GetComponent<PlayerHealth>().isDead) return;
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
        float dashDuration = 0.2f; // Thời gian dashes
        float dashCD = 0.5f; // Thời gian cooldown dashes
        yield return new WaitForSeconds(dashDuration);
        moveSpeed /= dashSpeed;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false; // Kết thúc trạng thái dashes
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
}

