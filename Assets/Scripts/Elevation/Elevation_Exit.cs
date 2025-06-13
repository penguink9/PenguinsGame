using UnityEngine;

public class Elevation_Exit : MonoBehaviour
{
    public Collider2D[] moutainColliders;
    public Collider2D[] boundaryColliders;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            foreach (Collider2D mountainCollider in moutainColliders)
            {
                mountainCollider.enabled = true;
            }

            foreach (Collider2D boundaryCollider in boundaryColliders)
            {
                boundaryCollider.enabled = false;
            }
            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }
}
