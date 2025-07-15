public class KeyPickup : Pickup
{
    public override void OnPickup()
    {
        InventoryManager.Instance.PickupKey();
    }
}
