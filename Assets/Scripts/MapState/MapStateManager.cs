using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// Singleton trung tâm lưu trạng thái từng map (enemy + item).
// Cũng đăng ký và điều phối các group theo mapIndex.
public class MapStateManager : Singleton<MapStateManager>
{
    [SerializeField] List<int> capturedPenguinIndexs = new();
    public Dictionary<int,bool> isCapturedPenguinUnlocked = new();
    // Trạng thái từng map
    private Dictionary<int, MapState> mapStates = new();

    // Quản lý các group theo map
    private Dictionary<int, EnemyGroupManager> enemyGroups = new();
    private Dictionary<int, ItemGroupManager> itemGroups = new();

    protected override void Awake()
    {
        base.Awake();
        foreach (var index in capturedPenguinIndexs)
        {
            isCapturedPenguinUnlocked.Add(index, false);
        }
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

    public int GetMapIndexFromSceneName()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Tách bằng dấu gạch dưới
        string[] parts = sceneName.Split('_');

        if (parts.Length >= 2 && parts[1].StartsWith("Map"))
        {
            string mapPart = parts[1].Substring(3); // bỏ "Map" → còn lại là số
            if (int.TryParse(mapPart, out int mapIndex))
                return mapIndex;
        }

        Debug.LogWarning("Không thể lấy mapIndex từ scene name: " + sceneName);
        return -1; // hoặc throw exception tùy mục đích
    }
}
