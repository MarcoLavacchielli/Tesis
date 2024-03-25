using UnityEngine;

public class PlayerChecker : MonoBehaviour
{
    [SerializeField]
    private Vector3 spawnPosition = new Vector3(1f, 0f, 0f);

    [SerializeField] private string enemyTypeToSpawn = "Type1";
    [SerializeField] private GameManager gameManager;

    private void Initialize(GameManager manager)
    {
        gameManager = manager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent("PlayerController"))
        {
            Vector3 spawnPosition = GetSpawnPosition();
            Debug.Log("Player detected. Enemy type to spawn: " + enemyTypeToSpawn);
            gameManager.SetEnemyFactory(enemyTypeToSpawn);
            gameManager.SpawnEnemy(spawnPosition);
            Destroy(gameObject);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}