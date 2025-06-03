using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDir;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
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

    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition.normalized; 
    }
}
