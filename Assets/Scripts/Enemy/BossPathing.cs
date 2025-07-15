using UnityEngine;

public class BossPathing : MonoBehaviour
{
    public float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private KnockBack knockback;


    private SpriteRenderer spriteRenderer;
    private Vector3 scale;
    public bool allowFlipByDirection = true;

    private void Awake()
    {
        knockback = GetComponent<KnockBack>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (knockback.gettingKnockedBack) { return; }

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));

        //if (moveDir.x > 0.01f)
        //{
        //    scale = transform.localScale;
        //    //scale.x = Mathf.Abs(scale.x) * (shouldFaceLeft ? -1 : 1);
        //    scale.x = -1;
        //    transform.localScale = scale;
        //}
        //else if (moveDir.x < -0.01f)
        //{
        //    scale = transform.localScale;

        //    scale.x = 1;
        //    transform.localScale = scale;
        //}
        if (allowFlipByDirection)
        {
            if (moveDir.x > 0.01f)
            {
                Vector3 scale = transform.localScale;
                scale.x = 1;
                transform.localScale = scale;
            }
            else if (moveDir.x < -0.01f)
            {
                Vector3 scale = transform.localScale;
                scale.x = -1;
                transform.localScale = scale;
            }

        }

        
    }
    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition.normalized;
    }
}
