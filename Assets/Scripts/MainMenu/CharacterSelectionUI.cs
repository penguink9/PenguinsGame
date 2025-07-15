using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class CharacterSelectionUI : MonoBehaviour
{
    public TMP_InputField characterName;

    public void SceneTransition(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void SaveName()
    {
        string playerName = characterName.text;
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
        Debug.Log("Welcome " + characterName.text);
    }


}
