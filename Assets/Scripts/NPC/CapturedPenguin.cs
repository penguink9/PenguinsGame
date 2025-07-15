using TMPro;
using UnityEngine;

public class CapturedPenguin : MonoBehaviour
{
    [SerializeField] private int penguinIndex;
    private TextMeshProUGUI text;

    private void Start()
    {
        MapStateManager.Instance.isCapturedPenguinUnlocked.TryGetValue(penguinIndex, out bool isUnlocked);
        if (isUnlocked)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text = UISingleton.Instance.ShowMessageStay(transform,"Press F to use key");
            InventoryManager.Instance.EnterInteractArea(penguinIndex);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if( collision.CompareTag("Player") && InventoryManager.Instance.usedKey)
        {
            InventoryManager.Instance.usedKey = false;
            Debug.Log("Penguin Captured: " + penguinIndex);
            MapStateManager.Instance.isCapturedPenguinUnlocked[penguinIndex] = true;
            UISingleton.Instance.HideMessageStay(text);
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager.Instance.ExitInteractArea();
            UISingleton.Instance.HideMessageStay(text);
        }
    }
}
