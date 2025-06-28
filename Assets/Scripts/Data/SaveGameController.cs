using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SaveGameController : MonoBehaviour
{
    [SerializeField] GameObject slot1Container;
    [SerializeField] GameObject slot2Container;
    [SerializeField] GameObject slot3Container;
    [SerializeField] GameObject slot4Container;

    public void OnClickBackButton(GameObject obj)
    {
        obj.SetActive(false);
    }

    public TextMeshProUGUI GetPlayerNameText(GameObject obj)
    {
        return obj.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    public TextMeshProUGUI GetLevelText(GameObject obj)
    {
        return obj.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    public TextMeshProUGUI GetDateText(GameObject obj)
    {
        return obj.transform.GetChild(1).GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    public TextMeshProUGUI GetSlotNameText(GameObject obj)
    {
        return obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public GameObject GetEmptyText(GameObject obj)
    {
        return obj.transform.GetChild(1).GetChild(0).gameObject;
    }
    public GameData CollectGameData()
    {
        GameData gameData = new GameData();
        gameData.level = TrackCurrentMap.Instance.level;
        gameData.currentMap = TrackCurrentMap.Instance.map;
        MapStateManager.Instance.SaveMapState(gameData.currentMap);
        gameData.coinCollected = CoinRecorder.Instance.TotalCoins;
        gameData.playerPosition = PlayerManager.Instance.GetActivePlayer().transform.position;
        gameData.unlockCharacters = PlayerManager.Instance.GetCharacterDatas();
        gameData.activeCharacterIndex = PlayerManager.Instance.GetActivePlayerIndex();
        gameData.mapStates = MapStateManager.Instance.GetAllMapStates();
        gameData.healthPotions = InventoryManager.Instance.HealthPotions;
        gameData.keys = InventoryManager.Instance.KeyCount;
        gameData.npcUnlockeds = MapStateManager.Instance.GetCapturedPenguinState();
        return gameData;
    }

    public void SaveGameToSlot(int slotIndex)
    {
        GameData gameData = CollectGameData();
        DataManager.Instance.SaveGameToSlot(slotIndex, gameData);
    }
    public void OnClickSaveSlot(int slotNumber)
    {
        SaveGameToSlot(slotNumber);
    }
}
