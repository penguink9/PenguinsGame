﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [Header("File Storage Config")]
    [SerializeField] private bool useEncryption;

    private List<SaveSlot> slots = new List<SaveSlot>();
    private FileDataHandler fileDataHandler;
    private SaveSlot loadedSlot;
    private int currentSlotNumber = -1;

    private const int MaxSlots = 4;
    private const string SlotFilePrefix = "save_slot_";

    protected override void Awake()
    {
        base.Awake();
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, useEncryption);
    }

    /// Lưu game vào slot cụ thể (1 đến 4)
    public bool SaveGameToSlot(int slotNumber, GameData gameData)
    {
        if (slotNumber < 1 || slotNumber > MaxSlots)
        {
            Debug.LogError("Invalid slot number: " + slotNumber);
            return false;
        }
        RefreshAllSlots();
        string fileName = SlotFilePrefix + slotNumber + ".json";
        fileDataHandler.SetFileName(fileName);

        SaveSlot saveSlot = new SaveSlot
        {
            slotNumber = slotNumber,
            LastModified = DateTime.Now,
            slotName = "Slot " + slotNumber,
            playerName = PlayerPrefs.GetString("PlayerName"),
            fileName = fileName,
            gameData = gameData
        };

        fileDataHandler.SaveData(saveSlot);
        PlayerPrefs.SetInt("CurrentSlot", slotNumber);
        Debug.Log($"Game saved to slot {slotNumber} at {fileName}");
        return true;
    }

    public bool SaveGameAfterCompletedLevel(List<ScoreRecord> records, int slotNumber, int level)
    {
        if (slotNumber < 1 || slotNumber > MaxSlots)
        {
            Debug.LogError("Invalid slot number: " + slotNumber);
            return false;
        }

        RefreshAllSlots();

        string fileName = SlotFilePrefix + slotNumber + ".json";
        fileDataHandler.SetFileName(fileName);

        SaveSlot saveSlot = new SaveSlot
        {
            slotNumber = slotNumber,
            scores = records,
            playerName = PlayerPrefs.GetString("PlayerName"),
            isLevelCompleted = true,
            LastModified = DateTime.Now,
            fileName = fileName
        };
        GameData gameData = new GameData()
        {
            level = level, // Cập nhật level hiện tại
        };
        saveSlot.gameData = gameData;
        fileDataHandler.SaveData(saveSlot);
        Debug.Log($"Game saved to slot {slotNumber} at {fileName}");
        PlayerPrefs.SetInt("CurrentSlot", slotNumber);
        // Cập nhật loadedSlot nếu cần
        SetLoadedSlot(saveSlot);
        return true;
    }


    public int GetEmptySlotNumber()
    {
        RefreshAllSlots();
        for (int i = 0; i < MaxSlots; i++)
        {
            if (slots[i] == null)
            {
                return i + 1; // Trả về số slot (1 đến 4)
            }
        }
        return -1; // Không có slot trống
    }
    public void RefreshAllSlots()
    {
        slots = new List<SaveSlot>(new SaveSlot[MaxSlots]);

        for (int i = 1; i <= MaxSlots; i++)
        {
            string fileName = SlotFilePrefix + i + ".json";
            fileDataHandler.SetFileName(fileName);
            SaveSlot loadedSlot = fileDataHandler.LoadData<SaveSlot>();

            if (loadedSlot != null)
            {
                slots[i - 1] = loadedSlot;
            }
        }
    }

    /// Lấy danh sách các slot đã lưu (dùng để hiển thị trong UI)
    public List<SaveSlot> GetAllSaveSlots()
    {
        RefreshAllSlots();
        return new List<SaveSlot>(slots);
    }

    /// Tải dữ liệu GameData trong slot được chọn
    public SaveSlot LoadDataInSlot(int slotNumber)
    {
        var slot = slots.Find(s => s != null && s.slotNumber == slotNumber);
        return slot;
    }

    /// Xoá slot lưu nếu cần (tuỳ chọn)
    public void DeleteSlot(int slotNumber)
    {
        string fileName = SlotFilePrefix + slotNumber + ".json";
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
            slots.RemoveAll(s => s.slotNumber == slotNumber);
            Debug.Log($"Deleted slot {slotNumber}");
        }
    }
    public SaveSlot GetLoadedSlot()
    {
        return loadedSlot;
    }
    public void SetLoadedSlot(SaveSlot slot)
    {
        loadedSlot = slot;
        if (slot != null)
        {
            // Cập nhật currentSlotNumber nếu slot hợp lệ
            currentSlotNumber = slot.slotNumber;
        } else
        {
            currentSlotNumber = -1; // Reset
        }
    }
    public void DestroyManagerInLevel()
    {
        Destroy(CameraManager.Instance.gameObject);
        Destroy(EnemyTargetProvider.Instance.gameObject);
        Destroy(UISingleton.Instance.gameObject);
        Destroy(PlayerManager.Instance.gameObject);
        Destroy(InventoryManager.Instance.gameObject);
        Destroy(MapStateManager.Instance.gameObject);
        Destroy(CoinRecorder.Instance.gameObject);
    }
}
