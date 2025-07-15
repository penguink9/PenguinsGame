using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Runtime.CompilerServices;

// Singleton trung tâm lưu trạng thái từng map (enemy + item).
[DefaultExecutionOrder(-2)]
public class MapStateManager : Singleton<MapStateManager>, ILoadGameInit
{
    [SerializeField] List<int> capturedPenguinIndexs = new();
    public Dictionary<int,bool> isCapturedPenguinUnlocked = new();
    // Trạng thái từng map
    private Dictionary<int, MapState> mapStates = new();

    // Quản lý các group theo map
    private Dictionary<int, EnemyGroupManager> enemyGroups = new();
    private Dictionary<int, ItemGroupManager> itemGroups = new();
    private bool entryExitArea = false;
    private bool firstTimeLoad = true;
    private bool isBossDefeated = false;
    public bool IsBossDefeated
    {
        get => isBossDefeated;
        set => isBossDefeated = value;
    }
    public bool FirstTimeLoad
    {
        get => firstTimeLoad;
        set => firstTimeLoad = value;
    }
    public bool EntryExitArea
    {
        get => entryExitArea;
        set => entryExitArea = value;
    }

    protected override void Awake()
    {
        base.Awake();
        if (capturedPenguinIndexs.Count > 0)
        {
            foreach (var index in capturedPenguinIndexs)
            {
                isCapturedPenguinUnlocked.Add(index, false);
            }
        }
    }
    private void Start()
    {
        LoadGameInit();
    }
    public void LoadGameInit()
    {
        if (!TrackCurrentMap.Instance.HasLoadData())
        {
            // Nếu chưa có dữ liệu đã lưu → khởi tạo trạng thái rỗng
            mapStates = new();
            enemyGroups = new();
            itemGroups = new();
            return;
        }

        // Lấy dữ liệu đã lưu từ DataManager
        var savedMapStates = DataManager.Instance.GetLoadedSlot().gameData.mapStates;
        var savedNpcUnlockeds = DataManager.Instance.GetLoadedSlot().gameData.npcUnlockeds;
        SetLoadData(savedMapStates, savedNpcUnlockeds);
    }
    // Đăng ký enemy group với mapIndex cụ thể
    public void RegisterEnemyGroup(int mapIndex, EnemyGroupManager group)
    {
        // Nếu đã có group cũ → thay thế
        if (enemyGroups.ContainsKey(mapIndex))
            enemyGroups[mapIndex] = group;
        else
            enemyGroups.Add(mapIndex, group);
    }

    // Đăng ký item group với mapIndex cụ thể
    public void RegisterItemGroup(int mapIndex, ItemGroupManager group)
    {
        if (itemGroups.ContainsKey(mapIndex))
            itemGroups[mapIndex] = group;
        else
            itemGroups.Add(mapIndex, group);
    }

    // Lấy trạng thái đã lưu của map (nếu có)
    public bool TryGetMapState(int mapIndex, out MapState state)
    {
        return mapStates.TryGetValue(mapIndex, out state);
    }

    // Lưu trạng thái map hiện tại (enemy + item)
    public void SaveMapState(int mapIndex)
    {
        var enemyGroup = enemyGroups.ContainsKey(mapIndex) ? enemyGroups[mapIndex] : null;
        var itemGroup = itemGroups.ContainsKey(mapIndex) ? itemGroups[mapIndex] : null;

        var enemyStates = enemyGroup != null ? enemyGroup.SaveEnemies() : new List<EnemyState>();
        var itemStates = itemGroup != null ? itemGroup.SaveItems() : new List<ItemState>();

        mapStates[mapIndex] = new MapState(enemyStates, itemStates);

        // Xoá tham chiếu group vì chuẩn bị rời map
        enemyGroups.Remove(mapIndex);
        itemGroups.Remove(mapIndex);
    }

    // Lưu toàn bộ map đã đăng ký
    public void SaveAll()
    {
        foreach (var key in enemyGroups.Keys)
            SaveMapState(key);
    }

    public List<MapStateEntry> GetAllMapStates()
    {
        return mapStates.Select(kvp => new MapStateEntry { mapIndex = kvp.Key, state = kvp.Value }).ToList();
    }

    //Mission: Release all captured penguin or defeat boss
    public bool IsMissionCompleted()
    {
        if (isCapturedPenguinUnlocked.Count == 0) 
        {
            return isBossDefeated;
        }
        foreach (var index in isCapturedPenguinUnlocked)
        {
            if (!index.Value) return false;
        }
        return true;
    }

    public List<BoolEntry> GetCapturedPenguinState()
    {
        return isCapturedPenguinUnlocked.Select(kvp => new BoolEntry { key = kvp.Key, value = kvp.Value })
    .ToList();
    }

    public void SetLoadData(List<MapStateEntry> savemapStates, List<BoolEntry> npcUnlockeds)
    {
        // Reset lại dictionary mapStates
        mapStates = new Dictionary<int, MapState>();

        if (savemapStates != null)
        {
            foreach (var entry in savemapStates)
            {
                mapStates[entry.mapIndex] = entry.state;
            }
        }

        // Reset lại trạng thái các NPC
        isCapturedPenguinUnlocked.Clear();

        if (npcUnlockeds != null)
        {
            foreach (var entry in npcUnlockeds)
            {
                isCapturedPenguinUnlocked[entry.key] = entry.value;
            }
        }
    }
}
