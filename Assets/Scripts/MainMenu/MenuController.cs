using UnityEngine;
using UnityEngine.UI;   
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    [Header("Levels To Load")]
    public string newGameLevel;

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(newGameLevel);
    }
    public void LoadGameDialogYes()
    {
       SceneManager.LoadScene("LoadGameScene");
    }
    public void ExitButton() { 
        Application.Quit();
    }
}
