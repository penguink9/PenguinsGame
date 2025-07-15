using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameController : MonoBehaviour
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
    public TextMeshProUGUI GetPlayerNameText(GameObject obj)
    {
        return obj.transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    public TextMeshProUGUI GetLevelText(GameObject obj)
    {
        return obj.transform.GetChild(2).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    public TextMeshProUGUI GetDateText(GameObject obj)
    {
        return obj.transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    public TextMeshProUGUI GetSlotNameText(GameObject obj)
    {
        return obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public GameObject GetEmptyText(GameObject obj)
    {
        return obj.transform.GetChild(1).gameObject;
    }
    public void StartUIState()
    {
        for (int i = 1; i <= 4; i++)
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
    public void ActiveEmptySlot(GameObject obj)
    {
        GetEmptyText(obj).SetActive(true);
        obj.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        obj.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        obj.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
        obj.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);

    }
    public void ActiceSlot(GameObject obj)
    {
        GetEmptyText(obj).SetActive(false);
        obj.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        obj.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
        obj.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
        obj.transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
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
                if (slot.gameData.currentMap != 0)
                {
                    GetLevelText(slotContainer).text = "Level " + slot.gameData.level.ToString() + " - Map " + slot.gameData.currentMap.ToString();
                }
                else
                {
                    GetLevelText(slotContainer).text = "Level " + slot.gameData.level.ToString();
                }
                GetDateText(slotContainer).text = slot.lastModified;
            }
        }
    }
    public void LoadGameFromSlot(int slotNumber)
    {
        SaveSlot slot = DataManager.Instance.LoadDataInSlot(slotNumber);
        if (slot != null)
        {
            DataManager.Instance.SetLoadedSlot(slot);
            PlayerPrefs.SetString("PlayerName", slot.playerName);
            PlayerPrefs.SetInt("CurrentSlot", slot.slotNumber);
            if (slot.isLevelCompleted)
            {
                SceneManager.LoadScene("MapSelection");
            } else
            {
                GameData gameData = slot.gameData;
                string firstSceneLoad = "Level" + gameData.level + "_Map1";
                SceneManager.LoadScene(firstSceneLoad);
            }
        }
        else
        {
            Debug.LogWarning($"No data found in slot {slotNumber}");
        }

    }
    public void OnClickLoadSlot(int slotNumber)
    {
        LoadGameFromSlot(slotNumber);
    }
}
