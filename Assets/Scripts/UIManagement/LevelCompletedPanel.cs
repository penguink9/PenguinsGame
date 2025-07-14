using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompletedPanel : MonoBehaviour
{
    public GameObject levelCompletedPanel;       // Panel Game Over
    public Animator levelCompletedAnimator;      // Animator của panel

    public TMP_Text goldText;
    public TMP_Text levelText;
    public Button backToMainMenuButton;
    private void Awake()
    {
    }
    private void Start()
    {
        backToMainMenuButton.onClick.AddListener(OnBackToMainMenuClicked);
    }

    private void OnEnable()
    {
        ShowLevelCompletedPanel();
    }

    public void ShowLevelCompletedPanel()
    {

        goldText.text = CoinRecorder.Instance.TotalCoins.ToString();

        string level = TrackCurrentMap.Instance.level.ToString();
        levelText.text = level;

        // Reset scale và alpha trước khi bật animation
        levelCompletedPanel.transform.localScale = Vector3.zero;
        levelCompletedPanel.SetActive(true);
        levelCompletedAnimator.ResetTrigger("ShowPopUp");
        levelCompletedAnimator.SetTrigger("ShowPopUp");
    }

    private void OnBackToMainMenuClicked()
    {
        DataManager.Instance.DestroyManagerInLevel();
        SceneManager.LoadScene("MainMenu");
    }

    // Nếu muốn hàm ẩn popup có animation
    public void HideGameOverPopUp()
    {
        if (levelCompletedAnimator != null)
        {
            levelCompletedAnimator.ResetTrigger("ShowPopUp");
            levelCompletedAnimator.SetTrigger("HidePopUp");
        }
        else
        {
            levelCompletedPanel.SetActive(false);
        }
    }
}
