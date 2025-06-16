using UnityEngine;

public class PickupSpawner : Singleton<PickupSpawner>
{
    [SerializeField] private PickupDatabase pickupDatabase;

    // Spawn coin
    public void SpawnCoins(int amount, Vector3 position)
    {
        SpawnPickup(pickupDatabase.pickupPrefabs[0], amount, position);
    }

    // Spawn health potion
    public void SpawnHealthPotions(int amount, Vector3 position)
    {
        SpawnPickup(pickupDatabase.pickupPrefabs[1], amount, position);
    }

    // Spawn key
    public void SpawnKeys(int amount, Vector3 position)
    {
        SpawnPickup(pickupDatabase.pickupPrefabs[2], amount, position);
    }

    // Hàm chung để spawn bất kỳ pickup nào
    private void SpawnPickup(GameObject pickupPrefab, int amount, Vector3 position)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(pickupPrefab, position + Random.insideUnitSphere * 2f, Quaternion.identity);
        }
    }
}
