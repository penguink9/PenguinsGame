using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class CharacterSelectionUI : MonoBehaviour
{
    public TMP_InputField characterName;
    public Image fadeScreen;          // Kéo image vào đây trong Inspector
    public float fadeSpeed = 1.5f;   // Tốc độ fade, điều chỉnh nếu muốn

    public void SceneTransition(string sceneName)
    {
        StartCoroutine(SceneTransitionRoutine(sceneName));
    }

    IEnumerator SceneTransitionRoutine(string sceneName)
    {
        // Bắt đầu hiệu ứng fade out
        yield return StartCoroutine(FadeOutRoutine());
        // Sau khi fade xong mới chuyển scene
        SceneManager.LoadScene(sceneName);
    }
    IEnumerator FadeOutRoutine()
    {
        float alpha = 0f;
        fadeScreen.gameObject.SetActive(true); // Đảm bảo bật FadeScreen
        fadeScreen.color = new Color(0, 0, 0, 0);

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeScreen.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
            yield return null;
        }
        fadeScreen.color = new Color(0, 0, 0, 1);
    }
    public void SaveName()
    {
        string playerName = characterName.text;
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
        // Load map/gameplay scene ở đây (nếu muốn)
        // SceneManager.LoadScene("TênSceneMap");
        Debug.Log("Welcome " + characterName.text);
    }


}
