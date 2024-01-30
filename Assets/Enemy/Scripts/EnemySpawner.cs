using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount;
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Keep track of spawned enemies

    // Coroutine for enemy spawning
    public IEnumerator EnemySpawn(Transform enemyTransform)
    {
        // Spawn up to 4 enemies
        while (enemyCount < 4)
        {
            // Calculate a random spawn offset within a range
            Vector3 spawnOffset = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-5f, 5f));
            
            // Calculate the spawn position in front of the enemy with random distance
            Vector3 spawnPosition = enemyTransform.position + enemyTransform.forward * Random.Range(10f, 20f) + spawnOffset;
            
            // Instantiate an enemy at the calculated position and add it to the list
            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(spawnedEnemy);
            
            // Add a 2-second delay before spawning the next enemy
            yield return new WaitForSeconds(4.0f);
            
            // Increase the enemy count
            enemyCount += 1;
        }
    }

    // Method to destroy all spawned enemies
    public void DestroySpawnedEnemies()
    {
        foreach (var enemy in spawnedEnemies)
        {
            Destroy(enemy);
        }
        spawnedEnemies.Clear(); // Clear the list after destroying the enemies
    }
}
