using UnityEngine;

[DefaultExecutionOrder(-1)]
public class UnLockCharacter : Singleton<UnLockCharacter>
{

    [Header("Unlock Character Panel")]
    public GameObject unlockPanel;
    public Animator panelAnimator;
    private TextTypewriter textTypewriter;
    private Transform characterPanel;
    private void Start()
    {
        if (unlockPanel == null)
        {
            Debug.LogError("Unlock Panel is not assigned in the inspector!");
        }
        if (panelAnimator == null)
        {
            Debug.LogError("Panel Animator is not assigned in the inspector!");
        }
        textTypewriter = unlockPanel.GetComponentInChildren<TextTypewriter>();
        characterPanel = unlockPanel.transform.GetChild(1);
    }
    public void ShowUnlockCharacterPanel(int characterIndex)
    {
        // Reset scale và alpha trước khi bật animation
        unlockPanel.transform.localScale = Vector3.zero;
        characterPanel.GetChild(characterIndex-1).gameObject.SetActive(true);
        string fulltext = "You have unlocked new character ! ";
        switch (characterIndex)
        {
            case 1:
                fulltext += "Kowalski";
                break;
            case 2:
                fulltext += "Skipper";
                break;
            case 3:
                fulltext += "Private";
                break;
            default:
                break;
        }
        unlockPanel.SetActive(true);
        textTypewriter.SetText(fulltext);
        CanvasGroup canvasGroup = unlockPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }
        panelAnimator.ResetTrigger("ShowPopUp");
        panelAnimator.SetTrigger("ShowPopUp");
    }

    public void HideUnlockCharacterPanel()
    {
        unlockPanel.SetActive(false);
        foreach (Transform child in characterPanel)
        {
            child.gameObject.SetActive(false);
        }
    }
    public void OnReceiveButtonClicked()
    {
        HideUnlockCharacterPanel();
    }
}
