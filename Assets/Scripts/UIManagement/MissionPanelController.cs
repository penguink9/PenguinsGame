using UnityEngine;

public class MissionPanelController : MonoBehaviour
{
    [Header("Mission Panel")]
    public GameObject missionPanel;
    public Animator panelAnimator;
    private TextTypewriter textTypewriter;
    private void Awake()
    {
        textTypewriter = missionPanel.GetComponentInChildren<TextTypewriter>();
    }
    public void ShowMissionPanel()
    {
        // Reset scale và alpha trước khi bật animation
        missionPanel.transform.localScale = Vector3.zero;

        int level = TrackCurrentMap.Instance.level;
        string fulltext = "";

        switch (level)
        {
            case 1:
                fulltext = "- Find the key by defeating elite enemy\n" +
                           "- Use the key to rescue Kowalski\n" +
                           "- Find the exit way";
                break;

            case 2:
                fulltext = "- Find the key by defeating elite enemy\n" +
                           "- Use the key to rescue Skipper and Private\n" +
                           "- Find the exit way";
                break;

            case 3:
                fulltext = "- Defeat Final Boss Polar Bear\n" +
                           "- Find the exit way";
                break;

            default:
                fulltext = "- Mission not defined for this level.";
                break;
        }

        missionPanel.SetActive(true);
        textTypewriter.SetText(fulltext);

        CanvasGroup canvasGroup = missionPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }

        panelAnimator.ResetTrigger("ShowPopUp");
        panelAnimator.SetTrigger("ShowPopUp");
    }
    public void HideMissionPanel()
    {
        missionPanel.SetActive(false);
    }
    public void OnPlayButtonClicked()
    {
        HideMissionPanel();
    }
}
