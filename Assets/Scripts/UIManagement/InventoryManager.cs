using TMPro;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    private int healthPotions = 0;
    private int keyCount = 0;
    private PlayerController playerControls;
    private TextMeshProUGUI potionCountText;
    private TextMeshProUGUI keyCountText;


    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerController();
        potionCountText = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        keyCountText = transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void Start()
    {
        playerControls.Combat.Heal.performed += _ => Healing();
        playerControls.Combat.Interact.performed += _ => UseKey();
    }
    private void Healing()
    {
        if(healthPotions <= 0)
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
    }
    private void UseKey()
    {
        if (keyCount <= 0)
        {
            Debug.LogWarning("No keys available to use.");
            return;
        }
        return;
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
}
