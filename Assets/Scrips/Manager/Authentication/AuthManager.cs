using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using System;
using UnityEngine.SceneManagement;



public class AuthManager : MonoBehaviour
{

    private FirebaseAuth auth;
    private FirebaseUser user;

    [Header("Login Form")]
    public InputField emailInputField;
    public InputField passwordInputField;
    public Text messageLoginText;
    public Button loginButton;

    [Header("Register Form")]
    public InputField registerEmailInputField;
    public InputField registerPasswordInputField;
    public InputField confirmPasswordInputField;
    public Text messageRegisterText; 
    public Button registerButton;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                user = auth.CurrentUser;

                if (user != null)
                {
                    messageLoginText.text = "Chào mừng trở lại, " + user.Email;
                    Debug.Log("User already logged in: " + user.Email);
                }
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {task.Result}");
            }
        });
    }


    public void Register()
    {
        string regemail = registerEmailInputField.text.Trim();
        string regpassword = registerPasswordInputField.text.Trim();
        string confirmPassword = confirmPasswordInputField.text.Trim();

        if (string.IsNullOrEmpty(regemail) || string.IsNullOrEmpty(regpassword) || string.IsNullOrEmpty(confirmPassword))
        {
            messageRegisterText.text = "Vui lòng nhập đầy đủ email, mật khẩu và xác nhận mật khẩu!";
            return;
        }

        if (regpassword != confirmPassword)
        {
            messageRegisterText.text = "Mật khẩu không khớp!";
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(regemail, regpassword).ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                FirebaseUser user = task.Result.User;
                messageRegisterText.text = "Tạo tài khoản thành công! Chào mừng, " + user.Email;
                Debug.Log("User created successfully: " + user.Email);

                // Chuyển sang GameScene
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                FirebaseException fbEx = task.Exception?.Flatten().InnerExceptions[0] as FirebaseException;
                messageRegisterText.text = "Tạo tài khoản thất bại: " + fbEx?.Message;
                Debug.LogError("User creation failed: " + fbEx?.Message);
            }
        });
    }



    public void Login()
    {
        string email = emailInputField.text.Trim();
        string password = passwordInputField.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            messageLoginText.text = "Vui lòng nhập email và mật khẩu!";
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                FirebaseUser user = task.Result.User;
                messageLoginText.text = "Đăng nhập thành công! Chào mừng, " + user.Email;
                Debug.Log("Login successful! User email: " + user.Email);

                // Chuyển sang GameScene
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                FirebaseException fbEx = task.Exception?.Flatten().InnerExceptions[0] as FirebaseException;
                messageLoginText.text = "Đăng nhập thất bại: " + fbEx?.Message;
                Debug.LogError("Login failed: " + fbEx?.Message);
            }
        });
    }


    public void ForgotPassword()
    {
        string email = emailInputField.text.Trim();

        if (string.IsNullOrEmpty(email))
        {
            messageLoginText.text = "Vui lòng nhập email để đặt lại mật khẩu!";
            return;
        }

        auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                messageLoginText.text = "Email đặt lại mật khẩu đã được gửi!";
                Debug.Log("Password reset email sent.");
            }
            else
            {
                messageLoginText.text = "Không thể gửi email: " + task.Exception?.Flatten().InnerExceptions[0].Message;
                Debug.LogError("Failed to send password reset email: " + task.Exception?.Flatten().InnerExceptions[0].Message);
            }
        });
    }
    private string GetFirebaseErrorMessage(FirebaseException exception)
    {
        switch ((AuthError)exception.ErrorCode)
        {
            case AuthError.EmailAlreadyInUse:
                return "Email này đã được sử dụng. Vui lòng chọn email khác.";
            case AuthError.WeakPassword:
                return "Mật khẩu quá yếu. Vui lòng chọn mật khẩu mạnh hơn.";
            case AuthError.InvalidEmail:
                return "Định dạng email không hợp lệ.";
            default:
                return "Đã xảy ra lỗi. Vui lòng thử lại.";
        }
    }
    public void PlayAsGuest()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                FirebaseUser user = task.Result.User;
                messageLoginText.text = "Đăng nhập ẩn danh thành công! ID: " + user.UserId;
                Debug.Log("Anonymous login successful! User ID: " + user.UserId);

                // Chuyển sang GameScene
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                FirebaseException fbEx = task.Exception?.Flatten().InnerExceptions[0] as FirebaseException;
                messageLoginText.text = "Đăng nhập ẩn danh thất bại: " + fbEx?.Message;
                Debug.LogError("Anonymous login failed: " + fbEx?.Message);
            }
        });
    }





}
