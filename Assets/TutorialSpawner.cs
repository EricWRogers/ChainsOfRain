using UnityEngine;
using System.Collections.Generic;
public class TutorialSpawner : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    public Transform spawnpoint;

    public bool positive;
    public void Spawn()
    {
        int rand = Random.Range(0, enemies.Count);
        GameObject obj = Instantiate(enemies[rand], spawnpoint.position, spawnpoint.rotation);
        obj.GetComponent<Carosel>().positive = positive;
    }
}

