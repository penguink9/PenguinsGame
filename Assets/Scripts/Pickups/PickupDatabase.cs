using UnityEngine;

[CreateAssetMenu(fileName = "PickupDatabase", menuName = "Scriptable Objects/PickupDatabase")]
public class PickupDatabase : ScriptableObject
{
    public GameObject[] pickupPrefabs;
}
