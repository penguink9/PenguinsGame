using Unity.VisualScripting;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.GetComponent<EnemyHealth>())
        //{
        //    EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        //    enemyHealth.TakeDamage(damageAmount, transform);
        //}

        Transform parent = transform.parent;

        if (parent != null && parent.CompareTag("Player"))
        {
            // Gây damage cho enemy nếu đây là đòn đánh từ Player
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount, transform);
            }
        }

        else
        {
            // Gây damage cho người chơi nếu không phải từ Player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount, transform);
            }
        }




    }
}
