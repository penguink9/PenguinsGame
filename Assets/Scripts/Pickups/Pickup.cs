using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            OnPickup();
            Destroy(gameObject);
        }
    }
    public virtual void OnPickup()
    {
        // This method can be overridden by derived classes to implement specific pickup behavior
    }
}
