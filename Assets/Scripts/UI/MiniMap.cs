using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public static MiniMap Instance { get; private set; }
    public Transform playerOnScene;
    public RectTransform playerOnMap;
    public float scale;
    public GameObject blip;
    public bool relativeDirection;
    private List<Transform> enemies = new();
    private List<RectTransform> enemiesOnMap = new();

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            AlignPosition(i);
        }
    }

    public void RegisterEnemy(EnemyMap _enemy)
    {
        enemies.Add(_enemy.gameObject.transform);
        GameObject temp = Instantiate(blip, playerOnMap);
        temp.GetComponent<Image>().color = _enemy.color;
        temp.transform.localScale = Vector3.one * _enemy.scale; 
        enemiesOnMap.Add(temp.GetComponent<RectTransform>());
    }

    public void UnregisterEnemy(EnemyMap _enemy)
    {
        int index = enemies.IndexOf(_enemy.transform);
        if (index >= 0)
        {
            if (enemiesOnMap[index] != null)
                Destroy(enemiesOnMap[index].gameObject);

            enemies.RemoveAt(index);
            enemiesOnMap.RemoveAt(index);
        }
    }

    void AlignPosition(int i)
    {
        Transform enemy = enemies[i];
        RectTransform indicator = enemiesOnMap[i];
        if (enemy != null && indicator != null)
        {
            Vector3 relativePos;
            if (relativeDirection)
                relativePos = playerOnScene.InverseTransformPoint(enemy.position);
            else
                relativePos = enemy.position - playerOnScene.position;
            indicator.anchoredPosition = new Vector2(relativePos.x, relativePos.z) * scale;
        }
    }
}
