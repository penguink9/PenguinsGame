using UnityEngine;

public class BossPathing : EnemyPathing
{
    public bool allowFlipByDirection = true;

    protected override void FixedUpdate()
    {
        if (knockback.gettingKnockedBack) return;

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));

        if (allowFlipByDirection)
        {
            Vector3 scale = transform.localScale;

            if (moveDir.x > 0.01f)
            {
                scale.x = 1;
                transform.localScale = scale;
            }
            else if (moveDir.x < -0.01f)
            {
                scale.x = -1;
                transform.localScale = scale;
            }
        }
    }
}
