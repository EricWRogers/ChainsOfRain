using System.Collections.Generic;
using UnityEngine;

public class CloudNavTestDummy : MonoBehaviour
{
    public CloudNav cloudNav;
    public List<Vector3> path;
    public GameObject start;
    public GameObject end;
    public int startId;
    public int endId;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startId = cloudNav.aStar.GetPointByPosition(start.transform.position);
        endId = cloudNav.aStar.GetPointByPosition(end.transform.position);

        path = cloudNav.aStar.GetPath(startId, endId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
