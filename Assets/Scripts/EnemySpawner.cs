using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 3f;
    public float spawnRadius = 10f;

    private float minSpawnInterval = 0.5f;

    public AudioSource powerupSFX;

    [Header("Enemy Difficulty")]
    public float enemySpeed = 2f;
    public float enemyHealth = 1f;
    public float enemyDamage = 1f;

    [Header("Difficulty Scaling")]
    public float speedIncrease = 0.3f;
    public float healthIncrease = 1f;
    public float spawnRateIncrease = 0.3f;
    public float damageIncrease = 0.3f;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = Random.insideUnitCircle.normalized * spawnRadius;
        int randomEnemy = Random.Range(0, enemyPrefabs.Length);
        GameObject e = Instantiate(enemyPrefabs[randomEnemy], spawnPos, Quaternion.identity);
        Enemy enemy = e.GetComponent<Enemy>();
        enemy.moveSpeed = enemySpeed;
        enemy.health = enemyHealth;
        enemy.damageToPlayer = enemyDamage;
    }

    public void IncreaseDifficulty()
    {
        powerupSFX.Play();
        spawnInterval -= spawnRateIncrease;
        enemySpeed += speedIncrease;
        enemyHealth += healthIncrease;
        enemyDamage += damageIncrease;
    }
}
