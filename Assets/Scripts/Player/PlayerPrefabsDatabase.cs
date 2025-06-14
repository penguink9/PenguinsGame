using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPrefabsDatabase", menuName = "Scriptable Objects/PlayerPrefabsDatabase")]
public class PlayerPrefabsDatabase : ScriptableObject
{
    public GameObject[] playerPrefabs;
}
