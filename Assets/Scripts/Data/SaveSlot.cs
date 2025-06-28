using System;
using UnityEngine;

[System.Serializable]
public class SaveSlot
{
    public int slotNumber;
    public DateTime lastModified;
    public string slotName;
    public string playerName;
    public string fileName;
    public GameData gameData;
}
