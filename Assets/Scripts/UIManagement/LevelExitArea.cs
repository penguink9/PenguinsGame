using UnityEngine;

public class LevelExitArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!MapStateManager.Instance.IsMissionCompleted()) return;
            // Nếu người chơi đã hoàn thành nhiệm vụ, hiển thị popup hoàn thành cấp độ
            MapStateManager.Instance.EntryExitArea = true;
            UISingleton.Instance.ShowLevelCompletedPopup();
            PlayerManager.Instance.gameObject.SetActive(false);
        }
    }
}
