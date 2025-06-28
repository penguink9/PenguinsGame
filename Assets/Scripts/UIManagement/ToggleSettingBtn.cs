using UnityEngine;

public class ToggleSettingBtn : MonoBehaviour
{
    [SerializeField] GameObject settingPanel;

    private void Start()
    {
        UIInputManager.Instance.OnSettingOpen += Toggle;
    }
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
