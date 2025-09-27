using UnityEngine;

public class EnemyMap : MonoBehaviour
{
    public float scale;
    public Color color;

    void Start()
    {
        MiniMap.Instance.RegisterEnemy(this);
    }
}
