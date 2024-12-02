using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class BossController : MonoBehaviour
{
    public bool isBoss = true;
    public float speed = 1f;            // Tốc độ di chuyển xuống.
    public float horizontalSpeed = 1f;  // Tốc độ di chuyển trái/phải.
    public float maxHealth = 100f;      // Máu tối đa của Elite Enemy.
    private float currentHealth;        // Máu hiện tại.
    private bool moveSideways = false;  // Kiểm tra xem có nên di chuyển ngang không.
    private AudioManager audioManager; //Tham chiếu đến AudioManger.
    private float damage = 10f;

    [Header("UI Elements")]
    public GameObject healthBarUI;      // GameObject chứa thanh máu.
    public Slider healthSlider;         // Thanh máu (Slider).

    [Header("Score and Coin")]
    public int coinReward = 10; // Số coin nhận được khi giết địch.
    public int scoreReward = 100; // Số điểm nhận được khi giết địch.


    private void Start()
    {
        currentHealth = maxHealth; // Đặt máu tối đa.
        healthSlider.value = CalculateHealth(); // Cập nhật thanh máu.
        PositionHealthBar(); // Đặt vị trí thanh máu.
    }

    private void Update()
    {
        MoveEnemy();        // Di chuyển Elite Enemy.
        PositionHealthBar(); // Cập nhật vị trí của thanh máu theo vị trí của Elite Enemy.
    }
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // Hàm tính toán vị trí và di chuyển của Elite Enemy.
    private void MoveEnemy()
    {
        if (!moveSideways)  // Nếu chưa đạt tới giới hạn để di chuyển ngang.
        {
            // Di chuyển xuống dưới.
            transform.Translate(Vector3.down * speed * Time.deltaTime);

            // Khi Elite Enemy chạm vị trí y cụ thể (ví dụ y = 2), bắt đầu di chuyển trái/phải.
            if (transform.position.y <= 2f)
            {
                moveSideways = true;  // Kích hoạt di chuyển ngang.
            }
        }
        else
        {
            // Di chuyển trái/phải khi đã tới giới hạn.
            transform.Translate(Vector3.right * horizontalSpeed * Time.deltaTime);

            // Đảo chiều khi chạm đến biên màn hình.
            if (transform.position.x >= Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x || transform.position.x <= Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x)
            {
                horizontalSpeed = -horizontalSpeed;  // Đảo chiều di chuyển.
            }
        }
    }

    // Cập nhật vị trí của thanh máu dưới Elite Enemy.
    private void PositionHealthBar()
    {
        // Vị trí thanh máu dưới kẻ thù (tùy chỉnh khoảng cách theo ý bạn).
        Vector3 healthBarPosition = transform.position;
        healthBarPosition.y -= 0.6f; // Đặt thanh máu phía dưới enemy.

        // Cập nhật vị trí của UI thanh máu.
        healthBarUI.transform.position = Camera.main.WorldToScreenPoint(healthBarPosition);
    }

    // Hàm nhận sát thương cho Elite Enemy.
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value = CalculateHealth();  // Cập nhật thanh máu.

        if (currentHealth <= 0)
        {
            Die();  // Chết khi máu bằng 0.
        }
    }
    // Tính tỷ lệ phần trăm máu để cập nhật thanh máu.

    private float CalculateHealth()
    {
        return currentHealth / maxHealth;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerMissile")) // Kiểm tra va chạm với đạn
        {
            TakeDamage(1); // Gọi hàm nhận sát thương, giá trị sát thương có thể tùy chỉnh.
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);

            }
        }
    }

    void Die()
    {
        // Gọi âm thanh từ AudioManager
        audioManager.PlaySound(audioManager.explo1Clip);
        // Gọi hiệu ứng nổ từ pool.
        GameObject explosion = ExplosionPool.instance.GetExplosion(transform.position);

        // Trả lại pool sau thời gian hiệu ứng kết thúc.
        StartCoroutine(ReturnExplosionToPool(explosion));

        // Cộng điểm và coin.
        ScoreAndCoin.instance.AddScore(scoreReward);
        ScoreAndCoin.instance.AddCoins(coinReward);

        // Gửi thông báo lên WaveManager nếu cần.
        WaveManager.instance.OnEnemyKilled(gameObject);

        // Hủy Boss.
        Destroy(gameObject);
    }

    private IEnumerator ReturnExplosionToPool(GameObject explosion)
    {
        yield return new WaitForSeconds(2f); // Thời gian tồn tại của hiệu ứng nổ.
        ExplosionPool.instance.ReturnToPool(explosion);
    }


}
