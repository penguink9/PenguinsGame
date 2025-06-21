using UnityEngine;

public class CapturedPenguin : MonoBehaviour
{
    [SerializeField] private int penguinIndex;

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
