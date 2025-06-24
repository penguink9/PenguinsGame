using UnityEngine;
using TMPro;
using System.Collections;

public class TextTypewriter : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    [TextArea] public string fullText;
    public float delay = 0.03f;

    void OnEnable()
    {
        StartCoroutine(RevealText());
    }

    IEnumerator RevealText()
    {
        textMeshPro.text = "";
        foreach (char c in fullText)
        {
            textMeshPro.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}
