using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerPrefabsDatabase prefabDatabase;
    [SerializeField] private Transform charactersUIParent;
    [SerializeField] private Slider healthSlider;
    [SerializeField] List<int> beginningCharacterIndexs;
    private List<Slider> characterHPSliders = new List<Slider>();

    private List<GameObject> unlockedPlayers = new List<GameObject>();
    private List<bool> aliveCharacterList = new List<bool>();
    private int activePlayerIndex = 0;


   
    private void Start()
    {
        CacheCharacterHPSliders();
        if (beginningCharacterIndexs.Count != 0) 
        {
            foreach (int index in beginningCharacterIndexs)
            {
                UnlockCharacter(index);
            }
        }
        SwitchCharacter(0);
    }

    public bool UnlockCharacter(int prefabIndex)
    {
        if (prefabIndex < 0 || prefabIndex >= prefabDatabase.playerPrefabs.Length) return false;

        GameObject newPlayer = Instantiate(prefabDatabase.playerPrefabs[prefabIndex], transform);
        var health = newPlayer.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.OnPlayerDeath += () => OnPlayerDeath(prefabIndex);
            health.SetHealthBar(characterHPSliders[prefabIndex]);
        }
        newPlayer.SetActive(false);
        unlockedPlayers.Add(newPlayer);
        aliveCharacterList.Add(true); // Assume the character is alive when unlocked
        List<int> unlockedPlayersIndext = new List<int>();
        for (int i = 0; i < unlockedPlayers.Count; i++)
        {
            if (unlockedPlayers[i])
            {
                unlockedPlayersIndext.Add(i);
            }
        }
        //charactersUIParent.GetChild(prefabIndex).gameObject.SetActive(true);
        charactersUIParent.GetComponent<ChangeCharacter>().UpdateCharBox(unlockedPlayers.Count);
        return true;
    }
    private void OnPlayerDeath(int playerIndex)
    {
        aliveCharacterList[playerIndex] = false;

        // Cập nhật UI Death
        var charUI = charactersUIParent.GetChild(playerIndex);
        charUI.Find("CharBox/Death").gameObject.SetActive(true);
        charUI.Find("CharBox/CharAvt").gameObject.SetActive(false);
        // Tự động tìm nhân vật sống và switch
        bool switched = false;
        for (int i = 0; i < aliveCharacterList.Count; i++)
        {
            if (aliveCharacterList[i])
            {
                charactersUIParent.GetComponent<ChangeCharacter>().TriggerSlot(i + 1);
                switched = true;
                break;
            }
        }

        // Nếu không còn nhân vật nào sống -> GameOver
        if (!switched)
        {
            TriggerGameOver();
        }

    }

    private void TriggerGameOver()
    {
        Debug.Log("GAME OVER");
        AudioManager.Instance.PlaySFX("Game Over");
        Destroy(CameraManager.Instance.gameObject);
        Destroy(EnemyTargetProvider.Instance.gameObject);
        Destroy(UISingleton.Instance.gameObject);
        Destroy(gameObject);
        SceneManager.LoadScene("Level1_Map1");
    }

    public bool SwitchCharacter(int index)
    {
        Vector3 currentPosition = unlockedPlayers[activePlayerIndex].transform.position;
        if (index < 0 || index >= unlockedPlayers.Count) return false;
        //Set old player's health bar to the UI
        if (!unlockedPlayers[activePlayerIndex].GetComponent<PlayerState>().CanSwitch()) return false;
        unlockedPlayers[activePlayerIndex].GetComponent<PlayerHealth>().SetHealthBar(characterHPSliders[activePlayerIndex]);

        // Deactivate all players and activate the selected one
        unlockedPlayers[activePlayerIndex].SetActive(false);
        // Activate the new player and set its position
        activePlayerIndex = index;
        unlockedPlayers[activePlayerIndex].SetActive(true);
        unlockedPlayers[activePlayerIndex].transform.position = currentPosition;
        // Update the camera to follow the new player
        CameraManager.Instance.SetFollow(unlockedPlayers[activePlayerIndex].transform);
        // Set the health bar for the active player
        unlockedPlayers[activePlayerIndex].GetComponent<PlayerHealth>().SetHealthBar(healthSlider);
        // Update the enemy target provider to follow the new player
        EnemyTargetProvider.Instance.SetTarget(unlockedPlayers[activePlayerIndex].transform);
        return true;
    }

    public GameObject GetActivePlayer()
    {
        return unlockedPlayers[activePlayerIndex];
    }
    public int GetActivePlayerIndex()
    {
        return activePlayerIndex;
    }  
    public bool IsCharacterUnlocked(int index) => index < unlockedPlayers.Count;
    public int UnlockedCharacterCount() => unlockedPlayers.Count;

    private void CacheCharacterHPSliders()
    {
        characterHPSliders.Clear();

        foreach (Transform character in charactersUIParent)
        {
            Slider hpSlider = character.GetChild(1).GetComponent<Slider>();
            characterHPSliders.Add(hpSlider);
        }
    }
    public bool IsCharacterAlive(int index)
    {
        return index >= 0 && index < aliveCharacterList.Count && aliveCharacterList[index];
    }
}
