using System.Collections.Generic;
using UnityEngine;

public class ExplosionPool : MonoBehaviour
{
    public static ExplosionPool instance;

    [Header("Explosion Prefab")]
    public GameObject explosionPrefab; // Prefab của hiệu ứng nổ

    private Queue<GameObject> explosionPool = new Queue<GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        InitializePool();
    }

    private void InitializePool()
    {
        if (explosionPrefab == null)
        {
            Debug.LogError("Explosion Prefab is not assigned in the ExplosionPool.");
            return;
        }

        // Tạo pool cho explosion
        for (int i = 0; i < 10; i++)
        {
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.SetActive(false);
            explosionPool.Enqueue(explosion);
        }
    }

    public GameObject GetExplosion(Vector3 position)
    {
        if (explosionPool.Count > 0)
        {
            GameObject explosion = explosionPool.Dequeue();
            explosion.SetActive(true);
            explosion.transform.position = position;
            return explosion;
        }
        else
        {
            GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            return explosion;
        }
    }

    public void ReturnToPool(GameObject explosion)
    {
        explosion.SetActive(false);
        explosionPool.Enqueue(explosion);
    }
}
