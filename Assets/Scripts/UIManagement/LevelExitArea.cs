using TMPro;
using UnityEngine;

public class LevelExitArea : MonoBehaviour
{
    private TextMeshProUGUI text;
    private bool isTextShown = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!MapStateManager.Instance.IsMissionCompleted())
            {
                if(!isTextShown)
                {
                    text = UISingleton.Instance.ShowMessageStay(transform, "Complete your mission before leaving");
                    isTextShown = true;
                }                
                return;
            }            
            // Nếu người chơi đã hoàn thành nhiệm vụ, hiển thị popup hoàn thành cấp độ
            MapStateManager.Instance.EntryExitArea = true;
            UISingleton.Instance.ShowLevelCompletedPopup();
            PlayerManager.Instance.gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isTextShown)
            {
                UISingleton.Instance.HideMessageStay(text);
                isTextShown = false;
            }
        }
    }
}
