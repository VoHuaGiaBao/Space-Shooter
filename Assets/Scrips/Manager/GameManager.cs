using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Settings")]
    public Transform spawnPoint; // Vị trí spawn nhân vật.
    public GameObject[] characterPrefabs; // Danh sách các prefab nhân vật.
    private GameObject currentPlayer;

    [Header("UI Panels")]
    public GameObject startMenu; // Menu bắt đầu.
    public GameObject pauseMenu; // Menu tạm dừng.
    public GameObject gameOverMenu; // Menu khi thua.
    public GameObject gameWonMenu; // Menu khi thắng.
    public GameObject shopMenu;
    public GameObject settingMenu;

    [Header("In game bar")]
    public GameObject ingameBar;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {


        // Hiển thị UI menu bắt đầu.
        Time.timeScale = 0f; // Tạm dừng game cho đến khi người chơi nhấn "Start".
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gameWonMenu.SetActive(false);
        ingameBar.SetActive(false);

    }

    public void StartGame()
    {
        Time.timeScale = 1f; // Bắt đầu game.
        startMenu.SetActive(false);
        ingameBar.SetActive(true);

        EnsureDefaultCharacter(); // Đảm bảo có nhân vật mặc định.
        LoadSelectedCharacter();  // Tải nhân vật đã chọn hoặc mặc định.

        // Nếu có WaveManager, bắt đầu waves.
        if (WaveManager.instance != null)
        {
            WaveManager.instance.StartWaves();
        }
    }


    public void PauseGame()
    {
        Time.timeScale = 0f; // Tạm dừng game.
        pauseMenu.SetActive(true);
        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Tiếp tục game.
        pauseMenu.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0f; // Dừng game.
        gameOverMenu.SetActive(true);
        ScoreAndCoin.instance.SaveCoins();
    }

    public void GameWon()
    {
        Time.timeScale = 0f; // Dừng game.
        gameWonMenu.SetActive(true);
        ScoreAndCoin.instance.SaveCoins();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ShopMenu()
    {
        Time.timeScale = 0f;
        shopMenu.SetActive(true);
        startMenu.SetActive(false);
    }    
    public void SettingMenu()
    {
        Time.timeScale = 0f;
        settingMenu.SetActive(true);
        startMenu.SetActive(false);
    }
   

    private void LoadSelectedCharacter()
    {
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        Debug.Log($"Đang tải nhân vật: {selectedCharacterIndex}");

        if (selectedCharacterIndex >= 0 && selectedCharacterIndex < characterPrefabs.Length)
        {
            if (currentPlayer != null)
            {
                Destroy(currentPlayer);
            }

            currentPlayer = Instantiate(characterPrefabs[selectedCharacterIndex], spawnPoint.position, Quaternion.identity);
            Debug.Log("Nhân vật đã được tạo.");
        }
        else
        {
            Debug.LogError("Chỉ số nhân vật không hợp lệ.");
        }
    }

    // Hàm này có thể gọi từ ShopManager để thay đổi nhân vật ngay lập tức.
    public void ChangeCharacter(int newCharacterIndex)
    {
        if (newCharacterIndex >= 0 && newCharacterIndex < characterPrefabs.Length)
        {
            PlayerPrefs.SetInt("SelectedCharacter", newCharacterIndex); // Lưu lại lựa chọn.
            PlayerPrefs.Save();
            LoadSelectedCharacter(); // Tải nhân vật mới.
        }
        else
        {
            Debug.LogError($"Không thể chuyển đổi sang nhân vật {newCharacterIndex}, chỉ số không hợp lệ.");
        }
    }
    private void EnsureDefaultCharacter()
    {
        // Nếu không có nhân vật nào được chọn, đặt nhân vật mặc định (index = 0).
        if (!PlayerPrefs.HasKey("SelectedCharacter"))
        {
            PlayerPrefs.SetInt("SelectedCharacter", 0);
            PlayerPrefs.Save();
        }
    }

}
