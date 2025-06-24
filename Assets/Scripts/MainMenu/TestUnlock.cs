using UnityEngine;

public class TestUnlock : MonoBehaviour
{
    public Sprite testCharacterSprite;
    void Update()
    {
        // Test lần lượt qua 3 level của map1 (index 0)
        if (Input.GetKeyDown(KeyCode.U)) UIManager.instance.OnLevelCompleted(0, 0);
        if (Input.GetKeyDown(KeyCode.J)) UIManager.instance.OnLevelCompleted(0, 1);
        if (Input.GetKeyDown(KeyCode.K)) UIManager.instance.OnLevelCompleted(0, 2);// <-- hoàn thành level cuối map1
        if (Input.GetKeyDown(KeyCode.B)) UIManager.instance.OnLevelCompleted(1, 0);// <-- mở level1 Map2 
        if (Input.GetKeyDown(KeyCode.C)) UIManager.instance.OnLevelCompleted(1, 1);// <-- mở level1 Map2 
        if (Input.GetKeyDown(KeyCode.D)) UIManager.instance.OnLevelCompleted(1, 2);// <-- mở level1 Map2 



        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("Đã reset PlayerPrefs, hãy reload lại scene.");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            UnLockCharacter.instance.ShowUnlockCharacterPanel();
        }
       
    }
}