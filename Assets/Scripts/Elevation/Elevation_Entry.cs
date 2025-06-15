using UnityEngine;

public class Eval : MonoBehaviour
{
    public Collider2D[] moutainColliders;
    public Collider2D[] boundaryColliders;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            foreach (Collider2D mountainCollider in moutainColliders)
            {                
                mountainCollider.enabled = false;                
            }

            foreach (Collider2D boundaryCollider in boundaryColliders)
            {
                boundaryCollider.enabled = true;
            }
            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder= 15;
        }
    }

}
