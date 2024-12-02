using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance; // Singleton để dễ quản lý.

    public AudioSource audioSource;
    private bool isMusicOn;

    private void Awake()
    {
        // Đảm bảo MusicManager không bị xóa khi chuyển cảnh.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Lấy trạng thái nhạc từ PlayerPrefs (mặc định bật).
        isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        UpdateMusicState();
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;

        // Lưu trạng thái nhạc vào PlayerPrefs.
        PlayerPrefs.SetInt("MusicOn", isMusicOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateMusicState();
    }

    private void UpdateMusicState()
    {
        if (isMusicOn)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}
