using UnityEngine;

public interface IEnemyFactory
{
    GameObject SpawnEnemy(Vector3 spawnPosition);
}
public class EnemyFactory : MonoBehaviour
{
    public EnemyType1Factory enemyType1Factory;
    public EnemyType2Factory enemyType2Factory;
    public EnemyType3Factory enemyType3Factory;

    public IEnemyFactory GetEnemyFactory(string enemyType)
    {
        switch (enemyType)
        {
            case "Type1":
                return enemyType1Factory;
            case "Type2":
                return enemyType2Factory;
            case "Type3":
                return enemyType3Factory;
            default:
                return null;
        }
    }
}