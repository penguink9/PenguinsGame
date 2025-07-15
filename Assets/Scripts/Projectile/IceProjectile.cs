using System.Collections;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    [SerializeField] private GameObject splatterPrefab;
    [SerializeField] private float delay = 0.5f;
    private void Start()
    {
        StartCoroutine(SpawnSplatterRoutine());
    }

    private IEnumerator SpawnSplatterRoutine()
    {
        yield return new WaitForSeconds(delay);  // Đợi một khoảng thời gian

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 playerPos = player.transform.position;
           
            Instantiate(splatterPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }


}
