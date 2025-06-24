using UnityEngine;

public class ToggleSettingBtn : MonoBehaviour
{
    [SerializeField] GameObject settingPanel;
    public void Toggle()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(!settingPanel.activeSelf);
        }
        else
        {
            Debug.LogWarning("Setting panel is not assigned.");
        }
    }
}
