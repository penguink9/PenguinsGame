using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MapSelection : MonoBehaviour
{
    public bool isUnlock = false;         // Trạng thái mở khoá (set bởi UIManager)
    public GameObject lockedGo;           // Panel/Icon khóa
    public GameObject unlockedGo;         // Panel/Icon đã mở khoá
    public int mapIndex;                  // Thứ tự map (0,1,2,3...)

    [Header("Hiệu ứng chuyển cảnh (nếu cần)")]
    public Image fadeScreen;
    public float fadeSpeed = 1.5f;

    private void Update()
    {
        UpdateMapStatus();
    }

    private void UpdateMapStatus()
    {
        if (unlockedGo != null) unlockedGo.SetActive(isUnlock);
        if (lockedGo != null) lockedGo.SetActive(!isUnlock);
    }

    // Gọi khi muốn chuyển scene có hiệu ứng fade (nếu cần)
    public void SceneTransition(string sceneName)
    {
        StartCoroutine(SceneTransitionRoutine(sceneName));
    }

    IEnumerator SceneTransitionRoutine(string sceneName)
    {
        yield return StartCoroutine(FadeOutRoutine());
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeOutRoutine()
    {
        float alpha = 0f;
        if (fadeScreen != null)
        {
            fadeScreen.gameObject.SetActive(true);
            fadeScreen.color = new Color(0, 0, 0, 0);

            while (alpha < 1f)
            {
                alpha += Time.deltaTime * fadeSpeed;
                fadeScreen.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return null;
            }
            fadeScreen.color = new Color(0, 0, 0, 1);
        }
    }
}
