using UnityEngine;
using System.Collections;
public class EnemySpawneer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawns;
    [SerializeField] private DifficultyManager difficultyManager;

    [Header("Values")]
    [SerializeField] private int baseEnemiesPerWave;
    [SerializeField] private float baseHealth;
    [SerializeField] private float baseSpeed = 3f;

    private void Start()
    {
        StartCoroutine(VerificarEnemigos());
    }
    public void GenerarOleada()
    {
        int enemiesToSpawn = baseEnemiesPerWave;
        float enemiesHealth = baseHealth;
        float enemiesSpeed = baseSpeed;

        switch (difficultyManager.currentDifficulty)
        {
            case DifiicultyLevel.Easy:
                enemiesToSpawn = 3;
                enemiesHealth = 80f;
                enemiesHealth = 3.5f;
                break;
            case DifiicultyLevel.Medium:
                enemiesToSpawn = 5;
                enemiesHealth = 100;
                enemiesHealth = 4.5f;
                break;
            case DifiicultyLevel.Hard:
                enemiesToSpawn = 10;
                enemiesHealth = 150f;
                enemiesHealth = 6f;
                break;
        }
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Transform spawnPoint = spawns[Random.Range(0, spawns.Length)];

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            AdaptativeCoordEnemyController acec = enemy.GetComponent<AdaptativeCoordEnemyController>();

            if (acec != null)
            {
                acec.enemyIndex = i;
                acec.totalEnemies = enemiesToSpawn;
                acec.speed = enemiesSpeed;
                float randomValue = Random.value;
                if (randomValue < difficultyManager.tacticChance)
                {
                    acec.SetState(2);
                }
                else
                {
                    acec.SetState(0);
                }
            }
        }
        Debug.Log($"Oleada generada: {enemiesToSpawn} enemigos - Difficultad: {difficultyManager.currentDifficulty}");
    }
    IEnumerator VerificarEnemigos()
    {
        while (true)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                GenerarOleada();
            }
            yield return new WaitForSeconds(2f);
        }
    }
}

