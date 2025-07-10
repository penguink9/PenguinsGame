using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class  GameOverPopup : MonoBehaviour
{
    public static GameOverPopup instance;
    public GameObject gameOverPanel;       // Panel Game Over
    public Animator gameOverAnimator;      // Animator của panel

    public TMP_Text goldText;
    public TMP_Text levelText;
    public Button tryAgainButton;
    public Button backToMainMenuButton;

    // Giá trị này được set từ bên ngoài khi kết thúc map
    public int final_gold;

    // Tên map hiện tại, ví dụ "Level1_Map1"
    public string currentMapName;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        tryAgainButton.onClick.AddListener(OnTryAgainClicked);
        backToMainMenuButton.onClick.AddListener(OnBackToMainMenuClicked);
    }

    public void ShowGameOverPopUp(int gold, string mapName)
    {
        final_gold = gold;
        currentMapName = mapName;

        goldText.text = final_gold.ToString();

        string level = ExtractLevelFromMapName(currentMapName);
        levelText.text = level;

        // Reset scale và alpha trước khi bật animation
        gameOverPanel.transform.localScale = Vector3.zero;
        gameOverPanel.SetActive(true);


        gameOverAnimator.ResetTrigger("ShowPopUp");
        gameOverAnimator.SetTrigger("ShowPopUp");
    }

    private string ExtractLevelFromMapName(string mapName)
    {
        if (string.IsNullOrEmpty(mapName))
            return "";

        int underscoreIndex = mapName.IndexOf('_');
        if (underscoreIndex > 0)
            return mapName.Substring(0, underscoreIndex);
        else
            return mapName;
    }

    private void OnTryAgainClicked()
    {
        SceneManager.LoadScene(currentMapName);
    }

    private void OnBackToMainMenuClicked()
    {
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
