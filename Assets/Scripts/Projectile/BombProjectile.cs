using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;

public class BombProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private GameObject bombProjectileShadow;
    [SerializeField] private float maxRange = 5f; // Giới hạn khoảng cách bắn tối đa
    [SerializeField] private GameObject explosionPrefab;

    private void Start()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        mouseWorldPosition.z = 0f;
        Vector3 direction = mouseWorldPosition - transform.position;
        if (direction.magnitude > maxRange)
        {
            direction = direction.normalized * maxRange;
        }
        Vector3 targetPosition = transform.position + direction;

        GameObject bombShadow =
        Instantiate(bombProjectileShadow, transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);

        Vector3 bombShadowStartPosition = bombShadow.transform.position;
        

        StartCoroutine(ProjectileCurveRoutine(transform.position, targetPosition));
        StartCoroutine(MoveGrapeShadowRoutine(bombShadow, bombShadowStartPosition, targetPosition));
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

            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);

            yield return null;
        }
        Instantiate(explosionPrefab, transform.position , Quaternion.identity);

        Destroy(gameObject);
    }

    private IEnumerator MoveGrapeShadowRoutine(GameObject bombShadow, Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            bombShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT);
            yield return null;
        }

        Destroy(bombShadow);
    }
}
