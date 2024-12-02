using UnityEngine;

public class DebugTools : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Nhấn phím R để reset.
        {
            ResetLocalValues();
            ResetData();
        }
        if (Input.GetKeyDown(KeyCode.T)) // Nhấn phím T để thêm tiền.
        {
            AddDebugCoins(10000); // Thêm 1000 coin.
        }
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll(); // Xóa toàn bộ dữ liệu trong PlayerPrefs.
        PlayerPrefs.Save();
        Debug.Log("Dữ liệu đã được reset!");
    }
    public void AddDebugCoins(int amount)
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoins += amount;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
        Debug.Log($"Đã thêm {amount} coin. Tổng số coin: {totalCoins}");
    }
    private void ResetLocalValues()
    {
        // Reset tiền.
        if (ScoreAndCoin.instance != null)
        {
            ScoreAndCoin.instance.SaveCoins(); // Reset tổng tiền về 0.
        }

    }





}
