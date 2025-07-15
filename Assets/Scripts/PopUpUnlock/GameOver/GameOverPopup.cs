using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class  GameOverPopup : MonoBehaviour
{
    public GameObject gameOverPanel;       // Panel Game Over
    public Animator gameOverAnimator;      // Animator của panel

    public TMP_Text goldText;
    public TMP_Text levelText;
    public Button tryAgainButton;
    public Button backToMainMenuButton;
    private void Start()
    {
        tryAgainButton.onClick.AddListener(OnTryAgainClicked);
        backToMainMenuButton.onClick.AddListener(OnBackToMainMenuClicked);
    }

    private void OnEnable()
    {
        ShowGameOverPopUp();
    }

    public void ShowGameOverPopUp()
    {

        goldText.text = CoinRecorder.Instance.TotalCoins.ToString();

        string level = TrackCurrentMap.Instance.level.ToString();
        levelText.text = level;

        // Reset scale và alpha trước khi bật animation
        gameOverPanel.transform.localScale = Vector3.zero;
        gameOverPanel.SetActive(true);


        gameOverAnimator.ResetTrigger("ShowPopUp");
        gameOverAnimator.SetTrigger("ShowPopUp");
    }
    private void OnTryAgainClicked()
    {
        string currentMapName = "Level"+TrackCurrentMap.Instance.level+"_Map1";
        DataManager.Instance.DestroyManagerInLevel();
        SceneManager.LoadScene(currentMapName);
    }

    private void OnBackToMainMenuClicked()
    {
        DataManager.Instance.DestroyManagerInLevel();
        SceneManager.LoadScene("MainMenu");
    }

    // Nếu muốn hàm ẩn popup có animation
    public void HideGameOverPopUp()
    {
        if (gameOverAnimator != null)
        {
            gameOverAnimator.ResetTrigger("ShowPopUp");
            gameOverAnimator.SetTrigger("HidePopUp");
        }
        else
        {
            gameOverPanel.SetActive(false);
        }
    }
}
