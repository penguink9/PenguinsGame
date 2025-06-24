using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPanelController : MonoBehaviour
{
    [SerializeField] private Scrollbar musicScrollbar;
    [SerializeField] private Scrollbar sfxScrollbar;
    private GameObject audioContent;
    private void Start()
    {
        // Initialize scrollbars with current volume levels
        musicScrollbar.value = AudioManager.Instance.GetMusicVolume();
        sfxScrollbar.value = AudioManager.Instance.GetSfxVolume();
        audioContent = GameObject.Find("AudioContent");
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
        audioContent.SetActive(true);
    }
    public void ExitToMainMenu()
    {
        Destroy(CameraManager.Instance.gameObject);
        Destroy(EnemyTargetProvider.Instance.gameObject);
        Destroy(AudioManager.Instance.gameObject);
        Destroy(UISingleton.Instance.gameObject);
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
}
