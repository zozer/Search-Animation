using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTree : MonoBehaviour
{
    public TreeNode root;
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

        start.Connections.Add(nodex);
        start.Connections.Add(nodea);

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

        root = gameObject.AddComponent<TreeNode>();
        root.data = start.data;

        BuildBM(start, goal, start, history, root);

        preorder(root);
    }

    /// <summary>
    /// test function that check if the node is there in the tree
    /// </summary>
    /// <param name="root"></param>
    public static void preorder(TreeNode root)
    {
        //Debug.Log(root.data);
        for (int i = 0; i < root.Children.Count; i++)
        {
            preorder(root.Children[i]);
        }
    }

    /// <summary>
    /// sorting the list
    /// </summary>
    /// <param name="children"></param>
    public static void order(List<MapNode> children)
    {
        children.Sort((child1, child2) => child1.data.CompareTo(child2.data));
    }

    /// <summary>
    /// Traversing the map and creating tree for the British Museum
    /// </summary>
    /// <param name="start">A starting node of the map</param>
    /// <param name="goal">A destination node of the map</param>
    /// <param name="current">A tracking node of the current traversal progress.
    /// (It must be the same as the starting node at the beginning)</param>
    /// <param name="history">history nodes that had visited in the path</param>
    /// <param name="root">root of the tree that we want to build</param>
    private void BuildBM(MapNode start, MapNode goal, MapNode current, List<string> history, TreeNode root)
    {
        //check if the node is leaf or goal
        if (current .Connections.Count == 0 || current.data == goal.data)
        {
            return;
        }

        //sort the list to follow alphabetical order
        if(current.Connections.Count > 1)
        {
            order(current.Connections);
        }

        int num = current.Connections.Count;
        string startData = start.data;
        string currentData = current.data;

        for (int i = 0; i < num; i++)
        {
            //create new node for the tree
            TreeNode child = new GameObject("" + current.Connections[i].data, typeof(TreeNode)).GetComponent<TreeNode>();
            child.data = current.Connections[i].data;

            history.Add(currentData);

            string nextData = current.Connections[i].data;

            //check if the node is visted or it is a start node
            if (currentData == startData || nextData != startData &&
                history.Contains(nextData) == false)
            {
                child.Parent = root;
                root.AddChild(child);

                BuildBM(start, goal, current.Connections[i], history, child);
            }
            else
            {
                Destroy(child.gameObject);
            }
            history.Remove(currentData);
        }
    }
}
