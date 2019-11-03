using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTree : MonoBehaviour
{
    //sample testing
    public void createTest()
    {
        MapNode start = new MapNode();
        MapNode nodez = new MapNode();
        MapNode nodea = new MapNode();
        MapNode nodex = new MapNode();
        MapNode noded = new MapNode();
        MapNode nodec = new MapNode();
        MapNode nodef = new MapNode();
        MapNode goal = new MapNode();

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
        nodec.Connections.Add(goal);

        nodef.Connections.Add(noded);
        nodef.Connections.Add(nodec);
        nodef.Connections.Add(goal);

        goal.Connections.Add(nodef);
        goal.Connections.Add(nodec);

        quickSort(start.Connections, 0, start.Connections.Count - 1);
        quickSort(nodea.Connections, 0, nodea.Connections.Count - 1);
        quickSort(nodez.Connections, 0, nodez.Connections.Count - 1);
        quickSort(nodex.Connections, 0, nodex.Connections.Count - 1);
        quickSort(nodec.Connections, 0, nodec.Connections.Count - 1);
        quickSort(nodef.Connections, 0, nodef.Connections.Count - 1);
        quickSort(noded.Connections, 0, noded.Connections.Count - 1);
        quickSort(goal.Connections, 0, goal.Connections.Count - 1);

        List<string> history = new List<string>();

        makeTree(start, goal, start, history);
    }

    //input:
    //start - starting node
    //goal - goal node
    //current - to track the current node - It must be same as the start at the beginning
    //history - history of nodes that had visited
    private void makeTree(MapNode start, MapNode goal, MapNode current, List<string> history)
    {
        Debug.Log(current.data);

        if(current.data == goal.data)
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
                makeTree(start, goal, current.Connections[i], history);
            }
            
            history.Remove(currentData);
        }
    }

    public static void swapNodes(List<MapNode> nodes, int index1, int index2)
    {
        MapNode swap = nodes[index1];
        nodes[index1] = nodes[index2];
        nodes[index2] = swap;
    }

    public static int partition(List<MapNode> nodes, int low, int high)
    {
        string pivot = nodes[high].data;

        int i = (low - 1);

        for(int j = low; j < high; j++)
        {
            if(string.Compare(nodes[j].data, pivot) == -1){

                i++;

                swapNodes(nodes, i, j);
            }
        }

        swapNodes(nodes, i+1, high);

        return (i + 1);
    }
    
    public static void quickSort(List<MapNode> nodes, int low, int high)
    {
        if(low < high)
        {
            int partIndex = partition(nodes, low, high);

            quickSort(nodes, low, partIndex - 1);
            quickSort(nodes, partIndex + 1, high);
        }
    }
}
