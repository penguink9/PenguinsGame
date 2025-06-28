using System.Collections.Generic;
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
    private Dictionary<int, GameObject> unlockedPlayers = new Dictionary<int, GameObject>();
    private Dictionary<int, bool> aliveCharacterMap = new Dictionary<int, bool>();
    private List<Slider> characterHPSliders = new List<Slider>();
    private int activePlayerIndex = -1;

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

        if (unlockedPlayers.Count > 0)
        {
            SwitchCharacter(beginningCharacterIndexs[0]);
        }
    }

    public bool UnlockCharacter(int prefabIndex)
    {
        if (prefabIndex < 0 || prefabIndex >= prefabDatabase.playerPrefabs.Length)
            return false;

        if (unlockedPlayers.ContainsKey(prefabIndex))
            return false; // already unlocked

        GameObject newPlayer = Instantiate(prefabDatabase.playerPrefabs[prefabIndex], transform);
        var health = newPlayer.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.OnPlayerDeath += () => OnPlayerDeath(prefabIndex);
            health.SetHealthBar(characterHPSliders[prefabIndex]);
        }

        newPlayer.SetActive(false);
        unlockedPlayers[prefabIndex] = newPlayer;
        aliveCharacterMap[prefabIndex] = true;

        // Update UI
        charactersUIParent.GetComponent<ChangeCharacter>().UpdateCharBox(new List<int>(unlockedPlayers.Keys));
        return true;
    }

    private void OnPlayerDeath(int playerIndex)
    {
        if (!aliveCharacterMap.ContainsKey(playerIndex)) return;

        aliveCharacterMap[playerIndex] = false;

        // UI: Death icon on
        var changeChar = charactersUIParent.GetComponent<ChangeCharacter>();
        var charUI = charactersUIParent.GetChild(playerIndex);

        if (charUI != null)
        {
            charUI.Find("CharBox/Death").gameObject.SetActive(true);
            charUI.Find("CharBox/CharAvt").gameObject.SetActive(false);
        }

        // Try switch to another alive character
        bool switched = false;
        foreach (var kvp in aliveCharacterMap)
        {
            if (kvp.Value)
            {
                changeChar.TriggerSlot(changeChar.GetSlotIndexByPrefabIndex(kvp.Key) + 1);
                switched = true;
                break;
            }
        }

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
        if (!unlockedPlayers.ContainsKey(index)) return false;
        if (!aliveCharacterMap.ContainsKey(index) || !aliveCharacterMap[index]) return false;

        GameObject newPlayer = unlockedPlayers[index];

        if (activePlayerIndex != -1 && unlockedPlayers.ContainsKey(activePlayerIndex))
        {
            var current = unlockedPlayers[activePlayerIndex];
            if (!current.GetComponent<PlayerState>().CanSwitch()) return false;
            current.GetComponent<PlayerHealth>().SetHealthBar(characterHPSliders[activePlayerIndex]);
            current.SetActive(false);
        }

        Vector3 spawnPos = (activePlayerIndex != -1 && unlockedPlayers.ContainsKey(activePlayerIndex)) ?
            unlockedPlayers[activePlayerIndex].transform.position : transform.position;

        activePlayerIndex = index;
        newPlayer.SetActive(true);
        newPlayer.transform.position = spawnPos;

        newPlayer.GetComponent<PlayerHealth>().SetHealthBar(healthSlider);
        CameraManager.Instance.SetFollow(newPlayer.transform);
        EnemyTargetProvider.Instance.SetTarget(newPlayer.transform);

        return true;
    }

    public GameObject GetActivePlayer()
    {
        if (activePlayerIndex != -1 && unlockedPlayers.ContainsKey(activePlayerIndex))
            return unlockedPlayers[activePlayerIndex];
        return null;
    }

    public int GetActivePlayerIndex() => activePlayerIndex;

    public bool IsCharacterUnlocked(int index) => unlockedPlayers.ContainsKey(index);

    public int UnlockedCharacterCount() => unlockedPlayers.Count;

    public bool IsCharacterAlive(int index)
    {
        return aliveCharacterMap.ContainsKey(index) && aliveCharacterMap[index];
    }

    private void CacheCharacterHPSliders()
    {
        characterHPSliders.Clear();
        foreach (Transform character in charactersUIParent)
        {
            Slider hpSlider = character.GetChild(1).GetComponent<Slider>();
            characterHPSliders.Add(hpSlider);
        }
    }
}
