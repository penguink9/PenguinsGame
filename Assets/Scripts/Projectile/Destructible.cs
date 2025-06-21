using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float healthDropChance = 0.5f; // 50% chance to drop health on death
    [SerializeField] private int maxCoins = 3; // Amount of max coins to drop
    private void OnTriggerEnter2D(Collider2D collision)
    {
            // Gọi hàm DestroyObject để phá hủy đối tượng này
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            SpawnPickup();
            Destroy(gameObject);
    }
    public void SpawnPickup()
    {
        if (Random.value < healthDropChance)
        {
            PickupSpawner.Instance.SpawnHealthPotions(1, transform.position);
        }
        int coinsToDrop = Random.Range(1, maxCoins + 1);
        PickupSpawner.Instance.SpawnCoins(coinsToDrop, transform.position);
    }
    public ItemState SaveState()
    {
        return new ItemState { id = name, isDestroyed = false };
    }
}
