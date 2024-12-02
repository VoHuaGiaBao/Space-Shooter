using System.Collections;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    public float missileSpeed = 10f;  // Tốc độ đạn.
    public float damage = 10f;      // Sát thương đạn gây ra.

    // Update is called once per frame
    void Update()
    {
        // Di chuyển đạn xuống phía dưới.
        transform.Translate(Vector3.down * missileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Va chạm với Player.
        if (collision.CompareTag("Player"))
        {
            // Gây sát thương lên Player.
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // Gọi hiệu ứng nổ từ pool.
            GameObject explosion = ExplosionPool.instance.GetExplosion(transform.position);

            // Trả lại pool sau thời gian hiệu ứng kết thúc.
            StartCoroutine(ReturnExplosionToPool(explosion));

            // Hủy đạn sau va chạm.
            Destroy(gameObject);
        }
    }

    private IEnumerator ReturnExplosionToPool(GameObject explosion)
    {
        yield return new WaitForSeconds(2f); // Thời gian tồn tại của hiệu ứng nổ.
        ExplosionPool.instance.ReturnToPool(explosion);
    }
}
