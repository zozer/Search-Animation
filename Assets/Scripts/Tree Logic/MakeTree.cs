using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeTree : MonoBehaviour
{
    [HideInInspector]
    public TreeNode rootBM;
    public GameObject mapNode;
    public GameObject treeNode;
    public GameObject linePrefab;
    public List<List<string>> DFSearch(char end)
    {
        //add start node as the first line
        List<List<string>> procedure = new List<List<string>>() { new List<string> { $"{rootBM.Data}" } };

        //perform the search until either the the next item to be searched is a goal node or the are no more nodes to check
        while (procedure.Last().Count > 0 && procedure.Last().First().Last() != end)
        {
            string branch = procedure.Last().First();
            TreeNode current = FindNodeByPath(branch);
            List<string> newProcedure = current.Children.Select(e => branch + e.Data).ToList();
            newProcedure.AddRange(procedure.Last().Skip(1));
            procedure.Add(newProcedure);
        }
        return procedure;
    }

    public List<List<string>> BFSearch(char end)
    {
        //add start node as the first line
        List<List<string>> procedure = new List<List<string>>() { new List<string> { $"{rootBM.Data}" } };

        //perform the search until either the the next item to be searched is a goal node or the are no more nodes to check
        while (procedure.Last().Count > 0 && procedure.Last().First().Last() != end)
        {
            string branch = procedure.Last().First();
            List<string> newProcedure = procedure.Last().Skip(1).ToList();
            TreeNode current = FindNodeByPath(branch);
            newProcedure.AddRange(current.Children.Select(e => branch + e.Data));
            procedure.Add(newProcedure);
        }
        return procedure;
    }
    /// <summary>
    /// For testing purpose only
    /// </summary>
    public void CreateTest()
    {
        //sample nodes
        MapNode start = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode nodez = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode nodea = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode nodex = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode noded = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode nodec = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode nodef = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode goal = Instantiate(mapNode).GetComponent<MapNode>();

        //enter data for nodes
        start.Data = 's';
        nodez.Data = 'z';
        nodea.Data = 'a';
        nodex.Data = 'x';
        noded.Data = 'd';
        nodec.Data = 'c';
        nodef.Data = 'f';
        goal.Data = 'g';

        //add links to each node
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

        //new gameObject for BM tree
        rootBM = Instantiate(treeNode, transform).GetComponent<TreeNode>();
        rootBM.Data = start.Data;

        //build BM tree
        BuildBM(start, goal, start, new List<char>(), rootBM);

        /*List<List<string>> procedureDF = BuildBF(rootBM, goal.Data);
        for (int i = 0; i < procedureDF.Count; i++) {
            for(int j = 0; j < procedureDF[i].Count; j++)
            {
                Debug.Log(procedureDF[i][j]);
            }
        }*/
    }

    public void BuildBM(MapNode start, MapNode goal, MapNode current, List<char> history, TreeNode root)
    {
        BuildBM(start, goal, current, history, root, treeNode);
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
    public static void BuildBM(MapNode start, MapNode goal, MapNode current, List<char> history, TreeNode root, GameObject treeNode)
    {
        //check if the node is leaf or goal
        if (current.Connections.Count == 0 || current.Data == goal.Data)
        {
            return;
        }

        //sort the list to follow alphabetical order
        if(current.Connections.Count > 1)
        {
            Order(current.Connections);
        }

        //initilizing variables
        char startData = start.Data;
        char currentData = current.Data;

        //travsering nodes
        foreach (MapNode connection in current.Connections)
        {
            //create new node for the tree
            history.Add(currentData);
            char nextData = connection.Data;

            //check if the node is visted or it is a start node
            if (currentData == startData || nextData != startData &&
                !history.Contains(nextData))
            {
                TreeNode child = Instantiate(treeNode).GetComponent<TreeNode>();
                child.Data = connection.Data;
                child.Parent = root;

                BuildBM(start, goal, connection, history, child, treeNode);
            }
            history.Remove(currentData);
        }
    }

    /// <summary>
    /// put all children in alphabetical order
    /// </summary>
    /// <param name="children">a list of nodes that are children of the current node</param>
    public static void Order(List<MapNode> children) =>
        children.Sort((child1, child2) => child1.Data.CompareTo(child2.Data));

    TreeNode FindNodeByPath(string path) => FindNodeByPath(rootBM, path);
    public static TreeNode FindNodeByPath(TreeNode root, string path)
    {
        List<char> dataPath = path.ToList();
        IEnumerable<(TreeNode, TreeNode)> nodes = root.GetComponentsInChildren<TreeNode>().Select(e => (e, e));
        do
        {
            nodes = nodes.Where(e => e.Item2.Data == dataPath.Last())
                .Select(e => (e.Item1, e.Item2.Parent))
                .ToList();
            dataPath.RemoveAt(dataPath.Count - 1);
        } while (nodes.Count() != 1);
        return nodes.First().Item1;
    }
}
