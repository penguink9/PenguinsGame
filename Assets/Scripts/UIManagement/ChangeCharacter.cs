using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeCharacter : MonoBehaviour
{
    private int activeSlotIndexNum = 0;
    private bool isCooldown = false;

    // mapping giữa slot UI và index thật trong prefabDatabase
    private List<int> slotToIndexMap = new List<int>();

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

    public void TriggerSlot(int slotNum)
    {
        if (isCooldown)
            return;

        int listIndex = slotNum - 1;
        if (listIndex < 0 || listIndex >= slotToIndexMap.Count)
        {
            Debug.LogWarning("Slot number out of range: " + slotNum);
            return;
        }

        int realCharacterIndex = slotToIndexMap[listIndex];

        if (!PlayerManager.Instance.IsCharacterUnlocked(realCharacterIndex) || !PlayerManager.Instance.IsCharacterAlive(realCharacterIndex))
        {
            Debug.LogWarning("Character not unlocked or alive: index " + realCharacterIndex);
            return;
        }

        if (PlayerManager.Instance.SwitchCharacter(realCharacterIndex))
        {
            AudioManager.Instance.PlaySFX("Change Char");
            HighlightSelectedChar(realCharacterIndex);
            StartCoroutine(CooldownCoroutine());
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(1.5f);
        isCooldown = false;
    }

    private void HighlightSelectedChar(int realIndex)
    {
        activeSlotIndexNum = realIndex;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform character = transform.GetChild(i);
            if (!character.gameObject.activeSelf)
                continue;

            character.GetChild(0).GetChild(0).gameObject.SetActive(false); // Remove highlight
            character.GetChild(1).gameObject.SetActive(true);              // Show normal state
        }

        if (realIndex >= 0 && realIndex < transform.childCount)
        {
            Transform selectedCharacter = transform.GetChild(realIndex);
            selectedCharacter.GetChild(0).GetChild(0).gameObject.SetActive(true); // Add highlight
            selectedCharacter.GetChild(1).gameObject.SetActive(false);           // Hide normal state
        }
    }


    public void UpdateCharBox(List<int> unlockedIndices)
    {
        int totalCharBox = transform.childCount;
        slotToIndexMap = new List<int>();

        for (int i = 0; i < totalCharBox; i++)
        {
            Transform charBox = transform.GetChild(i);
            if (unlockedIndices.Contains(i))
            {
                charBox.gameObject.SetActive(true);
                slotToIndexMap.Add(i); // Lưu index thật tương ứng với slot này
            }
            else
            {
                charBox.gameObject.SetActive(false);
            }
        }
    }
    public int GetSlotIndexByPrefabIndex(int prefabIndex)
    {
        return slotToIndexMap.IndexOf(prefabIndex);
    }

}
