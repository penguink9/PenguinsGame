using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    private int activeSlotIndexNum = 0;

    private PlayerController playerControls;
    private int unlockedCharacterCount;

    private void Awake()
    {
        playerControls = new PlayerController();
    }

    private void Start()
    {
        playerControls.SelectChar.Keyboard.performed += ctx => TriggerSlot((int)ctx.ReadValue<float>());
        unlockedCharacterCount = PlayerManager.Instance.UnlockedCharacterCount();
        UpdateCharBox(unlockedCharacterCount);
        HighlightSelectedChar(PlayerManager.Instance.GetActivePlayerIndex());
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void TriggerSlot(int numValue)
    {
        if(PlayerManager.Instance.IsCharacterUnlocked(numValue - 1) == false)
            return;
        HighlightSelectedChar(numValue - 1);
        PlayerManager.Instance.SwitchCharacter(numValue - 1);
    }

    private void HighlightSelectedChar(int indexNum)
    {
        activeSlotIndexNum = indexNum;

        // Set all character highlights to inactive and active Hp Slider
        foreach (Transform character in transform)
        {
            if (!character.gameObject.activeSelf)
                continue;

            character.GetChild(0).GetChild(0).gameObject.SetActive(false);
            character.GetChild(1).gameObject.SetActive(true);

        }

        // Set active character highlight
        Transform selectedCharacter = transform.GetChild(indexNum);
        selectedCharacter.GetChild(0).GetChild(0).gameObject.SetActive(true);
        selectedCharacter.GetChild(1).gameObject.SetActive(false);
    }


    public void UpdateCharBox(int unlockedCount)
    {
        int totalCharBox = transform.childCount;

        for (int i = 0; i < totalCharBox; i++)
        {
            Transform charBox = transform.GetChild(i);

            if (i < unlockedCount)
                charBox.gameObject.SetActive(true);
            else
                charBox.gameObject.SetActive(false);
        }
    }

}
