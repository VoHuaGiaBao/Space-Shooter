using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileController : MonoBehaviour
{
    public float missileSpeed = 25f;
    public float damage = 10f;  // Sát thương của đạn.
    [Header("Explosion Prefab")]
    public GameObject explosionPrefab;
    // Update is called once per frame
    void Update()
    {
        // Di chuyển đạn lên phía trên.
        transform.Translate(Vector3.up * missileSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Va chạm với Enemy thường.
        if (collision.gameObject.tag == "Enemy")
        {

            // Hủy cả đạn và enemy sau khi va chạm.
            Destroy(this.gameObject);
            Destroy(collision.gameObject);

            // Gọi phương thức OnEnemyKilled và truyền vào đối tượng enemy
            WaveManager.instance.OnEnemyKilled(gameObject);

        }

        // Va chạm với Elite Enemy.
        if (collision.gameObject.tag == "EliteEnemy")
        {
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, 0.3f); // Hủy hiệu ứng nổ sau 2 giây (hoặc thời gian phù hợp)
            }
            EliteEnemyController eliteEnemy = collision.gameObject.GetComponent<EliteEnemyController>();
            if (eliteEnemy != null)
            {
                eliteEnemy.TakeDamage(damage);  // Gây sát thương cho Elite Enemy.
            }

            Destroy(this.gameObject);  // Hủy viên đạn sau khi va chạm.
        }

        // Va chạm với Boss.
        if (collision.gameObject.tag == "Boss")
        {
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, 0.3f); // Hủy hiệu ứng nổ sau 2 giây (hoặc thời gian phù hợp)
            }
            BossController boss = collision.gameObject.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(damage);  // Gây sát thương cho Boss.
            }

            Destroy(this.gameObject);  // Hủy viên đạn sau khi va chạm.
        }
    }

}
