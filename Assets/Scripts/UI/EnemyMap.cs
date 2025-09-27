using UnityEngine;

public class EnemyMap : MonoBehaviour
{
    public float scale;
    public Color color;

    void Start()
    {
        MiniMap.Instance.RegisterEnemy(this);
    }
    public void DeleteBlip()
    {
        if (MiniMap.Instance != null)
            MiniMap.Instance.UnregisterEnemy(this);
    }
}
