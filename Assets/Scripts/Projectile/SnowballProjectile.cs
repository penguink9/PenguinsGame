using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Splines;


public class SnowballProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 0.35f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 3f;
    //[SerializeField] private GameObject snowballProjectileShadow;
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

        


        StartCoroutine(ProjectileCurveRoutine(transform.position, targetPosition));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;
        //endPosition.y -= 1f;

        // In ra giá trị bắt đầu và kết thúc
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);
            // In ra vị trí tính toán tại mỗi bước

            Debug.Log("Current Position: " + transform.position.y);
            yield return null;
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    
}
