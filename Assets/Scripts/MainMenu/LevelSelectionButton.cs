using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
    public int mapIndex;
    public int levelIndex;
    public Image lockImg;    // Kéo Image khoá vào đây
    public Image tickImg;    // Kéo Image tick vào đây (nếu có)
    public Button btn;       // Kéo Button vào đây (nếu chưa tự động)

    private void Reset()
    {
        btn = GetComponent<Button>();
        if (!lockImg) lockImg = transform.Find("LockImg")?.GetComponent<Image>();
        if (!tickImg) tickImg = transform.Find("TickImg")?.GetComponent<Image>();
    }

    /// <summary>
    /// Gọi từ UIManager.UpdateUI, cập nhật giao diện của nút level này.
    /// </summary>
    public void SetState(bool unlocked, bool completed)
    {
        if (btn) btn.interactable = unlocked; // Giữ nút có thể tương tác khi unlocked = true
        if (lockImg) lockImg.enabled = !unlocked; // Hiện khóa nếu chưa mở khóa
        if (tickImg) tickImg.enabled = completed; // Hiện tick nếu hoàn thành

        // Nếu level bị khóa, thêm logic hiển thị thông báo
        if (!unlocked)
        {
            btn.onClick.RemoveAllListeners(); // Hủy sự kiện click
            btn.onClick.AddListener(() => ShowLockedMessage()); // Thêm sự kiện hiển thị thông báo
        }
        else
        {
            btn.onClick.RemoveAllListeners(); // Hủy sự kiện click cũ
            btn.onClick.AddListener(() => OnClick()); // Gọi hàm OnClick nếu level đã mở khóa
        }
    }

    private void ShowLockedMessage()
    {
        // Logic để hiển thị thông báo khi level bị khóa
        Debug.Log("This level is locked. Please unlock it first!");
        // Bạn có thể gọi thêm một hàm để mở popup hoặc UI thông báo nếu cần
    }

    // Gán sự kiện này vào Button.OnClick trong Editor
    public void OnClick()
    {
       
    }
}
