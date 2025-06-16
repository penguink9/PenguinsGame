using UnityEngine;

public class HealthPickup : Pickup
{
    public override void OnPickup()
    {
        InventoryManager.Instance.PickupHealPotions();
    }
}
