using UnityEngine;
using TMPro;
using System.Collections;

[DefaultExecutionOrder(-3)]
public class UISingleton : Singleton<UISingleton>
{
    [SerializeField] private TextMeshProUGUI healTextPrefab;  // Prefab của TextMeshPro
    [SerializeField] private TextMeshProUGUI dmgTakePrefab;
    [SerializeField] private TextMeshProUGUI dmgDealPrefab;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI warningText;
    private void Start()
    {
    }

    public void ShowHealEffect(Transform targetTransform, int amount)
    {
        // Tạo hiệu ứng heal
        TextMeshProUGUI healText = Instantiate(healTextPrefab, transform);  // Gán parent là Canvas
        healText.text = "+" + amount + "HP";
        Vector3 target = targetTransform.position;
        target.x += 1f;

        // Đặt vị trí hiển thị text trên màn hình
        healText.transform.position = Camera.main.WorldToScreenPoint(target);

        // Gọi Coroutine để di chuyển text lên
        StartCoroutine(MoveTextUp(healText));
    }

    private IEnumerator MoveTextUp(TextMeshProUGUI healText)
    {
        Vector3 initialPosition = healText.rectTransform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, 100f, 0);  // Di chuyển lên 100 đơn vị

        float elapsedTime = 0f;
        float duration = 1f;  // Thời gian di chuyển

        while (elapsedTime < duration)
        {
            healText.rectTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        healText.rectTransform.position = targetPosition;  // Đảm bảo đạt được vị trí cuối cùng
        Destroy(healText.gameObject);  // Hủy sau khi di chuyển xong
    }

    public void ShowDmgTakeEffect(Transform targetTransform, int amount)
    {
        // Tạo hiệu ứng heal
        TextMeshProUGUI dmgText = Instantiate(dmgTakePrefab, transform);  // Gán parent là Canvas
        dmgText.text = "-" + amount + "HP";
        Vector3 target = targetTransform.position;
        target.x += 0.5f;

        // Đặt vị trí hiển thị text trên màn hình
        dmgText.transform.position = Camera.main.WorldToScreenPoint(target);

        // Gọi Coroutine để di chuyển text lên
        StartCoroutine(MoveTextUp(dmgText));
    }

    public void ShowDmgDealEffect(Transform targetTransform, int amount)
    {
        // Tạo hiệu ứng heal
        TextMeshProUGUI dmgText = Instantiate(dmgDealPrefab, transform);  // Gán parent là Canvas
        dmgText.text =  amount.ToString();
        Vector3 target = targetTransform.position;
        target.x += 0.5f;

        // Đặt vị trí hiển thị text trên màn hình
        dmgText.transform.position = Camera.main.WorldToScreenPoint(target);

        // Gọi Coroutine để di chuyển text lên
        StartCoroutine(MoveTextUp(dmgText));
    }
    public void ShowGameOverPopup()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ShowLevelCompletedPopup()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }
    public void ShowMessageText(Transform targetTransform, string message)
    {
        // Tạo hiệu ứng heal
        TextMeshProUGUI mgsText = Instantiate(messageText, transform);  // Gán parent là Canvas
        mgsText.text = message;
        Vector3 target = targetTransform.position;
        target.y += 1f;
        target.x += 1f;

        // Đặt vị trí hiển thị text trên màn hình
        mgsText.transform.position = Camera.main.WorldToScreenPoint(target);

        // Gọi Coroutine để di chuyển text lên
        StartCoroutine(MoveTextUp(mgsText));
    }

    public void ShowWarningText(Transform targetTransform, string message)
    {
        // Tạo hiệu ứng heal
        TextMeshProUGUI mgsText = Instantiate(warningText, transform);  // Gán parent là Canvas
        mgsText.text = message;
        Vector3 target = targetTransform.position;
        target.y += 1f;
        target.x += 1f;

        // Đặt vị trí hiển thị text trên màn hình
        mgsText.transform.position = Camera.main.WorldToScreenPoint(target);

        // Gọi Coroutine để di chuyển text lên
        StartCoroutine(MoveTextUp(mgsText));
    }
    public TextMeshProUGUI ShowMessageStay(Transform targetTransform, string message)
    {
        // Tạo hiệu ứng heal
        TextMeshProUGUI mgsText = Instantiate(messageText, transform);  // Gán parent là Canvas
        mgsText.text = message;
        Vector3 target = targetTransform.position;
        target.y += 1f;
        target.x += 1f;

        // Đặt vị trí hiển thị text trên màn hình
        mgsText.transform.position = Camera.main.WorldToScreenPoint(target);
        return mgsText;
    }
    public void HideMessageStay(TextMeshProUGUI mgsText)
    {
        if (mgsText != null)
        {
            Destroy(mgsText.gameObject);
        }
    }
}

