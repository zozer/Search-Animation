using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTree : MonoBehaviour
{
    /// <summary>
    /// For testing purpose only
    /// </summary>
    public void CreateTest()
    {
        MapNode start = gameObject.AddComponent<MapNode>();
        MapNode nodez = gameObject.AddComponent<MapNode>();
        MapNode nodea = gameObject.AddComponent<MapNode>();
        MapNode nodex = gameObject.AddComponent<MapNode>();
        MapNode noded = gameObject.AddComponent<MapNode>();
        MapNode nodec = gameObject.AddComponent<MapNode>();
        MapNode nodef = gameObject.AddComponent<MapNode>();
        MapNode goal = gameObject.AddComponent<MapNode>();

        start.data = "s";
        nodez.data = "z";
        nodea.data = "a";
        nodex.data = "x";
        noded.data = "d";
        nodec.data = "c";
        nodef.data = "f";
        goal.data = "g";

        start.Connections.Add(nodea);
        start.Connections.Add(nodex);

        nodea.Connections.Add(start);
        nodea.Connections.Add(nodez);

        nodez.Connections.Add(nodea);

        nodex.Connections.Add(start);
        nodex.Connections.Add(noded);
        nodex.Connections.Add(nodec);

        noded.Connections.Add(nodex);
        noded.Connections.Add(nodec);
        noded.Connections.Add(nodef);

        nodec.Connections.Add(noded);
        nodec.Connections.Add(nodef);
        nodec.Connections.Add(nodex);
        nodec.Connections.Add(goal);

        nodef.Connections.Add(noded);
        nodef.Connections.Add(nodec);
        nodef.Connections.Add(goal);

        goal.Connections.Add(nodef);
        goal.Connections.Add(nodec);

        List<string> history = new List<string>();

        TraversingBM(start, goal, start, history);
    }

    /// <summary>
    /// Traversal the map in a way that creating British Museum
    /// </summary>
    /// <param name="start">A starting node of the map</param>
    /// <param name="goal">A destination node of the map</param>
    /// <param name="current">A tracking node of the current traversal progress.(It must be 
    /// the same as the starting node at the beginning)</param>
    /// <param name="history">history nodes that had visited in the path</param>
    private void TraversingBM(MapNode start, MapNode goal, MapNode current, List<string> history)
    {
        Debug.Log(current.data);

        if (current.data == goal.data)
        {
            return;
        }

        int num = current.Connections.Count;
        string startData = start.data;
        string currentData = current.data;

        for (int i = 0; i < num; i++)
        {
            history.Add(currentData);
            string nextData = current.Connections[i].data;

            if (currentData == startData || nextData != startData &&
                history.Contains(nextData) == false)
            {
                Debug.Log(currentData + " to " + nextData);
                TraversingBM(start, goal, current.Connections[i], history);
            }

            history.Remove(currentData);
        }
    }
}
