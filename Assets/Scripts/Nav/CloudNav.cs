using UnityEngine;
using System.Collections.Generic;


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

    public List<BoxCollider> spawnAreas;

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
        Debug.Log("Cloud Nav: Removing Old Cloud");
        aStar.ResetGraph();
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        Debug.Log("Cloud Nav: Spawn New Cloud");
        int halfXCount = xCount / 2;
        int halfYCount = yCount / 2;
        int halfZCount = zCount / 2;

        for (int x = -halfXCount; x < halfXCount; x++)
        {
            for (int y = -halfYCount; y < halfYCount; y++)
            {
                for (int z = -halfZCount; z < halfZCount; z++)
                {
                    Vector3 targetPoint = new Vector3(x, y, z);

                    bool safe = false;

                    for (int i = 0; i < spawnAreas.Count; i++)
                        if (spawnAreas[i].ClosestPoint(targetPoint) == targetPoint)
                            safe = true;

                    if (safe)
                        aStar.AddPoint(targetPoint);
                }
            }
        }

        aStar.RefreshCashe();

        Debug.Log("Cloud Nav: Connect New Points");
        for (int x = -halfXCount; x < halfXCount; x++)
        {
            for (int y = -halfYCount; y < halfYCount; y++)
            {
                for (int z = -halfZCount; z < halfZCount; z++)
                {
                    int id = aStar.GetPointByPosition(new Vector3(x, y, z));

                    int forward = aStar.GetPointByPosition(new Vector3(x, y, z + 1));

                    int forwardLeft = aStar.GetPointByPosition(new Vector3(x - 1, y, z + 1));
                    int forwardRight = aStar.GetPointByPosition(new Vector3(x + 1, y, z + 1));
                    int left = aStar.GetPointByPosition(new Vector3(x - 1, y, z));
                    int right = aStar.GetPointByPosition(new Vector3(x + 1, y, z));
                    int back = aStar.GetPointByPosition(new Vector3(x, y, z - 1));
                    int backLeft = aStar.GetPointByPosition(new Vector3(x - 1, y, z - 1));
                    int backRight = aStar.GetPointByPosition(new Vector3(x + 1, y, z - 1));

                    int upForward = aStar.GetPointByPosition(new Vector3(x, y + 1, z + 1));
                    int upForwardLeft = aStar.GetPointByPosition(new Vector3(x - 1, y + 1, z + 1));
                    int upForwardRight = aStar.GetPointByPosition(new Vector3(x + 1, y + 1, z + 1));
                    int upLeft = aStar.GetPointByPosition(new Vector3(x - 1, y + 1, z));
                    int up = aStar.GetPointByPosition(new Vector3(x, y + 1, z));
                    int upRight = aStar.GetPointByPosition(new Vector3(x + 1, y + 1, z));
                    int upBack = aStar.GetPointByPosition(new Vector3(x, y + 1, z - 1));
                    int upBackLeft = aStar.GetPointByPosition(new Vector3(x - 1, y + 1, z - 1));
                    int upBackRight = aStar.GetPointByPosition(new Vector3(x + 1, y + 1, z - 1));

                    int downForward = aStar.GetPointByPosition(new Vector3(x, y - 1, z + 1));
                    int downForwardLeft = aStar.GetPointByPosition(new Vector3(x - 1, y - 1, z + 1));
                    int downForwardRight = aStar.GetPointByPosition(new Vector3(x + 1, y - 1, z + 1));
                    int downLeft = aStar.GetPointByPosition(new Vector3(x - 1, y - 1, z));
                    int down = aStar.GetPointByPosition(new Vector3(x, y - 1, z));
                    int downRight = aStar.GetPointByPosition(new Vector3(x + 1, y - 1, z));
                    int downBack = aStar.GetPointByPosition(new Vector3(x, y - 1, z - 1));
                    int downBackLeft = aStar.GetPointByPosition(new Vector3(x - 1, y - 1, z - 1));
                    int downBackRight = aStar.GetPointByPosition(new Vector3(x + 1, y - 1, z - 1));

                    if (forward > -1) aStar.ConnectPoints(id, forward);
                    if (forwardLeft > -1) aStar.ConnectPoints(id, forwardLeft);
                    if (forwardRight > -1) aStar.ConnectPoints(id, forwardRight);
                    if (left > -1) aStar.ConnectPoints(id, left);
                    if (right > -1) aStar.ConnectPoints(id, right);
                    if (back > -1) aStar.ConnectPoints(id, back);
                    if (backLeft > -1) aStar.ConnectPoints(id, backLeft);
                    if (backRight > -1) aStar.ConnectPoints(id, backRight);

                    if (upForward > -1) aStar.ConnectPoints(id, upForward);
                    if (upForwardLeft > -1) aStar.ConnectPoints(id, upForwardLeft);
                    if (upForwardRight > -1) aStar.ConnectPoints(id, upForwardRight);
                    if (upLeft > -1) aStar.ConnectPoints(id, upLeft);
                    if (up > -1) aStar.ConnectPoints(id, up);
                    if (upRight > -1) aStar.ConnectPoints(id, upRight);
                    if (upBack > -1) aStar.ConnectPoints(id, upBack);
                    if (upBackLeft > -1) aStar.ConnectPoints(id, upBackLeft);
                    if (upBackRight > -1) aStar.ConnectPoints(id, upBackRight);

                    if (downForward > -1) aStar.ConnectPoints(id, downForward);
                    if (downForwardLeft > -1) aStar.ConnectPoints(id, downForwardLeft);
                    if (downForwardRight > -1) aStar.ConnectPoints(id, downForwardRight);
                    if (downLeft > -1) aStar.ConnectPoints(id, downLeft);
                    if (down > -1) aStar.ConnectPoints(id, down);
                    if (downRight > -1) aStar.ConnectPoints(id, downRight);
                    if (downBack > -1) aStar.ConnectPoints(id, downBack);
                    if (downBackLeft > -1) aStar.ConnectPoints(id, downBackLeft);
                    if (downBackRight > -1) aStar.ConnectPoints(id, downBackRight);
                }
            }
        }

        /*Debug.Log("Cloud Nav: Validate Points");

        List<Vector3> path = new List<Vector3>();
        for (int x = -halfXCount; x < halfXCount; x++)
        {
            for (int y = -halfYCount; y < halfYCount; y++)
            {
                for (int z = -halfZCount; z < halfZCount; z++)
                {
                    Vector3 targetPosition = new Vector3(x, y, z);

                    if (targetPosition == Vector3.zero)
                        continue;

                    int startId = aStar.GetClosestPoint(targetPosition);
                    int originId = aStar.GetClosestPoint(Vector3.zero);

                    if (startId == -1)
                        continue;

                    //Debug.Log("count " + aStar.Count + " id " + startId + " origin " + originId);

                    path = aStar.GetPath(startId, originId);


                    bool redo = false;

                    if (path.Count == 0)
                    {
                        aStar.RemovePoint(startId);
                    }
                    else
                    {
                        path.Insert(0, targetPosition);

                        for (int i = 0; i < path.Count - 1; i++)
                        {
                            if (Physics.Linecast(path[i], path[i + 1]))
                            {
                                redo = true;
                                aStar.DisconnectPoints(aStar.GetPointByPosition(path[i]), aStar.GetPointByPosition(path[i + 1]));
                                break;
                            }
                        }
                    }

                    if (redo)
                    {
                        z--;
                    }
                }
            }
        }*/

        aStar.RefreshCashe();

        for (int g = 0; g < aStar.Count; g++)
        {
            GameObject pointObject = Instantiate(
                        cloudPointPrefab,
                        aStar.graph[g].position,
                        Quaternion.identity,
                        transform);
        }
    }
}
