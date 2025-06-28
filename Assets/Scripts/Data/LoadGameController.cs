using TMPro;
using UnityEngine;

public class LoadGameController : MonoBehaviour
{
    [SerializeField] GameObject slot1Container;
    [SerializeField] GameObject slot2Container;
    [SerializeField] GameObject slot3Container;
    [SerializeField] GameObject slot4Container;

    public TextMeshProUGUI GetPlayerNameText(GameObject obj)
    {
        return obj.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    public TextMeshProUGUI GetLevelText(GameObject obj)
    {
        return obj.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    public TextMeshProUGUI GetDateText(GameObject obj)
    {
        return obj.transform.GetChild(1).GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    public TextMeshProUGUI GetSlotNameText(GameObject obj)
    {
        return obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public GameObject GetEmptyText(GameObject obj)
    {
        return obj.transform.GetChild(1).GetChild(0).gameObject;
    }
}
