using UnityEngine;
using System.Collections.Generic;

public class EnemyGroupManager : MonoBehaviour
{
    [Tooltip("Số thứ tự của map (ví dụ: 1, 2, 3...)")]
    public int mapIndex;

    private List<EnemyHealth> enemies;

    private void Awake()
    {
        // Lấy toàn bộ EnemyHealth trong con
        enemies = new List<EnemyHealth>(GetComponentsInChildren<EnemyHealth>());        
    }

    private void Start()
    {
        // Đăng ký với MapStateManager
        MapStateManager.Instance.RegisterEnemyGroup(mapIndex, this);
        // Nếu có trạng thái map đã lưu → khôi phục lại trạng thái cho enemy
        if (MapStateManager.Instance.TryGetMapState(mapIndex, out var state))
        {
            Debug.Log($"Loading {state.enemies.Count} enemies in map {mapIndex}");
            foreach (var enemy in enemies)
            {
                var saved = state.enemies.Find(e => e.id == enemy.name);
                if (saved != null)
                    enemy.LoadState(saved);
                else Destroy(enemy.gameObject); // Xóa nếu đã bị phá trước đó
            }
        }
    }

    /// Lưu trạng thái toàn bộ enemy hiện tại
    public List<EnemyState> SaveEnemies()
    {
        List<EnemyState> list = new();
        foreach (var enemy in enemies)
            if (enemy != null) list.Add(enemy.SaveState());
        Debug.Log($"Saved {list.Count} enemies in map {mapIndex}");
        return list;
    }
}
