using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject mapSelectionPanel;
    public GameObject[] levelSelectionPanel;

    public int key;
    public TextMeshProUGUI keyText;
    public MapSelection[] mapSelections;
 
    public TextMeshProUGUI[] questKeyTexts;
    public TextMeshProUGUI[] lockedKeyTexts;
    public TextMeshProUGUI[] unlockedKeyTexts;



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
           if(instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        
    }

    private void Update()
    {
        UpdateKeyUI();
        updateUnlockKeyUI();
        UpdateLockKeyUI();
    }

    private void UpdateLockKeyUI()
    {
        for (int i = 0; i < mapSelections.Length; i++)
        {
            questKeyTexts[i].text = mapSelections[i].keyRq.ToString();
            if (mapSelections[i].isUnlock == false) 
            {
                lockedKeyTexts[i].text = key.ToString() + "/" + mapSelections[i].endLevel;
            }
        }
    }

    private void updateUnlockKeyUI()
    {
        for (int i = 0; i < mapSelections.Length; i++)
        {
            unlockedKeyTexts[i].text = key.ToString() + "/" + mapSelections[i].endLevel;
        }
    }
    
    private void UpdateKeyUI()
    {
        keyText.text = key.ToString();
    }
    public void PressMapButton(int mapIndex)
    {
        if (mapSelections[mapIndex].isUnlock == true)
        {
            levelSelectionPanel[mapIndex].gameObject.SetActive(true);
            mapSelectionPanel.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Map is locked, please unlock it first.");
        }
    }
    public void BackMapButton()
    {
        mapSelectionPanel.gameObject.SetActive(true);
        for (int i = 0; i < mapSelections.Length; i++)
        {
            levelSelectionPanel[i].gameObject.SetActive(false);
        }
    }
    public void ScenceTransition(string ScenceName)
    {
        SceneManager.LoadScene(ScenceName);
    }
    }
