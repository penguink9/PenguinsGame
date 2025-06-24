using UnityEngine;

public class CapturedPenguin : MonoBehaviour
{
    [SerializeField] private int penguinIndex;

    private void Awake()
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
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager.Instance.ExitInteractArea();
        }
    }
}
