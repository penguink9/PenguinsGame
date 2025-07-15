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
        if(TrackCurrentMap.Instance.level != 3)
        {
            AudioManager.Instance.PlayMusic("Background-1");
        } else
        {
            AudioManager.Instance.PlayMusic("Background-2");
        }
        if (!TrackCurrentMap.Instance.HasLoadData())
        {
            if(MapStateManager.Instance.FirstTimeLoad && TrackCurrentMap.Instance.map == 1)
            {
                UISingleton.Instance.ShowMissionPanel();
                MapStateManager.Instance.FirstTimeLoad = false;
            }
            return;
        }
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
            UISingleton.Instance.ShowMissionPanel();
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
