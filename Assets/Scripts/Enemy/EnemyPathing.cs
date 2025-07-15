using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    public float moveSpeed = 2f;

    protected Rigidbody2D rb;
    protected Vector2 moveDir;
    protected KnockBack knockback;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        knockback = GetComponent<KnockBack>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void FixedUpdate()
    {
        if (knockback.gettingKnockedBack) return;

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));

        if (moveDir.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveDir.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }
    }

    public virtual void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition.normalized;
    }
}
