using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class VictoryController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject popupPanel;             // Root popup (ẩn lúc start)
    public RectTransform contentPanel;        // Panel con chứa text & buttons
    public TextMeshProUGUI titleText;         // "VICTORY!"
    public TextMeshProUGUI subtitleText;      // "Level X Completed"
    public TextMeshProUGUI scoreText;         // "Score: Y"
    public Button nextLevelButton;            // Next Level
    public Button mainMenuButton;             // Main Menu

    [Header("Typewriter & Effects")]
    public float charDelay = 0.05f;
    public float popScale = 1.3f;
    public float popDuration = 0.4f;
    public float glowAmount = 1.5f;
    public float glowDuration = 0.6f;

    [Header("Level Scene Names")]
    public string[] levelScenes = new[]
    {
        "Level1_Map1",
        "Level1_Map2",
        "Level1_Map3",
        // … thêm tên các scene ở đây…
    };

    private Material titleMat;

    void Awake()
    {
        // Clone material để animate glow
        titleMat = Instantiate(titleText.fontMaterial);
        titleText.fontMaterial = titleMat;

        // Ẩn popup & content, tắt nút
        popupPanel.SetActive(false);
        contentPanel.localScale = Vector3.zero;
        nextLevelButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);

        // Gán sự kiện cho nút
        nextLevelButton.onClick.AddListener(OnNextLevel);
        mainMenuButton.onClick.AddListener(OnMainMenu);
    }

    /// <summary>
    /// Gọi khi level kết thúc: bật popup, chạy hiệu ứng và hiển thị nút.
    /// </summary>
    public void ShowVictory(int levelIndex, int score)
    {
        // 1. Bật popup
        popupPanel.SetActive(true);

        // 2. Scale content panel lên
        contentPanel.localScale = Vector3.zero;
        contentPanel.DOScale(1f, 0.5f).SetEase(Ease.OutBack);

        // 3. Xóa text cũ và tắt nút
        titleText.text = "";
        subtitleText.text = "";
        scoreText.text = "";
        nextLevelButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);

        // 4. Bắt đầu hiệu ứng typewriter
        StartCoroutine(PlaySequence(levelIndex, score));
    }

    private IEnumerator PlaySequence(int levelIndex, int score)
    {
        // delay nhỏ để panel kịp scale lên
        yield return new WaitForSeconds(0.25f);

        // Title
        foreach (char c in "VICTORY!")
        {
            titleText.text += c;
            yield return new WaitForSeconds(charDelay);
        }
        Pop(titleText.rectTransform);
        Glow();

        // Subtitle
        yield return new WaitForSeconds(0.1f);
        string sub = $"Level {levelIndex} Completed";
        foreach (char c in sub)
        {
            subtitleText.text += c;
            yield return new WaitForSeconds(charDelay);
        }
        Pop(subtitleText.rectTransform);

        // Score
        yield return new WaitForSeconds(0.1f);
        string sc = $"Score: {score}";
        foreach (char c in sc)
        {
            scoreText.text += c;
            yield return new WaitForSeconds(charDelay);
        }
        Pop(scoreText.rectTransform);

        // Hiển thị nút
        yield return new WaitForSeconds(0.2f);
        nextLevelButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
    }

    private void Pop(RectTransform rt)
    {
        rt.localScale = Vector3.zero;
        rt.DOScale(popScale, popDuration * 0.7f).SetEase(Ease.OutBack)
          .OnComplete(() => rt.DOScale(1f, popDuration * 0.3f).SetEase(Ease.OutSine));
    }

    private void Glow()
    {
        titleMat.SetFloat(ShaderUtilities.ID_GlowPower, 0f);
        DOTween.To(() => titleMat.GetFloat(ShaderUtilities.ID_GlowPower),
                   x => titleMat.SetFloat(ShaderUtilities.ID_GlowPower, x),
                   glowAmount, glowDuration)
               .SetLoops(2, LoopType.Yoyo)
               .SetEase(Ease.InOutSine);
    }

    private void OnNextLevel()
    {
        string cur = SceneManager.GetActiveScene().name;
        int idx = Array.IndexOf(levelScenes, cur);
        if (idx >= 0 && idx < levelScenes.Length - 1)
            SceneManager.LoadScene(levelScenes[idx + 1]);
        else
            SceneManager.LoadScene("MainMenu");
    }

    private void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // --- Dành cho test nhanh: bấm Q để show popup ---
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ShowVictory(1, 999);
    }
}
