using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;

    [Header("Score and Coin")]
    public int coinReward = 10;          // Số coin nhận được khi giết địch.
    public int scoreReward = 100;        // Số điểm nhận được khi giết địch.

    [Header("Auto-Destruction")]
    public float autoDestroyTime = 10f;  // Thời gian tối đa tồn tại trước khi bị tự hủy.
    private float damage = 10f;

    private bool killedByPlayer = false; // Cờ kiểm tra xem kẻ địch bị tiêu diệt bởi người chơi.

    private PlayerController playerController;

    private AudioManager audioManager;

    private void Start()
    {

        // Bắt đầu đếm thời gian tự hủy khi kẻ địch xuất hiện
        StartCoroutine(AutoDestroyCountdown());
    }

    void Update()
    {
        // Di chuyển xuống dưới.
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    private void Awake()
    {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerMissile")) // Kiểm tra va chạm với đạn
        {

            killedByPlayer = true; // Đặt cờ cho biết kẻ địch bị tiêu diệt bởi người chơi.
            Die(); // Gọi hàm Die.
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);

            }
            Die();
        }    
    }
    

    void Die()
    {
        // Gọi âm thanh từ AudioManager
        audioManager.PlaySound(audioManager.exploClip);
        // Gọi hiệu ứng nổ từ pool.
        GameObject explosion = ExplosionPool.instance.GetExplosion(transform.position);

        // Trả lại pool sau thời gian hiệu ứng kết thúc.
        StartCoroutine(ReturnExplosionToPool(explosion));

        // Thông báo cho WaveManager rằng kẻ địch đã bị tiêu diệt.
        WaveManager.instance.OnEnemyKilled(gameObject);

        // Chỉ cộng điểm/coin nếu kẻ địch bị tiêu diệt bởi người chơi.
        if (killedByPlayer)
        {
            ScoreAndCoin.instance.AddScore(scoreReward);
            ScoreAndCoin.instance.AddCoins(coinReward);
        }

        // Hủy kẻ địch.
        Destroy(gameObject);
    }

    private IEnumerator AutoDestroyCountdown()
    {
        // Chờ trong khoảng thời gian `autoDestroyTime`.
        yield return new WaitForSeconds(autoDestroyTime);

        // Kẻ địch tự hủy nếu không bị tiêu diệt trong thời gian quy định.
        Debug.Log("Enemy auto-destroyed due to timeout.");
        Die();
    }

    private IEnumerator ReturnExplosionToPool(GameObject explosion)
    {
        yield return new WaitForSeconds(1f); // Thời gian tồn tại của hiệu ứng nổ.
        ExplosionPool.instance.ReturnToPool(explosion);
    }
}
