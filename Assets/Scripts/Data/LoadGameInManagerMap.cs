using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class LoadGameInManagerMap : MonoBehaviour
{
    [SerializeField] private int level;
    private void Start()
    {
        // Check if the current scene is the one we want to load the game in
        if (DataManager.Instance.GetLoadedSlot() == null || DataManager.Instance.GetLoadedSlot().gameData.level != level) return;

        // Load the game data for the specified level
        GameData gameData = DataManager.Instance.GetLoadedSlot().gameData;
        CoinRecorder.Instance.TotalCoins = gameData.coinCollected;
        InventoryManager.Instance.HealthPotions = gameData.healthPotions;
        InventoryManager.Instance.KeyCount = gameData.keys;
        PlayerManager.Instance.SetLoadData(gameData.unlockCharacters, gameData.activeCharacterIndex);
        MapStateManager.Instance.SetLoadData(gameData.mapStates, gameData.npcUnlockeds);
        if(gameData.currentMap == TrackCurrentMap.Instance.map)
        {
            PlayerManager.Instance.GetActivePlayer().transform.position = gameData.playerPosition;
            DataManager.Instance.SetLoadedSlot(null);
        } else
        {
            string loadScene = "Level" + gameData.level + "_Map" + gameData.currentMap;
            SceneManager.LoadScene(loadScene);
        }
    }
}
