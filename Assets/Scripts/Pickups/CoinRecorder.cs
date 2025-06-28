using UnityEngine;
using TMPro;

public class CoinRecorder : Singleton<CoinRecorder>
{
    private int totalCoins;
    public int TotalCoins
    {
        get { return totalCoins; }
        set
        {
            totalCoins = value;
            UpdateCoinUI();
        }
    }

    private TextMeshProUGUI coinText;

    protected override void Awake()
    {
        base.Awake();

        coinText = GetComponent<TextMeshProUGUI>();
        if (coinText == null)
        {
            Debug.LogError("CoinRecorder: Không tìm thấy TextMeshProUGUI!");
        }

        UpdateCoinUI();
    }

    public void PickupCoin()
    {
        TotalCoins++;
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = totalCoins.ToString();
        }
    }
}

