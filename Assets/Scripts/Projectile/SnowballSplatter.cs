using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
public class SnowballSplatter : MonoBehaviour
{
    private SpriteFade spriteFade;

    private void Awake()
    {
        spriteFade = GetComponent<SpriteFade>();

    }
    private void Start()
    {
        StartCoroutine(spriteFade.SlowFadeRoutine());
        Invoke("DisableCollider", 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        enemyHealth?.TakeSlow(2, transform);
    }

    private void DisableCollider()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
