using System.IO.Pipes;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;
    [SerializeField] private float fireAngleLimit = 60f;

    private Vector3 startPosition;
    private Transform target;
    private Vector3 fireDirection;
    private bool directionSet = false;

    private void Start()
    {
        startPosition = transform.position;

        if (isEnemyProjectile)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
        bool isDestructible = other.gameObject.GetComponent<Destructible>();

        if (!other.isTrigger && (enemyHealth || indestructible || player || isDestructible))
        {
            if (player && isEnemyProjectile)
            {
                player.TakeDamage(1, transform);
            } else if (enemyHealth)
            {
                enemyHealth.TakeDamage(1, transform);
            }

            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }
    // Method để set hướng bắn từ bên ngoài
    public void SetFireDirection(bool playerFacingLeft)
    {
        if (directionSet) return; // Tránh set nhiều lần

        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));
        mouseWorldPosition.z = transform.position.z;

        // Tính vector hướng từ projectile đến chuột
        Vector3 mouseDirection = (mouseWorldPosition - transform.position).normalized;

        // Lấy forward direction của player dựa trên hướng nhìn
        Vector3 playerForward = playerFacingLeft ? Vector3.left : Vector3.right;

        // Tính góc giữa hướng player và hướng chuột
        float angleToMouse = Vector3.SignedAngle(playerForward, mouseDirection, Vector3.forward);

        // Giới hạn góc bắn trong phạm vi ±60 độ từ hướng player
        if (Mathf.Abs(angleToMouse) > fireAngleLimit)
        {
            angleToMouse = Mathf.Clamp(angleToMouse, -fireAngleLimit, fireAngleLimit);
            fireDirection = Quaternion.Euler(0, 0, angleToMouse) * playerForward;
        }
        else
        {
            fireDirection = mouseDirection;
        }

        directionSet = true;
    }

    private void MoveProjectile()
    {
        if (!isEnemyProjectile && directionSet)
        {
            transform.Translate(fireDirection * Time.deltaTime * moveSpeed);
        }
        else if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * Time.deltaTime * moveSpeed);
        }
    }
}
