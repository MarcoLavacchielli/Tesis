using UnityEngine;

public class GameManager : MonoBehaviour
{
    public EnemyFactory enemyFactoryPrefab;
    public GameObject playerCheckerPrefab;

    private IEnemyFactory enemyFactoryInstance;

    private void Start()
    {
        // Establece la tasa de fotogramas deseada en 60 FPS
        Application.targetFrameRate = 60;
    }

    public void SetEnemyFactory(string enemyType)
    {
        if (enemyFactoryPrefab == null)
        {
            Debug.LogError("EnemyFactoryPrefab is null in GameManager");
            return;
        }

        enemyFactoryInstance = enemyFactoryPrefab.GetEnemyFactory(enemyType);
    }

    public void SpawnEnemy(Vector3 spawnPosition)
    {
        if (enemyFactoryInstance != null)
        {
            GameObject spawnedEnemy = enemyFactoryInstance.SpawnEnemy(spawnPosition);

            if (spawnedEnemy != null)
            {
                Debug.Log("Enemy spawned at position: " + spawnPosition);
            }
            else
            {
                Debug.LogError("Error spawning enemy");
            }
        }
        else
        {
            Debug.LogError("EnemyFactoryInstance is null in GameManager");
        }
    }
}
