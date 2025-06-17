using UnityEngine;

public class CoinPickup : Pickup
{
    public override void OnPickup()
    {
        CoinRecorder.Instance.PickupCoin();
    }
}
