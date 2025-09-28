using UnityEngine;

public class EnemyMap : MonoBehaviour
{
    public float scale;
    public Color color;
    public bool added;

    void Start()
    {
        MiniMap.Instance.RegisterEnemy(this);
    }
    public void DeleteBlip()
    {
        if (!added)
        {
            MiniMap.Instance.RegisterEnemy(this);
            added = true;
        }
        if (MiniMap.Instance != null)
            MiniMap.Instance.UnregisterEnemy(this);
    }
}
