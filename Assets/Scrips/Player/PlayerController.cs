using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] protected Vector3 worldPosion;
    [SerializeField] protected float speed = 0.1f; // Tốc độ di chuyển

    [Header("Missile")]
    public GameObject missile;
    public Transform missileSpawnPosition;
    public float destroyTime = 2f;
    public float fireRate = 0.5f;  // Tốc độ bắn đạn (thời gian giữa các lần bắn)

    [Header("Health Settings")]
    public float maxHealth = 100f;  // Máu tối đa của Player
    private float currentHealth;    // Máu hiện tại
    public GameObject healthBarUI;  // UI chứa thanh máu
    public Slider healthSlider;     // Slider dùng làm thanh máu
    public float healthBarOffset = 1.2f; // Vị trí thanh máu so với nhân vật

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;  // Khởi tạo máu tối đa
        healthSlider.value = CalculateHealth(); // Cập nhật thanh máu ban đầu

        InvokeRepeating("PlayerShoot", fireRate, fireRate);  // Bắt đầu tự động bắn đạn
    }

    void Update()
    {
        PlayerMovement();  // Di chuyển
    }

    void PlayerMovement()
    {
        Vector3 worldPos = Vector3.zero;

        // Điều khiển cảm ứng (Mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            worldPos = Camera.main.ScreenToWorldPoint(touch.position);
        }
        // Điều khiển bằng chuột (PC)
        else if (Input.GetMouseButton(0))
        {
            worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // Nếu không có đầu vào từ người chơi, trả về (không làm gì)
        if (worldPos == Vector3.zero) return;

        worldPos.z = 0;

        // Giới hạn vị trí của nhân vật trong màn hình
        Vector3 clampedPosition = worldPos;

        // Tính toán biên của màn hình
        float minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        // Giới hạn di chuyển theo biên của màn hình
        clampedPosition.x = Mathf.Clamp(worldPos.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(worldPos.y, minY, maxY);

        // Di chuyển nhân vật tới vị trí mới
        transform.position = Vector3.MoveTowards(transform.position, clampedPosition, speed); // Dùng MoveTowards thay vì Lerp cho tốc độ ổn định
    }

    void PlayerShoot()
    {
        // Tạo ra viên đạn tại vị trí của con tàu
        GameObject gm = Instantiate(missile, missileSpawnPosition.position, Quaternion.identity);
        gm.transform.SetParent(null);  // Đảm bảo viên đạn không được đặt dưới tàu
        Destroy(gm, destroyTime);       // Hủy viên đạn sau destroyTime giây
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value = CalculateHealth(); // Cập nhật thanh máu

        if (currentHealth <= 0)
        {
            Die(); // Gọi hàm chết nếu máu <= 0
        }
    }

    private float CalculateHealth()
    {
        return currentHealth / maxHealth; // Trả về phần trăm máu
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyMissile")) // Kiểm tra va chạm với đạn
        {
            TakeDamage(1); // Gọi hàm nhận sát thương, giá trị sát thương có thể tùy chỉnh.
        }
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
        if (other.gameObject.CompareTag("EliteEnemy"))
        {
            TakeDamage(1);
        }
        if (other.gameObject.CompareTag("Boss"))
        {
            TakeDamage(1);
        }


    }


    private void Die()
    {
        // Kiểm tra nếu nhân vật chưa chết và hiệu ứng chưa xuất hiện
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true; // Đánh dấu là nhân vật đã chết để tránh tạo thêm explosion

            // Gọi hiệu ứng nổ từ pool
            GameObject explosion = ExplosionPool.instance.GetExplosion(transform.position);

            // Thực hiện Coroutine để trả lại Explosion sau khi nổ xong
            StartCoroutine(HandleExplosionAndGameOver(explosion));
            Destroy(gameObject, 1f);
            GameManager.instance.GameOver();

        }
    }

    private IEnumerator HandleExplosionAndGameOver(GameObject explosion)
    {
        // Hiển thị explosion trong vài giây
        yield return new WaitForSeconds(2f); // Thời gian hiệu ứng nổ

        // Trả lại Explosion vào pool
        ExplosionPool.instance.ReturnToPool(explosion);

        // Chuyển đến màn hình Game Over
    }



}
