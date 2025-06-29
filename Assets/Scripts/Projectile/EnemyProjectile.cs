using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private int damage = 1;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 playerPos = player.transform.position;
            StartCoroutine(ProjectileStraightRoutine(transform.position, playerPos));
        }
    }

    private IEnumerator ProjectileStraightRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float t = timePassed / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        Indestructible indestructible = other.GetComponent<Indestructible>();
        bool isDestructible = other.GetComponent<Destructible>() != null;

        if (!other.isTrigger && (player != null || indestructible != null || isDestructible))
        {
            if (player != null && isEnemyProjectile)
            {
                player.TakeDamage(damage, transform);
            }

            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
