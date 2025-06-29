using System.Collections;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class InventoryManager : Singleton<InventoryManager>, ILoadGameInit
{
    private int healthPotions = 0;
    public int HealthPotions
    {
        get { return healthPotions; }
        set
        {
            healthPotions = value;
            UpdatePotionsCount();
        }
    }
    private int keyCount = 0;
    public int KeyCount
    {
        get { return keyCount; }
        set
        {
            keyCount = value;
            UpdateKeyCount();
        }
    }
    private TextMeshProUGUI potionCountText;
    private TextMeshProUGUI keyCountText;
    private bool isCooldown = false;
    private bool canInteract = false;
    private int unlockCharacterIndex = 0;
    public bool usedKey = false;
    protected override void Awake()
    {
        base.Awake();
        potionCountText = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        keyCountText = transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        LoadGameInit();
    }
    public void LoadGameInit()
    {
        if (!TrackCurrentMap.Instance.HasLoadData())
        {
            HealthPotions = 0;
            KeyCount = 0;
            return;
        }
        HealthPotions = DataManager.Instance.GetLoadedSlot().gameData.healthPotions;
        KeyCount = DataManager.Instance.GetLoadedSlot().gameData.keys;
    }
    private void OnEnable()
    {
        PlayerInputManager.Instance.OnHeal += Healing;
        PlayerInputManager.Instance.OnInteract += UseKey;
    }

    private void OnDisable()
    {
        PlayerInputManager.Instance.OnHeal -= Healing;
        PlayerInputManager.Instance.OnInteract -= UseKey;
    }
    private void Healing()
    {
        if (isCooldown) return;
        if (healthPotions <= 0)
        {
            Debug.LogWarning("No health potions available to use.");
            return;
        }
        if(PlayerManager.Instance.GetActivePlayer().GetComponent<PlayerHealth>().Heal(2))
        {
            healthPotions--;
            UpdatePotionsCount();
        } else
        {
            Debug.LogWarning("Player is already at full health or cannot heal further.");
        }
        StartCoroutine(HealingCooldown());
    }
    private IEnumerator HealingCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(1.5f);
        isCooldown = false;
    }
    private void UseKey()
    {
        if (!canInteract) return;
        if (keyCount <= 0)
        {
            Debug.LogWarning("No keys available to use.");
            return;
        }
        else
        {
            usedKey = true; // Set usedKey to true when a key is used
            keyCount--;
            if (PlayerManager.Instance.UnlockCharacter(unlockCharacterIndex))
            {
                UnLockCharacter.Instance.ShowUnlockCharacterPanel(unlockCharacterIndex);
                UpdateKeyCount();
                canInteract = false; // Reset interaction state after using a key
            } else
            {
                Debug.LogWarning("Failed to unlock character with index: " + unlockCharacterIndex);
            }
            
        }
    }
    public void PickupHealPotions()
    {
       healthPotions++;
       UpdatePotionsCount();
    }
    public void PickupKey()
    {
        keyCount++;
        UpdateKeyCount();
    }
    private void UpdatePotionsCount()
    {
       potionCountText.text = healthPotions.ToString();
    }
    private void UpdateKeyCount()
    {
        keyCountText.text = keyCount.ToString();
    }
    public void EnterInteractArea(int characterIndex)
    {
        unlockCharacterIndex = characterIndex;
        canInteract = true;
    }
    public void ExitInteractArea()
    {
        canInteract = false;
        unlockCharacterIndex = 0; // Reset the index when exiting the interact area
    }
}
