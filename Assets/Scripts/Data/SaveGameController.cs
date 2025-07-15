using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveGameController : MonoBehaviour
{
    [SerializeField] GameObject slot1Container;
    [SerializeField] GameObject slot2Container;
    [SerializeField] GameObject slot3Container;
    [SerializeField] GameObject slot4Container;

    private void OnEnable()
    {
        StartUIState();
        SetUpUISlot();
    }
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
    public void ActiveEmptySlot(GameObject obj)
    {
        obj.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        obj.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        obj.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
        obj.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
    }
    public void ActiceSlot(GameObject obj)
    {
        obj.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        obj.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        obj.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        obj.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
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
        SetUpUISlot();
    }
    public void OnClickSaveSlot(int slotNumber)
    {
        if(MapStateManager.Instance.IsMissionCompleted() && MapStateManager.Instance.EntryExitArea)
        {
            SaveGameAfterCompleted(slotNumber);
        } else
        {
            SaveGameToSlot(slotNumber);
        }        
    }
    public void SaveGameAfterCompleted(int slotNumber)
    {
        ScoreRecord record = new ScoreRecord
        {
            levelIndex = TrackCurrentMap.Instance.level,
            score = CoinRecorder.Instance.TotalCoins
        };
        int currentSlot = PlayerPrefs.GetInt("CurrentSlot", 0);
        Debug.Log("Current slot number:" + currentSlot);
        List<ScoreRecord> scores = new List<ScoreRecord>();
        if (currentSlot!= 0)
        {
            DataManager.Instance.RefreshAllSlots();
            scores = DataManager.Instance.LoadDataInSlot(currentSlot).scores;
            // Ghi đè nếu đã có record cùng levelIndex, ngược lại thêm mới
            int index = scores.FindIndex(r => r.levelIndex == record.levelIndex);
            if (index >= 0)
                scores[index] = record;
            else
                scores.Add(record);
        } else
        {
            scores.Add(record);
        }
        DataManager.Instance.SaveGameAfterCompletedLevel(scores, slotNumber, TrackCurrentMap.Instance.level);
        DataManager.Instance.DestroyManagerInLevel();
        SceneManager.LoadScene("MapSelection");
    }
    public void StartUIState()
    {
        for(int i = 1; i <= 4; i++)
        {
            GameObject slotContainer = null;
            switch (i)
            {
                case 1:
                    slotContainer = slot1Container;
                    break;
                case 2:
                    slotContainer = slot2Container;
                    break;
                case 3:
                    slotContainer = slot3Container;
                    break;
                case 4:
                    slotContainer = slot4Container;
                    break;
            }
            ActiveEmptySlot(slotContainer);
        }
    }
    public void SetUpUISlot()
    {
        List<SaveSlot> slots = DataManager.Instance.GetAllSaveSlots();

        foreach (SaveSlot slot in slots)
        {
            GameObject slotContainer = null;
            if (slot != null)
            {
                switch (slot.slotNumber)
                {
                    case 1:
                        slotContainer = slot1Container;
                        break;
                    case 2:
                        slotContainer = slot2Container;
                        break;
                    case 3:
                        slotContainer = slot3Container;
                        break;
                    case 4:
                        slotContainer = slot4Container;
                        break;
                }
                ActiceSlot(slotContainer);
                GetPlayerNameText(slotContainer).text = slot.playerName;
                if(slot.gameData.currentMap != 0)
                {
                    GetLevelText(slotContainer).text = "Level " + slot.gameData.level.ToString() + " - Map " + slot.gameData.currentMap.ToString();
                } else
                {
                    GetLevelText(slotContainer).text = "Level " + slot.gameData.level.ToString();
                }
                GetDateText(slotContainer).text = slot.lastModified;
            }
        }
    }
}
