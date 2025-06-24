using UnityEngine;
using TMPro;
using System.Collections;

public class TextTypewriter : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    private string fullText;
    public float delay = 0.03f;
    public IEnumerator RevealText()
    {
        textMeshPro.text = "";
        foreach (char c in fullText)
        {
            textMeshPro.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
    public void SetText(string newText)
    {
        fullText = newText;
        textMeshPro.text = "";
        StartCoroutine(RevealText());
    }
}
