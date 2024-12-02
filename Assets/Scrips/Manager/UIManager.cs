using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Các form UI
    public GameObject formLogin;
    public GameObject formRegister;

    private void Start()
    {
        ShowLoginForm(); // Hiển thị form đăng nhập ban đầu
    }

    // Hiển thị form đăng nhập
    public void ShowLoginForm()
    {
        formLogin.SetActive(true);
        formRegister.SetActive(false);
    }

    // Hiển thị form đăng ký
    public void ShowRegisterForm()
    {
        formLogin.SetActive(false);
        formRegister.SetActive(true);
    }
}
