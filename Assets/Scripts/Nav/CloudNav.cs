using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CloudNav))]
public class CloudNavEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CloudNav cloudNav = (CloudNav)target;

        if (GUILayout.Button("Spawn Cloud"))
        {
            cloudNav.SpawnCloud();
        }
    }
}
#endif

public class CloudNav : MonoBehaviour
{
    public GameObject cloudPointPrefab;
    public AStar aStar;

    public int xCount = 50;
    public int yCount = 10;
    public int zCount = 50;

    void Awake()
    {
        aStar.RefreshCashe();
    }

    void Start()
    {
        Debug.Log("AStar graph Count: " + aStar.Count);
    }

    public void SpawnCloud()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        int halfXCount = xCount / 2;
        int halfYCount = yCount / 2;
        int halfZCount = zCount / 2;
        for (int x = -halfXCount; x < halfXCount; x++)
        {
            for (int y = -halfYCount; y < halfYCount; y++)
            {
                for (int z = -halfZCount; z < halfZCount; z++)
                {
                    GameObject pointObject = Instantiate(
                        cloudPointPrefab,
                        new Vector3(x, y, z),
                        Quaternion.identity,
                        transform);

                    aStar.AddPoint(pointObject.transform.position);
                }
            }
        }
    }
}
