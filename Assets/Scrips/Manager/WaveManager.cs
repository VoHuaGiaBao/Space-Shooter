using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [System.Serializable]
    public class Wave
    {
        public string waveName;           // Tên wave.
        public GameObject[] enemyPrefabs; // Danh sách các loại kẻ địch trong wave.
        public int enemyCount;            // Số lượng kẻ địch trong wave.
        public float spawnRate;           // Tốc độ spawn (kẻ/giây).
        public bool isBossWave;           // Đánh dấu nếu đây là wave boss.
    }

    public List<Wave> waves;              // Danh sách các wave.
    public GameObject bossPrefab;         // Prefab của boss.

    public float timeBetweenWaves = 5f;   // Thời gian chờ giữa các wave.

    private int currentWaveIndex = 0;     // Wave hiện tại.
    private int enemiesRemainingToSpawn;  // Số lượng kẻ địch cần spawn.
    private int enemiesRemainingAlive;    // Số lượng kẻ địch còn sống.

    private bool isSpawningWave = false;  // Kiểm tra nếu đang spawn wave.

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void StartWaves()
    {
        StartCoroutine(StartWave(currentWaveIndex));
    }

    private IEnumerator StartWave(int waveIndex)
    {
        if (waveIndex >= waves.Count)
        {
            Debug.Log("Tất cả các wave đã hoàn thành!");
            GameManager.instance.GameWon(); // Gọi GameManager khi hoàn thành game.
            yield break;
        }

        Debug.Log($"Bắt đầu wave {waves[waveIndex].waveName}...");
        Wave wave = waves[waveIndex];
        enemiesRemainingToSpawn = wave.enemyCount;
        enemiesRemainingAlive = wave.enemyCount;

        isSpawningWave = true;

        // Spawn từng kẻ địch trong wave
        yield return StartCoroutine(SpawnWave(wave));

        isSpawningWave = false;

        // Chờ kẻ địch bị tiêu diệt hết trước khi bắt đầu wave mới
        while (enemiesRemainingAlive > 0)
        {
            yield return null;
        }

        // Thời gian chờ giữa các wave
        Debug.Log($"Wave {wave.waveName} hoàn thành! Chờ {timeBetweenWaves} giây để qua wave tiếp theo...");
        yield return new WaitForSeconds(timeBetweenWaves);

        currentWaveIndex++;
        StartCoroutine(StartWave(currentWaveIndex));
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        while (enemiesRemainingToSpawn > 0)
        {
            SpawnEnemy(wave);
            enemiesRemainingToSpawn--;
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }
    }

    private void SpawnEnemy(Wave wave)
    {
        float minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x; // Biên trái
        float maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x; // Biên phải

        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, 6f, 0); // Tọa độ spawn

        GameObject enemyPrefab = wave.isBossWave ? bossPrefab : wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    public void OnEnemyKilled(GameObject enemy)
    {
        enemiesRemainingAlive--;

        // Nếu là boss wave, chỉ hoàn thành wave khi boss bị tiêu diệt
        if (waves[currentWaveIndex].isBossWave && enemy.CompareTag("Boss"))
        {
            Debug.Log("Boss defeated! Wave completed.");
        }

        Debug.Log($"Kẻ địch còn lại: {enemiesRemainingAlive}");
    }
}
