using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    public TMP_Text totalCoinsText; // Text hiển thị tổng số coin.
    public TMP_Text feedbackText;   // Text hiển thị thông báo (vd: "Mua thành công!").
    public TMP_Text[] priceTexts;   // Text hiển thị giá cho từng nhân vật.
    public int[] itemPrices;        // Giá của các vật phẩm.
    public GameObject[] characters; // Prefab các nhân vật trong shop.
    public GameObject[] selectButtons; // Nút chọn nhân vật.
    public GameObject[] ownedIndicators; // Hiển thị trạng thái "Owned".
    public GameObject[] buyButtons; // Nút mua nhân vật.

    private int totalCoins;

    private void Start()
    {
        // Lấy tổng số coin từ PlayerPrefs.
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        UpdateTotalCoinsUI();

        // Hiển thị giá cho từng nhân vật.
        UpdatePriceTexts();

        // Lấy nhân vật mặc định nếu chưa chọn nhân vật nào.
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", -1);
        if (selectedCharacter == -1)
        {
            PlayerPrefs.SetInt("SelectedCharacter", 0); // Đặt nhân vật đầu tiên là mặc định.
            PlayerPrefs.Save();
        }

        // Cập nhật trạng thái các nút và nhân vật.
        UpdateShopUI();
    }


    public void BuyItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= itemPrices.Length)
        {
            Debug.LogError("Item index không hợp lệ.");
            return;
        }

        int itemPrice = itemPrices[itemIndex];

        // Kiểm tra nếu đủ tiền và nhân vật chưa được mua
        if (totalCoins >= itemPrice && PlayerPrefs.GetInt($"ItemPurchased_{itemIndex}", 0) == 0)
        {
            totalCoins -= itemPrice;

            // Lưu trạng thái đã mua vào PlayerPrefs.
            PlayerPrefs.SetInt($"ItemPurchased_{itemIndex}", 1);
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            PlayerPrefs.Save();

            feedbackText.text = "Mua thành công!";
            UpdateTotalCoinsUI();
            UpdateShopUI(); // Cập nhật giao diện shop
        }
        else
        {
            feedbackText.text = "Không đủ tiền hoặc đã sở hữu!";
        }
    }

    public void SelectCharacter(int itemIndex)
    {
        if (PlayerPrefs.GetInt($"ItemPurchased_{itemIndex}", 0) == 1) // Kiểm tra nếu nhân vật đã được mua.
        {
            // Lưu chỉ số nhân vật đã chọn vào PlayerPrefs.
            PlayerPrefs.SetInt("SelectedCharacter", itemIndex);
            PlayerPrefs.Save();

            // Gọi GameManager để thay đổi nhân vật ngay lập tức.
            GameManager.instance.ChangeCharacter(itemIndex);

            // Cập nhật giao diện cửa hàng.
            UpdateShopUI();
            feedbackText.text = "Nhân vật đã được chọn!";
        }
        else
        {
            feedbackText.text = "Bạn cần mua nhân vật này trước!";
        }
    }

    private void UpdateShopUI()
    {
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", -1); // Lấy chỉ số nhân vật đã chọn.

        for (int i = 0; i < itemPrices.Length; i++)
        {
            bool isPurchased = PlayerPrefs.GetInt($"ItemPurchased_{i}", 0) == 1;

            // Cập nhật chỉ báo "Owned"
            if (ownedIndicators.Length > i && ownedIndicators[i] != null)
                ownedIndicators[i].SetActive(isPurchased);

            // Cập nhật nút "Select"
            if (selectButtons.Length > i && selectButtons[i] != null)
            {
                selectButtons[i].SetActive(isPurchased);

                // Làm nổi bật nút nếu là nhân vật đang được chọn.
                if (i == selectedCharacterIndex)
                {
                    selectButtons[i].GetComponent<UnityEngine.UI.Button>().interactable = false; // Tắt nút chọn.
                }
                else
                {
                    selectButtons[i].GetComponent<UnityEngine.UI.Button>().interactable = true; // Bật nút chọn.
                }
            }

            // Cập nhật nút mua và giá nếu chưa mua
            if (buyButtons.Length > i && buyButtons[i] != null)
                buyButtons[i].SetActive(!isPurchased);

            if (priceTexts.Length > i && priceTexts[i] != null)
                priceTexts[i].gameObject.SetActive(!isPurchased);
        }
    }

    private void UpdateTotalCoinsUI()
    {
        if (totalCoinsText != null)
            totalCoinsText.text = totalCoins.ToString();
    }

    public void ResetShopData()
    {
        // Xóa toàn bộ dữ liệu nhân vật đã mua.
        for (int i = 0; i < itemPrices.Length; i++)
        {
            PlayerPrefs.DeleteKey($"ItemPurchased_{i}"); // Xóa trạng thái mua của từng nhân vật.
        }

        // Xóa nhân vật đã chọn.
        PlayerPrefs.DeleteKey("SelectedCharacter");

        // Đặt lại tổng tiền (nếu cần).
        PlayerPrefs.DeleteKey("TotalCoins");
        totalCoins = 0; // Reset biến cục bộ.

        PlayerPrefs.Save(); // Lưu lại sau khi reset.

        // Cập nhật giao diện.
        UpdateTotalCoinsUI();
        UpdateShopUI();
        feedbackText.text = "Dữ liệu đã được reset!";
    }

    private void UpdatePriceTexts()
    {
        for (int i = 0; i < itemPrices.Length; i++)
        {
            if (priceTexts.Length > i && priceTexts[i] != null)
            {
                priceTexts[i].text = itemPrices[i] + "";
            }
        }
    }
}
