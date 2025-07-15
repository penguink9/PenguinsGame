using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-2)]
public class PlayerManager : Singleton<PlayerManager>, ILoadGameInit
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
        LoadGameInit();
    }
    public void LoadGameInit()
    {
        CacheCharacterHPSliders();
        if (!TrackCurrentMap.Instance.HasLoadData())
        {
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
            return;
        }
        // Load data from saved game
        var loadedData = DataManager.Instance.GetLoadedSlot().gameData.unlockCharacters;
        SetLoadData(loadedData, DataManager.Instance.GetLoadedSlot().gameData.activeCharacterIndex);
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
                InventoryManager.Instance.gameObject.SetActive(true);
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
        gameObject.SetActive(false);
        UISingleton.Instance.ShowGameOverPopup();
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
    public List<CharacterData> GetCharacterDatas()
    {
        List<CharacterData> datas = new List<CharacterData>();

        foreach (var kvp in unlockedPlayers)
        {
            int index = kvp.Key;
            GameObject playerObj = kvp.Value;
            PlayerHealth healthComponent = playerObj.GetComponent<PlayerHealth>();

            CharacterData data = new CharacterData
            {
                index = index,
                health = healthComponent != null ? healthComponent.GetCurrentHealth() : 0,
                isDead = aliveCharacterMap.ContainsKey(index) && !aliveCharacterMap[index]
            };

            datas.Add(data);
        }

        return datas;
    }
    public void SetLoadData(List<CharacterData> characterDatas, int activeCharacterIndex)
    {
        if (characterDatas == null || characterDatas.Count == 0)
        {
            Debug.LogWarning("Character data is empty, cannot load.");
            return;
        }

        // Clear hiện tại
        unlockedPlayers.Clear();
        aliveCharacterMap.Clear();

        // Cache lại sliders nếu cần
        CacheCharacterHPSliders();

        foreach (var data in characterDatas)
        {
            int index = data.index;

            // Bỏ qua index không hợp lệ
            if (index < 0 || index >= prefabDatabase.playerPrefabs.Length)
                continue;

            GameObject newPlayer = Instantiate(prefabDatabase.playerPrefabs[index], transform);
            var health = newPlayer.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.OnPlayerDeath += () => OnPlayerDeath(index);
                health.SetHealthBar(characterHPSliders[index]);
                health.SetCurrentHealth(data.health);
            }

            newPlayer.SetActive(false);
            unlockedPlayers[index] = newPlayer;
            aliveCharacterMap[index] = !data.isDead;

            // Cập nhật UI trạng thái chết nếu có
            var charUI = charactersUIParent.GetChild(index);
            if (charUI != null)
            {
                bool isDead = data.isDead;
                charUI.Find("CharBox/Death").gameObject.SetActive(isDead);
                charUI.Find("CharBox/CharAvt").gameObject.SetActive(!isDead);
            }
        }

        // Cập nhật danh sách UI
        var changeChar = charactersUIParent.GetComponent<ChangeCharacter>();
        changeChar.UpdateCharBox(new List<int>(unlockedPlayers.Keys));

        // Tự động Switch sang nhân vật đang active
        int slotIndex = changeChar.GetSlotIndexByPrefabIndex(activeCharacterIndex);
        if (slotIndex != -1)
        {
            changeChar.TriggerSlot(slotIndex + 1); // slotNum truyền vào TriggerSlot là từ 1-based
        }
        else
        {
            Debug.LogWarning($"Cannot find UI slot for character index {activeCharacterIndex}");
        }
    }



}
