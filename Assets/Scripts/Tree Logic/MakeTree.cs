using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeTree : MonoBehaviour
{
    [HideInInspector]
    public TreeNode rootBM;
    [HideInInspector]
    public TreeNode rootDF;
    [HideInInspector]
    public TreeNode rootBF;
    public GameObject mapNode;
    public GameObject treeNode;
    public GameObject linePrefab;
    public List<List<string>> DFSearch(MapNode start, MapNode end)
    {
        List<List<string>> procedureDF = new List<List<string>>();
        rootDF = Instantiate(treeNode, transform).GetComponent<TreeNode>();
        rootDF.Data = start.Data;
        rootDF.name = "DF-" + start.Data;

        //build DF tree
        BuildDF(rootBM, end, procedureDF);
        rootDF.gameObject.SetActive(false);
        return procedureDF;
    }

    public List<List<string>> BFSearch(MapNode start, MapNode end)
    {
        List<List<string>> procedureBF = new List<List<string>>();
        rootBF = Instantiate(treeNode, transform).GetComponent<TreeNode>();
        rootBF.Data = start.Data;
        rootBF.name = "BF-" + start.Data;
        int height = CheckHeight(rootBM);

        //create procedure for BF
        MakeProcedureBF(rootBF, procedureBF, height, end);
        rootBF.gameObject.SetActive(false);
        return procedureBF;
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
        start.Data = "s";
        nodez.Data = "z";
        nodea.Data = "a";
        nodex.Data = "x";
        noded.Data = "d";
        nodec.Data = "c";
        nodef.Data = "f";
        goal.Data = "g";

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
        BuildBM(start, goal, start, new List<string>(), rootBM);

        int height = CheckHeight(rootBM);
        List<List<string>> procedureDF = new List<List<string>>();
        MakeProcedureBF(rootBM, procedureDF, height, goal);
        for (int i = 0; i < procedureDF.Count; i++) {
            for(int j = 0; j < procedureDF[i].Count; j++)
            {
                Debug.Log(procedureDF[i][j]);
            }
        }
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
    public void BuildBM(MapNode start, MapNode goal, MapNode current, List<string> history, TreeNode root)
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
        string startData = start.Data;
        string currentData = current.Data;

        //travsering nodes
        foreach (MapNode connection in current.Connections)
        {
            //create new node for the tree
            history.Add(currentData);
            string nextData = connection.Data;

            //check if the node is visted or it is a start node
            if (currentData == startData || nextData != startData &&
                !history.Contains(nextData))
            {
                TreeNode child = Instantiate(treeNode).GetComponent<TreeNode>();
                child.Data = connection.Data;
                child.Parent = root;

                BuildBM(start, goal, connection, history, child);
            }
            history.Remove(currentData);
        }
    }

    /// <summary>
    /// build depth-first tree
    /// </summary>
    /// <param name="oriRoot">root of british museum tree</param>
    /// <param name="goal">goal node</param>
    /// <param name="newRoot">root of depth-first tree</param>
    /// <param name="procedure">list to keep procedure of building depth-first tree</param>
    /// <returns></returns>
    private bool BuildDF(TreeNode oriRoot, MapNode goal, List<List<string>> procedure)
    {

        //stop when goal is found
        if (oriRoot.Data == goal.Data)
        {
            return true;
        }

        //if the current has no child, delete the first thing in the procedure
        //and add to list as a new line
        else if (oriRoot.Children.Count == 0)
        {

            List<string> temp = new List<string>(procedure[procedure.Count - 1]);
            temp.RemoveAt(0);
            procedure.Add(temp);
            return false;
        }

        //if the procedure is empty,
        //add start node as the first line
        if (procedure.Count == 0)
        {
            procedure.Add(new List<string>() { oriRoot.Data });
        }

        //copy the last line
        List<string> newProcedure = new List<string>(procedure[procedure.Count - 1]);
        string branch = newProcedure[0];

        //remove the first element from the line
        //to indicate visited
        newProcedure.RemoveAt(0);

        //add all children of the current node to the front in alphabetical order
        for (int i = 0; i < oriRoot.Children.Count; i++)
        {
            string newBranch = branch + oriRoot.Children[i].Data;
            newProcedure.Insert(i, newBranch);
        }

        //add to the procedure as new line
        procedure.Add(newProcedure);

        //visit all nodes in the tree
        for (int i = 0; i < oriRoot.Children.Count; i++)
        {
            if (BuildDF(oriRoot.Children[i], goal, procedure))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// find the height of the tree
    /// </summary>
    /// <param name="root">root of the tree</param>
    /// <returns></returns>
    private int CheckHeight(TreeNode root)
    {
        //base height is 1
        if (root.Children.Count == 0)
        {
            return 1;
        }

        int height = 0;
        //loop through all the branch and calculate the height
        foreach (TreeNode child in root.Children)
        {
            int current = CheckHeight(child);

            //choose the highest child
            if (height < current)
            {
                height = current;
            }
        }

        return height + 1;
    }

    /// <summary>
    /// travsering BF tree
    /// </summary>
    /// <param name="root">root of the tree</param>
    /// <param name="procedureBF">list of list for storage of procedure</param>
    /// <param name="level">current level in the tree</param>
    /// <param name="goal">goal node</param>
    /// <param name="branch">a string that stores the first procedure in each line</param>
    /// <param name="newProcedure">a list of procedure that presents a line</param>
    /// <returns></returns>
    private bool ProcedureBFHelp(TreeNode root, List<List<string>> procedureBF, int level, MapNode goal, List<string> newProcedure, string branch)
    {
        //if the level is not the chosen level,
        //move on
        if (level != 1)
        {
            //loop through all chilren and stop when finding goal
            //create a new line after each visit
            foreach (TreeNode child in root.Children)
            {
                bool find = ProcedureBFHelp(child, procedureBF, level - 1, goal, newProcedure, branch);
                newProcedure = new List<string>(procedureBF[procedureBF.Count - 1]);
                branch = newProcedure[0];
                newProcedure.RemoveAt(0);
                if (find)
                {
                    return true;
                }
            }
        }
        else
        {
            //stop when we find the goal
            if (root.Data == goal.Data)
            {
                return true;
            }

            //create new line without the first element of the last line
            else if(root.Children.Count == 0)
            {
                procedureBF.Add(newProcedure);
                newProcedure = new List<string>(procedureBF[procedureBF.Count - 1]);
                newProcedure.RemoveAt(0);
                return false;
            }

            //add all children to the end of the new line
            foreach (TreeNode child in root.Children)
            {
                string temp = branch;
                branch += child.Data;
                newProcedure.Add(branch);
                branch = temp;
            }
            procedureBF.Add(newProcedure);
            return false;
        }

        return false;
    }
    
    /// <summary>
    /// travsering BF and creating procedure
    /// </summary>
    /// <param name="oriRoot">root for the BF tree</param>
    /// <param name="procedureBF">list of list for procedure</param>
    /// <param name="height">height of the tree</param>
    /// <param name="goal">goal node</param>
    private void MakeProcedureBF(TreeNode oriRoot, List<List<string>> procedureBF, int height, MapNode goal)
    {
        //add start node as the first line
        procedureBF.Add(new List<string> { oriRoot.Data });

        //create new line for each child in each level
        for (int i = 1; i <= height; i++)
        {
            List<string> newProcedure = new List<string>(procedureBF[procedureBF.Count - 1]);
            string branch = newProcedure[0];
            newProcedure.RemoveAt(0);

            //stop when we find the goal
            if (ProcedureBFHelp(oriRoot, procedureBF, i, goal, newProcedure, branch))
            {
              return;
            }
        }
    }

    /// <summary>
    /// put all children in alphabetical order
    /// </summary>
    /// <param name="children">a list of nodes that are children of the current node</param>
    public static void Order(List<MapNode> children) =>
        children.Sort((child1, child2) => child1.Data.CompareTo(child2.Data));
}
