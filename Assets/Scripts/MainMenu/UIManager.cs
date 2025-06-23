using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class LevelData
{
    public bool isUnlocked = false;
    public bool isCompleted = false;
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [Header("Map/Level UI")]
    public GameObject mapSelectionPanel;
    public GameObject[] levelSelectionPanels;     // Panel chọn level cho từng map
    public MapSelection[] mapSelections;          // Các map (Map1, Map2, Map3, Map4)

    [Header("Level Buttons")]
    public LevelSelectionButton[][] levelButtons; // [mapIndex][levelIndex]



    // Trạng thái các level, quản lý hoàn toàn bằng code
    public LevelData[][] allLevels;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        InitLevelButtons();
        InitLevelData();
        LoadUnlockState();
        UpdateUI();
    }

    private void InitLevelButtons()
    {
        int mapCount = levelSelectionPanels.Length;
        levelButtons = new LevelSelectionButton[mapCount][];
        for (int m = 0; m < mapCount; m++)
        {
            // Lấy đúng thứ tự các LevelSelectionButton
            levelButtons[m] = levelSelectionPanels[m].GetComponentsInChildren<LevelSelectionButton>(true);
            // Lưu ý: nếu trong panel có cả các nút khác, cần kiểm tra lại thứ tự (nên đặt tên 0,1,2... hoặc kéo đúng thứ tự trong Editor)
        }
    }

    private void InitLevelData()
    {
        int mapCount = levelSelectionPanels.Length;
        allLevels = new LevelData[mapCount][];
        for (int m = 0; m < mapCount; m++)
        {
            int levelCount = levelButtons[m].Length;
            allLevels[m] = new LevelData[levelCount];
            for (int l = 0; l < levelCount; l++)
                allLevels[m][l] = new LevelData();
        }
    }

    // ========== UI UPDATE ==========
    public void UpdateUI()
    {
        // Cập nhật trạng thái Map (khoá/mở)
        for (int m = 0; m < mapSelections.Length; m++)
        {
            if (mapSelections[m].lockedGo != null)
                mapSelections[m].lockedGo.SetActive(!mapSelections[m].isUnlock);
            if (mapSelections[m].unlockedGo != null)
                mapSelections[m].unlockedGo.SetActive(mapSelections[m].isUnlock);
        }

        // Cập nhật trạng thái từng level (nên check null tránh lỗi)
        for (int m = 0; m < levelButtons.Length; m++)
        {
            for (int l = 0; l < levelButtons[m].Length; l++)
            {
                bool unlocked = allLevels[m][l].isUnlocked;
                bool completed = allLevels[m][l].isCompleted;
                if (levelButtons[m][l] != null)
                    levelButtons[m][l].SetState(unlocked, completed);
            }
        }
    }



    // ====== STATE UNLOCK/CLEAR ======
    public void UnlockLevel(int mapIndex, int levelIndex)
    {
        if (levelIndex < allLevels[mapIndex].Length - 1)
        {        
            allLevels[mapIndex][levelIndex + 1].isUnlocked = true;
        }
        else
        {
            if (mapIndex < mapSelections.Length - 1)
            {
                mapSelections[mapIndex + 1].isUnlock = true;
                if (allLevels[mapIndex + 1].Length > 0)
                    allLevels[mapIndex + 1][0].isUnlocked = true;
            }
        }
        SaveUnlockState();
        UpdateUI();
    }


    public void CompleteLevel(int mapIndex, int levelIndex)
    {
        allLevels[mapIndex][levelIndex].isCompleted = true;
        UnlockLevel(mapIndex, levelIndex);  
    }

    // ========== LƯU & TẢI TRẠNG THÁI ==========
    private void SaveUnlockState()
    {
        for (int m = 0; m < mapSelections.Length; m++)
        {
            PlayerPrefs.SetInt($"Map_{m}_Unlocked", mapSelections[m].isUnlock ? 1 : 0);
            for (int l = 0; l < allLevels[m].Length; l++)
            {
                PlayerPrefs.SetInt($"Map_{m}_Level_{l}_Unlocked", allLevels[m][l].isUnlocked ? 1 : 0);
                PlayerPrefs.SetInt($"Map_{m}_Level_{l}_Completed", allLevels[m][l].isCompleted ? 1 : 0);
            }
        }
        PlayerPrefs.Save();
    }

    private void LoadUnlockState()
    {
        for (int m = 0; m < mapSelections.Length; m++)
        {
            mapSelections[m].isUnlock = PlayerPrefs.GetInt($"Map_{m}_Unlocked", m == 0 ? 1 : 0) == 1;
            for (int l = 0; l < allLevels[m].Length; l++)
            {
                bool defaultUnlock = (m == 0 && l == 0);
                allLevels[m][l].isUnlocked = PlayerPrefs.GetInt($"Map_{m}_Level_{l}_Unlocked", defaultUnlock ? 1 : 0) == 1;
                allLevels[m][l].isCompleted = PlayerPrefs.GetInt($"Map_{m}_Level_{l}_Completed", 0) == 1;
            }
            allLevels[m][0].isUnlocked = mapSelections[m].isUnlock;
        }
    }

    // ===== PANEL SWITCHING =====
    public void PressMapButton(int mapIndex)
    {
        if (!mapSelections[mapIndex].isUnlock)
        {
            Debug.Log("Map is locked, please unlock it first.");
            return;
        }
        for (int i = 0; i < levelSelectionPanels.Length; i++)
        {
            // Bật panel đúng map, tắt các panel khác
            levelSelectionPanels[i].SetActive(i == mapIndex);
        }
        mapSelectionPanel.SetActive(false); // <- Cần dòng này để tắt panel chọn map!
    }

    public void BackMapButton()
    {
        mapSelectionPanel.SetActive(true);
        foreach (var panel in levelSelectionPanels)
            panel.SetActive(false); // <- Tắt hết panel chọn level!
    }


    // Được LevelSelectionButton gọi khi bấm nút
    public void PressLevelButton(int mapIndex, int levelIndex)
    {
        if (!allLevels[mapIndex][levelIndex].isUnlocked)
        {
            Debug.Log("Level is locked!");
            // Có thể show popup ở đây nếu muốn
            return;
        }

        // Ghép tên scene theo đúng cấu trúc của bạn:
        string sceneName = $"Level1_Map{levelIndex + 1}";
        Debug.Log("Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void SceneTransition(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // ====== KHI QUA MÀN ======
    // Gọi hàm này khi qua màn (win)
    public void OnLevelCompleted(int mapIndex, int levelIndex)
    {
        CompleteLevel(mapIndex, levelIndex);
    }

}
