using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPanelController : MonoBehaviour
{
    [SerializeField] private Scrollbar musicScrollbar;
    [SerializeField] private Scrollbar sfxScrollbar;
    private Transform audioContent;
    private void Start()
    {
        // Initialize scrollbars with current volume levels
        musicScrollbar.value = AudioManager.Instance.GetMusicVolume();
        sfxScrollbar.value = AudioManager.Instance.GetSfxVolume();
        audioContent = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1);
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
        audioContent.gameObject.SetActive(true);
    }
    public void ExitToMainMenu()
    {
        Destroy(CameraManager.Instance.gameObject);
        Destroy(EnemyTargetProvider.Instance.gameObject);
        Destroy(AudioManager.Instance.gameObject);
        Destroy(UISingleton.Instance.gameObject);
        Destroy(PlayerManager.Instance.gameObject);
        Destroy(InventoryManager.Instance.gameObject);
        Destroy(MapStateManager.Instance.gameObject);
        Destroy(CoinRecorder.Instance.gameObject);
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
