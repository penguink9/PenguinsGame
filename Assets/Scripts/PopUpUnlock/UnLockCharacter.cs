using UnityEngine;

public class UnLockCharacter : MonoBehaviour
{
    public static UnLockCharacter instance;

    [Header("Unlock Character Panel")]
    public GameObject unlockPanel;
    public Animator panelAnimator;

    [Header("Cage Logic")]
    public GameObject cageObject;
    public int cageHealth = 3;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // Ẩn panel lúc bắt đầu
        if (unlockPanel != null)
            unlockPanel.SetActive(false);
    }

    public void DamageCage(int damage = 1)
    {
        if (cageObject == null) return;
        cageHealth -= damage;
        if (cageHealth <= 0)
        {
            Destroy(cageObject);
            ShowUnlockCharacterPanel();
        }
    }

    public void ShowUnlockCharacterPanel()
    {
        // Reset scale và alpha trước khi bật animation
        unlockPanel.transform.localScale = Vector3.zero;
        unlockPanel.SetActive(true);
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
    }
    public void OnReceiveButtonClicked()
    {
        HideUnlockCharacterPanel();
    }
}
