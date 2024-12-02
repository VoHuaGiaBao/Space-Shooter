using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("EnemyMissile")]
    public GameObject enemyMissile;
    public Transform enemyMissileSpawnPosition;
    public float destroyTime = 1f;
    public float fireRate = 0.5f;  // Tốc độ bắn đạn (thời gian giữa các lần bắn)

    void Start()
    {

        InvokeRepeating("EnemyShoot", fireRate, fireRate);  // Bắt đầu tự động bắn đạn theo chu kỳ fireRate
    }
    void EnemyShoot()
    {
        // Tạo ra viên đạn tại vị trí của con tàu
        GameObject gm = Instantiate(enemyMissile, enemyMissileSpawnPosition.position, Quaternion.identity);
        gm.transform.SetParent(null);  // Đảm bảo viên đạn không được đặt dưới tàu
        Destroy(gm, destroyTime);       // Hủy viên đạn sau destroyTime giây

    }
}
