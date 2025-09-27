using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class AStar
{
    public int Count => m_graph.Count;
    [SerializeField]
    private List<AStarNode> m_graph;
    private Dictionary<Vector3, int> m_umap = new Dictionary<Vector3, int>();

    // cache
    private List<int> searchingSet = new List<int>();
    private List<int> hasSearchedSet = new List<int>();
    public void ResetGraph()
    {
        m_graph.Clear();
        m_umap.Clear();
    }
    public void RefreshCashe()
    {
        m_umap.Clear();

        for (int i = 0; i < m_graph.Count; i++)
        {
            m_umap.Add(m_graph[i].position, i);
        }
    }
    public int AddPoint(Vector3 _position)
    {
        int id = m_graph.Count;
        m_graph.Add(new AStarNode());
        m_graph[id].position = _position;
        m_umap.Add(_position, id);
        return id;
    }
    public List<Vector3> GetPath(int _idFrom, int _idTo)
    {
        List<Vector3> path = new List<Vector3>();

        if (m_graph.Count <= _idFrom || _idFrom < 0)
        {
            Debug.LogError("AStar::GetPath _idFrom has not been added to the graph.");
            return path;
        }

        if (m_graph.Count <= _idTo || _idTo < 0)
        {
            Debug.LogError("AStar::GetPath _idTo has not been added to the graph.");
            return path;
        }

        {
            int graphSize = m_graph.Count;
            AStarNode node;
            for (int i = 0; i < graphSize; i++)
            {
                node = m_graph[i];
                node.f = 0.0f;
                node.g = 0.0f;
                node.h = 0.0f;
                node.predecessorID = -1;
            }
        }

        searchingSet.Clear();
        hasSearchedSet.Clear();
        int graphNodeIndex;

        searchingSet.Add(_idFrom);

        while (searchingSet.Count > 0)
        {
            int lowestPath = 0;

            for (int i = 0; i < searchingSet.Count; i++)
            {
                if (m_graph[searchingSet[lowestPath]].f > m_graph[searchingSet[i]].f)
                {
                    lowestPath = i;
                }
            }

            AStarNode node = m_graph[searchingSet[lowestPath]];
            graphNodeIndex = searchingSet[lowestPath];

            if (_idTo == searchingSet[lowestPath])
            {
                return BuildPath(node);
            }

            hasSearchedSet.Add(searchingSet[lowestPath]);

            searchingSet.RemoveAt(lowestPath);

            List<int> neighborIDs = node.adjacentPointIDs;

            for (int i = 0; i < neighborIDs.Count; i++)
            {
                AStarNode neighborNode = m_graph[neighborIDs[i]];

                if (hasSearchedSet.Contains(neighborIDs[i]) == false)
                {
                    if (graphNodeIndex == neighborIDs[i])
                    {
                        continue;
                    }

                    float currentG = node.g + Vector3.Distance(node.position, neighborNode.position);

                    bool isNewPath = false;

                    if (searchingSet.Contains(neighborIDs[i]))
                    {
                        if (currentG < neighborNode.g)
                        {
                            isNewPath = true;
                            neighborNode.g = currentG;
                        }
                    }
                    else
                    {
                        isNewPath = true;
                        neighborNode.g = currentG;
                        searchingSet.Add(neighborIDs[i]);
                    }

                    if (isNewPath)
                    {
                        neighborNode.h = Vector3.Distance(neighborNode.position, m_graph[_idTo].position);
                        neighborNode.f = neighborNode.g + neighborNode.h;
                        neighborNode.predecessorID = graphNodeIndex;
                    }
                }
            }
        }

        return path;
    }
    public int GetClosestPoint(Vector3 _position)
    {
        if (m_graph.Count == 0)
        {
            Debug.LogError("AStar::GetClosetPoint was called before a point was added to the graph.");
            return -1;
        }

        int id = 0;
        float distance;
        float minDistance = float.MaxValue;

        for (int i = 1; i < m_graph.Count; i++)
        {
            distance = Vector3.Distance(_position, m_graph[i].position);

            if (minDistance > distance)
            {
                id = i;
                minDistance = distance;
            }
        }

        return id;
    }
    public int GetPointByPosition(Vector3 _position)
    {
        if (m_umap.ContainsKey(_position))
            return m_umap[_position];
        else
            return -1;
    }
    public void RemovePoint(int _id)
    {
        if (m_graph.Count <= _id && _id < 0)
        {
            Debug.LogError("AStar::RemovePoint id has not been added to the graph.");
            return;
        }

        Vector3 position = m_graph[_id].position;

        // unconnect
        for (int i = 0; i < m_graph[_id].adjacentPointIDs.Count; i++)
        {
            int adjacentPointID = m_graph[_id].adjacentPointIDs[i];
            m_graph[adjacentPointID].adjacentPointIDs.Remove(_id);
        }

        m_graph.RemoveAt(_id);
        m_umap.Remove(position);
    }
    public void ConnectPoints(int _idFrom, int _idTo)
    {
        if (m_graph.Count <= _idFrom || _idFrom < 0)
        {
            Debug.LogError("AStar::ConnectPoints _idFrom has not been added to the graph.");
            return;
        }

        if (m_graph.Count <= _idTo || _idTo < 0)
        {
            Debug.LogError("AStar::ConnectPoints _idTo has not been added to the graph.");
            return;
        }

        if (_idFrom == _idTo)
        {
            Debug.LogError("AStar::ConnectPoints _idFrom is _idTo.");
            return;
        }

        if (m_graph[_idFrom].adjacentPointIDs.Contains(_idTo) == false)
            m_graph[_idFrom].adjacentPointIDs.Add(_idTo);

        if (m_graph[_idTo].adjacentPointIDs.Contains(_idFrom) == false)
            m_graph[_idTo].adjacentPointIDs.Add(_idFrom);
    }
    public bool ArePointsConnected(int _idFrom, int _idTo)
    {
        if (m_graph.Count <= _idFrom || _idFrom < 0)
        {
            Debug.LogError("AStar::ArePointsConnected _idFrom has not been added to the graph.");
            return false;
        }

        if (m_graph.Count <= _idTo || _idTo < 0)
        {
            Debug.LogError("AStar::ArePointsConnected _idTo has not been added to the graph.");
            return false;
        }

        for (int i = 0; i < m_graph[_idFrom].adjacentPointIDs.Count; i++)
        {
            if (m_graph[_idFrom].adjacentPointIDs[i] == _idTo)
            {
                return true;
            }
        }

        return false;
    }
    public bool ValidPoint(Vector3 _position)
    {
        return m_umap.ContainsKey(_position);
    }
    private List<Vector3> BuildPath(AStarNode _node)
    {
        List<Vector3> path = new List<Vector3>();

        AStarNode currentNode = _node;

        while (currentNode.predecessorID != -1)
        {
            path.Insert(0, currentNode.position);
            currentNode = m_graph[currentNode.predecessorID];
        }

        return path;
    }
}
