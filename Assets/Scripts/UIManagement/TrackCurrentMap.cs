using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackCurrentMap : Singleton<TrackCurrentMap> 
{
    private TextMeshProUGUI mapText;
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
            string levelPart = parts[0].Replace("Level", ""); // "1"
            string mapPart = parts[1].Replace("Map", "");     // "1"

            mapText.text = $"Level {levelPart} - Map {mapPart}";
            Debug.Log($"Current Level: {levelPart}, Map: {mapPart}"); // Debug log để kiểm tra
        }
        else
        {
            // Trường hợp lỗi tên scene
            mapText.text = "Unknown Level";
        }
    }
}
