using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MapSelection : MonoBehaviour
{
    public bool isUnlock = false;
    public GameObject lockedGo;
    public GameObject unlockedGo;

    public int mapIndex;
    public int keyRq;
    public int startLevel;
    public int endLevel;



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

    private void Update()
    {
        UpdateMapStatus();
        unLockMap(); 
    }
    private void UpdateMapStatus()
    {
        if (isUnlock)
        {
            unlockedGo.gameObject.SetActive(true);
            lockedGo.gameObject.SetActive(false);
        }
        else
        {
            unlockedGo.gameObject.SetActive(false);
            lockedGo.gameObject.SetActive(true);
        }
    }
    private void unLockMap()
    {
        if (UIManager.instance.key >= keyRq)
        {
            isUnlock = true;
        }
        else
        {
            isUnlock = false;
        }
    }
    
}
