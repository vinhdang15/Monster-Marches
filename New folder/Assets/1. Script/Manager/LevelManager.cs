using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject mapPref;
    private GameObject map;
    [SerializeField] private BulletPool bulletPoolPref;
    [SerializeField] private UnitPool unitPoolPref;
    [SerializeField] private EnemySpawnerManager spawnEnemyManagerPref;
    private EnemySpawnerManager spawnEnemyManager;
    [SerializeField] private List<EnemySpawner> spawnEnemiesPref = new List<EnemySpawner>();
    [SerializeField] public List<EnemySpawner> spawnEnemies = new List<EnemySpawner>();

    public PolygonCollider2D GetMapPolygonCollider2D()
    {
        return map.GetComponent<PolygonCollider2D>();
    }

    public void BindMap(GameObject tranformParent)
    {
        map = Instantiate(mapPref, tranformParent.transform);
        map.name = mapPref.ToString();
    }

    public void BindPoolObject(GameObject tranformParent)
    {
        BulletPool bulletP = Instantiate(bulletPoolPref, tranformParent.transform);
        bulletP.name = InitNameObject.BulletPool.ToString();

        UnitPool unitP = Instantiate(unitPoolPref, tranformParent.transform);
        unitP.name = InitNameObject.UnitPool.ToString();
    }

    public void BindSpawnEnemyManager(GameObject tranformParent)
    {
        spawnEnemyManager = Instantiate(spawnEnemyManagerPref, tranformParent.transform);
        spawnEnemyManager.name = InitNameObject.EnemySpawnerManager.ToString();
    }

    public void SpawnEnemyManagerPrepareGame()
    {
        spawnEnemyManager.PrepareGame();
    }

    public void BindSpawnEnemiesObject(GameObject tranformParent)
    {
        foreach(var SpawnEnemy in spawnEnemiesPref)
        {
            Instantiate(SpawnEnemy, tranformParent.transform);
        }
    }

    public void SpawnEnemiesPrepareGame()
    {
        foreach(var spawnEnemy in spawnEnemyManager.SpawnEnemies)
        {
            spawnEnemy.PrepareGame();
        }
    }
}
