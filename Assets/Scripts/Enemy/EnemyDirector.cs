using System.Collections.Generic;
using SuperPupSystems.Helper;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDirector : MonoBehaviour
{
    [Header("SpawnPoints")]
    public Transform bossSpawn;
    public List<Transform> groundSpawnPoints;
    public List<Transform> airSpawnPoints;

    [Header("Prefabs")]
    public GameObject bossPrefab;
    public List<GameObject> enemyPrefabs;
    public List<GameObject> flyingEnemyPrefabs;

    public int targetSpending = 10;
    public int currentSpending = 0;
    public CloudNav cloudNav;
    public UnityEvent bossOutOfHealth;
    public void Spawn()
    {
        if (currentSpending >= targetSpending)
            return;

        // select traversal type
        EnemyTraversalType traversalType = (EnemyTraversalType)Random.Range(0, 2);
        GameObject prefab;

        if (traversalType == EnemyTraversalType.GROUND)
            prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        else //if (traversalType == EnemyTraversalType.FLYING)
            prefab = flyingEnemyPrefabs[Random.Range(0, flyingEnemyPrefabs.Count)];

        // select spawn point
        Transform spawnPoint;

        if (traversalType == EnemyTraversalType.GROUND)
            spawnPoint = groundSpawnPoints[Random.Range(0, groundSpawnPoints.Count)];
        else //if (traversalType == EnemyTraversalType.FLYING)
            spawnPoint = airSpawnPoints[Random.Range(0, airSpawnPoints.Count)];

        // spawn
        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity, transform);
        EnemyInfo info = enemy.GetComponent<EnemyInfo>();
        Health health = enemy.GetComponent<Health>();

        if (traversalType == EnemyTraversalType.FLYING)
        {
            enemy.GetComponent<FlyGun>().cloudNav = cloudNav;
        }


        // spend/connect event
        currentSpending += info.cost;
        health.outOfHealth.AddListener(() => OnEnemyDeath(info.cost, enemy.transform.position, info.drop));
    }

    public void SpawnBoss()
    {
        // spawn
        GameObject enemy = Instantiate(bossPrefab, bossSpawn.position, Quaternion.identity, transform);
        EnemyInfo info = enemy.GetComponent<EnemyInfo>();
        Health health = enemy.GetComponent<Health>();

        if (info.traversalType == EnemyTraversalType.FLYING)
        {
            enemy.GetComponent<FlyGun>().cloudNav = cloudNav;
        }

        // spend/connect event
        currentSpending += info.cost;
        health.outOfHealth.AddListener(() => bossOutOfHealth.Invoke());
    }

    public void OnEnemyDeath(int _refund, Vector3 _position, GameObject _drop)
    {
        currentSpending -= _refund;
        Instantiate(_drop, _position, Quaternion.identity);
    }
}
