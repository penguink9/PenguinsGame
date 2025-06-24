using TMPro;
using UnityEngine;

public class GetCharacerInfor : MonoBehaviour
{
    public TMP_Text playerNameText;  

    void Start()
    {
        // Lấy tên người chơi từ PlayerPrefs và hiển thị lên UI
        string playerName = PlayerPrefs.GetString("PlayerName", "DefaultName");
        playerNameText.text = playerName;
    }
}
