using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
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
        PlayerPrefs.SetInt("CurrentSlot", 0);
        PlayerPrefs.Save();
    }


}
