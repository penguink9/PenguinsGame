using UnityEngine;
using System.Collections.Generic;

/// Quản lý toàn bộ Item phá hủy được trong 1 map. Tự động load trạng thái trong Start.
public class ItemGroupManager : MonoBehaviour
{
    [Tooltip("Số thứ tự của map (ví dụ: 1, 2, 3...)")]
    public int mapIndex;

    private List<Destructible> items;

    private void Awake()
    {
        // Lấy toàn bộ Destructible trong con
        items = new List<Destructible>(GetComponentsInChildren<Destructible>());

        // Đăng ký với MapStateManager
        MapStateManager.Instance.RegisterItemGroup(mapIndex, this);
    }

    private void Start()
    {
        // Nếu có trạng thái map đã lưu → khôi phục lại trạng thái cho item
        if (MapStateManager.Instance.TryGetMapState(mapIndex, out var state))
        {
            Debug.Log($"Loading {state.items.Count} items in map {mapIndex}");
            foreach (var item in items)
            {
                var saved = state.items.Find(i => i.id == item.name);
                if (saved == null)
                    Destroy(item.gameObject); // Xóa nếu đã bị phá trước đó
            }
        }
    }

    /// Lưu trạng thái toàn bộ item hiện tại
    public List<ItemState> SaveItems()
    {
        List<ItemState> list = new();
        foreach (var item in items)
            if (item != null) list.Add(item.SaveState());
        Debug.Log($"Saved {list.Count} items in map {mapIndex}");
        return list;
    }
}
