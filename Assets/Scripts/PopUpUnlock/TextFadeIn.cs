using UnityEngine;
using TMPro;
using System.Collections;

public class TextFadeIn : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float duration = 1f; // Thời gian fade in

    void OnEnable()
    {
        StartCoroutine(FadeInText());
    }

    IEnumerator FadeInText()
    {
        textMeshPro.alpha = 0;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            textMeshPro.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
        textMeshPro.alpha = 1; // Đảm bảo alpha = 1 sau khi chạy xong
    }
}
