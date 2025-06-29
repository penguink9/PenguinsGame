using UnityEngine;
using TMPro;

[DefaultExecutionOrder(-2)]
public class CoinRecorder : Singleton<CoinRecorder>, ILoadGameInit
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

    private void Start()
    {
        LoadGameInit();
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
    public void LoadGameInit()
    {
        if(!TrackCurrentMap.Instance.HasLoadData())
        {
            TotalCoins = 0;
            return;
        }
        TotalCoins = DataManager.Instance.GetLoadedSlot().gameData.coinCollected; 
    }
}

