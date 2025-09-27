using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CloudNavTestDummy : MonoBehaviour
{
    public CloudNav cloudNav;
    public List<Vector3> path;
    public GameObject start;
    public GameObject end;
    public int startId;
    public int endId;
    public int targetIndex;
    public float speed = 100.0f;

    void Start()
    {
        GetNewPath();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path.Count == 0)
            return;

        if (transform.position == path[targetIndex])
        {
            targetIndex++;

            if (targetIndex >= path.Count)
            {
                GetNewPath();
            }
        }

        Vector3 direction = (path[targetIndex] - transform.position).normalized;
        Vector3 movePosition = transform.position + (direction * speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, path[targetIndex]) < Vector3.Distance(transform.position, movePosition))
        {
            transform.position = path[targetIndex];
            return;
        }

        transform.position = movePosition;
    }

    void GetNewPath()
    {
        path.Clear();

        targetIndex = 0;

        startId = cloudNav.aStar.GetClosestPoint(transform.position);
        endId = cloudNav.aStar.GetClosestPoint(new Vector3(
            Random.Range(cloudNav.xCount/-2.0f, cloudNav.xCount/2.0f),
            Random.Range(cloudNav.yCount/-2.0f, cloudNav.yCount/2.0f),
            Random.Range(cloudNav.zCount/-2.0f, cloudNav.zCount/2.0f)
            ));

        path = cloudNav.aStar.GetPath(startId, endId);

    }
}
