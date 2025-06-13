using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerPrefabsDatabase prefabDatabase;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Transform charactersUIParent;
    [SerializeField] private Slider healthSlider;
    private List<Slider> characterHPSliders = new List<Slider>();

    private List<GameObject> unlockedPlayers = new List<GameObject>();
    private int activePlayerIndex = 0;


    private void Start()
    {
        CacheCharacterHPSliders();
        UnlockCharacter(0);
        UnlockCharacter(1);
        SwitchCharacter(0);
    }

    public void UnlockCharacter(int prefabIndex)
    {
        if (prefabIndex < 0 || prefabIndex >= prefabDatabase.playerPrefabs.Length) return;

        GameObject newPlayer = Instantiate(prefabDatabase.playerPrefabs[prefabIndex], transform);
        var health = newPlayer.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.SetHealthBar(characterHPSliders[prefabIndex]);
        }
        newPlayer.SetActive(false);
        unlockedPlayers.Add(newPlayer);        
    }


    public void SwitchCharacter(int index)
    {
        Vector3 currentPosition = unlockedPlayers[activePlayerIndex].transform.position;
        if (index < 0 || index >= unlockedPlayers.Count) return;
        //Set old player's health bar to the UI
        unlockedPlayers[activePlayerIndex].GetComponent<PlayerHealth>().SetHealthBar(characterHPSliders[activePlayerIndex]);

        // Deactivate all players and activate the selected one
        foreach (var player in unlockedPlayers)
            player.SetActive(false);
        // Activate the new player and set its position
        activePlayerIndex = index;
        unlockedPlayers[activePlayerIndex].SetActive(true);
        unlockedPlayers[activePlayerIndex].transform.position = currentPosition;
        // Update the camera to follow the new player
        cinemachineCamera.Follow = unlockedPlayers[activePlayerIndex].transform;
        // Set the health bar for the active player
        unlockedPlayers[activePlayerIndex].GetComponent<PlayerHealth>().SetHealthBar(healthSlider);
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
}
