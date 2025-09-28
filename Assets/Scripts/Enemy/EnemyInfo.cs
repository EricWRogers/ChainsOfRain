using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public EnemyTraversalType traversalType = EnemyTraversalType.GROUND;
    public int cost = 1;
}

public enum EnemyTraversalType
{
    GROUND = 0,
    FLYING = 1
}
