using UnityEngine;

using System.Collections;
public class ChangeCharacter : MonoBehaviour
{
    private int activeSlotIndexNum = 0;
    private bool isCooldown = false;
    private void Start()
    {
        HighlightSelectedChar(PlayerManager.Instance.GetActivePlayerIndex());
    }

    private void OnEnable()
    {
        PlayerInputManager.Instance.OnCharacterSelect += TriggerSlot;
    }

    private void OnDisable()
    {
        PlayerInputManager.Instance.OnCharacterSelect -= TriggerSlot;
    }


    public void TriggerSlot(int numValue)
    {
        if (isCooldown)
            return;

        if (!PlayerManager.Instance.IsCharacterUnlocked(numValue - 1) || !PlayerManager.Instance.IsCharacterAlive(numValue - 1))
        {
            Debug.LogWarning("Character not unlocked or alive: " + numValue);
            return;
        }
        if (PlayerManager.Instance.SwitchCharacter(numValue - 1))
        {
            HighlightSelectedChar(numValue - 1);
            StartCoroutine(CooldownCoroutine());
        }
        else return;
    }

    private IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(1.5f);
        isCooldown = false;
    }

    private void HighlightSelectedChar(int indexNum)
    {
        activeSlotIndexNum = indexNum;

        foreach (Transform character in transform)
        {
            if (!character.gameObject.activeSelf)
                continue;

            character.GetChild(0).GetChild(0).gameObject.SetActive(false);
            character.GetChild(1).gameObject.SetActive(true);
        }

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
