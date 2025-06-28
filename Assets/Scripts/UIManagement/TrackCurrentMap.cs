using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackCurrentMap : Singleton<TrackCurrentMap> 
{
    private TextMeshProUGUI mapText;
    public int level;
    public int map;
    private void Start()
    {
        mapText = GetComponent<TextMeshProUGUI>();
        if (mapText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on this GameObject.");
            return;
        }
        UpdateLevelText();
    }
    public void UpdateLevelText()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Tách phần số từ tên scene
        string[] parts = sceneName.Split('_'); // ["Level1", "Map1"]

        if (parts.Length == 2)
        {
            level =  int.Parse(parts[0].Replace("Level", "")); // "1"
            map = int.Parse(parts[1].Replace("Map", ""));     // "1"

            mapText.text = $"Level {level} - Map {map}";
            Debug.Log($"Current Level: {level}, Map: {map}"); // Debug log để kiểm tra
        }
        else
        {
            // Trường hợp lỗi tên scene
            mapText.text = "Unknown Level";
        }
    }
}
