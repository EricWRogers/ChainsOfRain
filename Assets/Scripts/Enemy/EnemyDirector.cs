
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
    [Tooltip(" Adds all Drop chances together then selects a random range from that value")]
    public List<drop> drops;
    private int m_totalPrecent = 0;

    public int targetSpending = 10;
    public int currentSpending = 0;
    public CloudNav cloudNav;
    public UnityEvent bossOutOfHealth;


    public void Start()
    {
        foreach(drop _drop in drops)
        {
            m_totalPrecent += _drop.dropChance;
        }
    }
    public void Spawn()
    {
        if (currentSpending >= targetSpending)
            return;

        // select traversal type
        EnemyTraversalType traversalType = (EnemyTraversalType)Random.Range(0, 2);
        GameObject prefab;

        if (traversalType == EnemyTraversalType.GROUND && groundSpawnPoints.Count > 0)
            prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        else //if (traversalType == EnemyTraversalType.FLYING)
            prefab = flyingEnemyPrefabs[Random.Range(0, flyingEnemyPrefabs.Count)];

        // select spawn point
        Transform spawnPoint;

        if (traversalType == EnemyTraversalType.GROUND && groundSpawnPoints.Count > 0)
            spawnPoint = groundSpawnPoints[Random.Range(0, groundSpawnPoints.Count)];
        else //if (traversalType == EnemyTraversalType.FLYING)
        {
            spawnPoint = airSpawnPoints[Random.Range(0, airSpawnPoints.Count)];
            traversalType = EnemyTraversalType.FLYING;

        }

        // spawn
        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity, transform);
        EnemyInfo info = enemy.GetComponent<EnemyInfo>();
        Health health = enemy.GetComponent<Health>();

        if (traversalType == EnemyTraversalType.FLYING)
        {
            enemy.GetComponent<FlyGun>().cloudNav = cloudNav;
        }


        //Randomised Drops
        int randNum = Random.Range(0, m_totalPrecent);
        int currentNum = 0;
        if(drops.Count >0)
        {
            foreach (drop _drop in drops)
            {
                currentNum += _drop.dropChance;
                if (randNum <= currentNum)
                {
                    info.drop = _drop.prefab;
                    break;
                }
            }
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

    [System.Serializable]
    public struct drop
    {
        public GameObject prefab;

        public int dropChance;


    }
}
