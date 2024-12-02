using TMPro;
using UnityEngine;

public class ScoreAndCoin : MonoBehaviour
{
    public static ScoreAndCoin instance;

    [Header("Score and Coin")]
    public TMP_Text coinText;   // Text hiển thị coin hiện tại.
    public TMP_Text scoreText;  // Text hiển thị điểm.

    private int coin;  // Số coin hiện tại của phiên chơi.
    private int score; // Điểm hiện tại của phiên chơi.
    private int totalCoins; // Tổng số coin đã tích lũy.

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Lấy tổng số coin từ PlayerPrefs.
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        // Đặt giá trị mặc định khi game bắt đầu.
        UpdateCoinUI();
        UpdateScoreUI();
    }

    // Phương thức cộng coin.
    public void AddCoins(int amount)
    {
        coin += amount; // Cộng tiền vào biến tạm thời của `ScoreAndCoin`.
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0) + amount; // Lấy tiền từ PlayerPrefs và cộng thêm.
        PlayerPrefs.SetInt("TotalCoins", totalCoins); // Lưu lại tổng tiền.
        PlayerPrefs.Save(); // Lưu dữ liệu vĩnh viễn vào PlayerPrefs.

        UpdateCoinUI(); // Cập nhật UI.
        Debug.Log($"Thêm {amount} coin. Tổng tiền hiện tại: {totalCoins}");
    }

    // Phương thức cộng điểm.
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    // Gọi khi kết thúc game để lưu tổng coin.
    public void SaveCoins()
    {
        totalCoins += coin; // Cộng số coin của phiên chơi vào tổng coin.
        PlayerPrefs.SetInt("TotalCoins", totalCoins); // Lưu vào PlayerPrefs.
        PlayerPrefs.Save(); // Lưu dữ liệu.
    }

    // Lấy tổng số coin để hiển thị trong shop.
    public int GetTotalCoins()
    {
        return totalCoins;
    }

    // Cập nhật UI Coin.
    private void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = coin.ToString();
    }

    // Cập nhật UI Score.
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }
}
