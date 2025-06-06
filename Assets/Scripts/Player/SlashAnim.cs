using UnityEngine;

public class SlashAnim : MonoBehaviour
{
    
    public void DestroySelf()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
    
}
