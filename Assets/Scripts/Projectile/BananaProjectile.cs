using UnityEngine;
using System.Collections;

public class BananaProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 0.1f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private GameObject splatterPrefab;

    private void Start()
    {

        //Vector3 playerPos = PlayerBase.Instance.transform.position;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player.transform.position;

        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPos));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector3.Lerp(startPosition, endPosition, linearT) + new Vector3(0f, height);

            yield return null;
        }

        Instantiate(splatterPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }


}
