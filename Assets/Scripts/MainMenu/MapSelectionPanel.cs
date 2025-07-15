using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelectionPanel : MonoBehaviour, ILoadGameInit
{
    [SerializeField] private Button[] mapButtons; // Các nút chọn map

    private void Start()
    {
        // Khởi tạo các nút map, có thể thêm logic nếu cần
        mapButtons[0].interactable = true; // Map 1 luôn mở khóa
        mapButtons[1].interactable = false; // Map 2 khóa
        mapButtons[2].interactable = false; // Map 3 khóa
        LoadGameInit();
    }
    public void LoadGameInit()
    {
        SaveSlot slot = DataManager.Instance.GetLoadedSlot();
        if (slot == null)
        {
            Debug.Log("No save slot loaded.");
            return;
        }
        // Lấy thông tin các map từ SaveSlot
        foreach(ScoreRecord scoreRecord in slot.scores)
        {
            UnlockMap(mapButtons[scoreRecord.levelIndex-1]);
            DisplayStat(mapButtons[scoreRecord.levelIndex - 1], scoreRecord.score);
        }
        UnlockMap(mapButtons[slot.gameData.level]); // Mở khóa map tiếp theo
        DataManager.Instance.SetLoadedSlot(null);
    }
    public void SceneTransition(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void UnlockMap(Button mapButton)
    {
        mapButton.transform.GetChild(0).gameObject.SetActive(false); // Deactivate the lock icon
        mapButton.interactable = true; // Enable the button
    }
    public void DisplayStat(Button mapButton, int coinRecorder)
    {
        // Hiển thị thông tin thống kê cho map
        mapButton.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Player:" +PlayerPrefs.GetString("PlayerName", "DefaultName");
        // Map CoinRecorder
        mapButton.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = coinRecorder.ToString();
        mapButton.transform.GetChild(1).gameObject.SetActive(true);
    }

    
}
