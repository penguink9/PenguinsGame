using System;
using System.Collections.Generic;

[System.Serializable]
public class SaveSlot
{
    public int slotNumber;
    public string lastModified;
    public string slotName;
    public string playerName;
    public string fileName;
    public GameData gameData;
    public bool isLevelCompleted;
    public List<ScoreRecord> scores;

    public DateTime LastModified
    {
        get => DateTime.TryParse(lastModified, out var dt) ? dt : DateTime.MinValue;
        set => lastModified = value.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
