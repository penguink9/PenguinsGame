using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPanelController : MonoBehaviour
{
    [SerializeField] private Scrollbar musicScrollbar;
    [SerializeField] private Scrollbar sfxScrollbar;
    private Transform audioContent;
    private Transform guideContent;
    private void Awake()
    {
        // Initialize scrollbars with current volume levels
        musicScrollbar.value = AudioManager.Instance.GetMusicVolume();
        sfxScrollbar.value = AudioManager.Instance.GetSfxVolume();
        audioContent = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0);
        guideContent = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1);
    }
    private void OnEnable()
    {
        OnClickAudioButton();
    }
    public void ToggleMusicButton()
    {
        AudioManager.Instance.ToggleMusic();
    }
    public void ToggleSfxButton()
    {
        AudioManager.Instance.ToggleSfx();
    }
    public void OnClickAudioButton()
    {
        guideContent.gameObject.SetActive(false);
        audioContent.gameObject.SetActive(true);
    }
    public void OnClickGuideButton()
    {
        audioContent.gameObject.SetActive(false);
        guideContent.gameObject.SetActive(true);
    }
    public void ExitToMainMenu()
    {
        DataManager.Instance.DestroyManagerInLevel();
        SceneManager.LoadScene("MainMenu");
    }

    public void OnMusicVolumeChange()
    {
        AudioManager.Instance.SetMusicVolume(musicScrollbar.value);
    }
    public void OnSfxVolumeChange()
    {
        AudioManager.Instance.SetSfxVolume(sfxScrollbar.value);
    }
    public void OnClickSaveGameButton(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
}
