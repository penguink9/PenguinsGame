using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class LoadGameInManagerMap : MonoBehaviour, ILoadGameInit
{
    private void Start()
    {
        LoadGameInit();
    }
    public void LoadGameInit()
    {
        // Check if the current scene is the one we want to load the game in
        if (!TrackCurrentMap.Instance.HasLoadData()) return;
        AudioManager.Instance.PlayMusic("Background-1");
        UIFade.Instance.FadeBlack();

        // Load the game data for the specified level        
        TrackCurrentMap.Instance.UpdateLevelText();
        GameData gameData = DataManager.Instance.GetLoadedSlot().gameData;
        if (gameData.currentMap == TrackCurrentMap.Instance.map)
        {
            PlayerManager.Instance.GetActivePlayer().transform.position = gameData.playerPosition;
            GameObject confiner = GameObject.FindGameObjectWithTag("Confiner");
            CameraManager.Instance.SetConfiner(confiner.GetComponent<PolygonCollider2D>());
            DataManager.Instance.SetLoadedSlot(null);
            UIFade.Instance.FadeToClear();
        }
        else
        {
            string loadScene = "Level" + gameData.level + "_Map" + gameData.currentMap;            
            StartCoroutine(LoadSceneCourotine(loadScene));
        }
    }
    public IEnumerator LoadSceneCourotine(string loadScene)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(loadScene);
    }
}
