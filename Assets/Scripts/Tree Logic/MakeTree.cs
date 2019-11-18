﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTree : MonoBehaviour
{
    public TreeNode rootBM;
    public TreeNode rootDF;
    public TreeNode rootBF;
    public GameObject mapNode;
    /// <summary>
    /// For testing purpose only
    /// </summary>
    public void CreateTest()
    {
        MapNode start = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode nodez = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode nodea = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode nodex = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode noded = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode nodec = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode nodef = Instantiate(mapNode).GetComponent<MapNode>();
        MapNode goal = Instantiate(mapNode).GetComponent<MapNode>();

        start.Data = "s";
        nodez.Data = "z";
        nodea.Data = "a";
        nodex.Data = "x";
        noded.Data = "d";
        nodec.Data = "c";
        nodef.Data = "f";
        goal.Data = "g";

        start.name = "s";
        nodez.name = "z";
        nodea.name = "a";
        nodex.name = "x";
        noded.name = "d";
        nodec.name = "c";
        nodef.name = "f";
        goal.name = "g";

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

        //root = gameObject.AddComponent<TreeNode>();
        rootBM = new GameObject("", typeof(TreeNode)).GetComponent<TreeNode>();
        rootBM.name = start.Data;
        rootBM.data = start.Data;

        BuildBM(start, goal, start, history, rootBM);


        List<List<string>> procedureDF = new List<List<string>>();
        rootDF = new GameObject("", typeof(TreeNode)).GetComponent<TreeNode>();
        rootDF.name = start.Data;
        rootDF.data = start.Data;
        BuildDF(rootBM, goal, rootDF, procedureDF);

        List<List<string>> procedureBF = new List<List<string>>();
        rootBF = new GameObject("", typeof(TreeNode)).GetComponent<TreeNode>();
        rootBF.name = start.Data;
        rootBF.data = start.Data;
        int height = CheckHeight(rootBM);
        BuildBF(rootBM, goal, rootBF, height);
        MakeProcedureBF(rootBF, procedureBF, height, goal);

        /*
        for (int i = 0; i < procedureDF.Count; i++)
        {
            string sentence = "";
            for (int j = 0; j < procedureDF[i].Count; j++)
            {
                sentence += "(" + procedureDF[i][j] +")";
            }
            Debug.Log(sentence);
        }*/

        /*for (int i = 0; i < procedureBF.Count; i++)
        {
            string sentence = "";
            for (int j = 0; j < procedureBF[i].Count; j++)
            {
                sentence += "(" + procedureBF[i][j] + ")";
            }
            Debug.Log(sentence);
        }*/
    }

    /// <summary>
    /// sorting the list
    /// </summary>
    /// <param name="children"></param>
    public static void order(List<MapNode> children)
    {
        children.Sort((child1, child2) => child1.Data.CompareTo(child2.Data));
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
    public static void BuildBM(MapNode start, MapNode goal, MapNode current, List<string> history, TreeNode root)
    {
        //check if the node is leaf or goal
        if (current.Connections.Count == 0 || current.Data == goal.Data)
        {
            return;
        }

        //sort the list to follow alphabetical order
        if(current.Connections.Count > 1)
        {
            order(current.Connections);
        }

        int num = current.Connections.Count;
        string startData = start.Data;
        string currentData = current.Data;

        for (int i = 0; i < num; i++)
        {
            //create new node for the tree
            TreeNode child = new GameObject("" + current.Connections[i].Data, typeof(TreeNode)).GetComponent<TreeNode>();
            child.data = current.Connections[i].Data;

            history.Add(currentData);

            string nextData = current.Connections[i].Data;

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

    /// <summary>
    /// build depth-first tree
    /// </summary>
    /// <param name="oriRoot">root of british museum tree</param>
    /// <param name="goal">goal node</param>
    /// <param name="newRoot">root of depth-first tree</param>
    /// <param name="procedure">list to keep procedure of building depth-first tree</param>
    /// <returns></returns>
    private bool BuildDF(TreeNode oriRoot, MapNode goal, TreeNode newRoot, List<List<string>> procedure)
    {

        if (oriRoot.data == goal.Data)
        {
            return true;
        }
        else if (oriRoot.Children.Count == 0)
        {
            int num = procedure.Count;

            List<string> temp = new List<string>(procedure[num - 1]);
            temp.RemoveAt(0);
            procedure.Add(temp);
            return false;
        }
        else
        {

            if (procedure.Count == 0)
            {
                string proString = oriRoot.data;
                List<string> temp = new List<string>();
                temp.Add(proString);
                procedure.Add(temp);
            }

            int num = procedure.Count;

            List<string> newProcedure = new List<string>(procedure[num - 1]);
            string branch = newProcedure[0];

            newProcedure.RemoveAt(0);

            for (int i = 0; i < oriRoot.Children.Count; i++)
            {
                string newBranch = branch + oriRoot.Children[i].data;
                newProcedure.Insert(i, newBranch);
            }

            procedure.Add(newProcedure);

            for (int i = 0; i < oriRoot.Children.Count; i++)
            {
                TreeNode child = new GameObject("" + oriRoot.Children[i].data, typeof(TreeNode)).GetComponent<TreeNode>();
                child.data = oriRoot.Children[i].data;
                child.Parent = newRoot;
                newRoot.AddChild(child);
            }

            
            for (int i = 0; i < oriRoot.Children.Count; i++)
            {
                bool find = BuildDF(oriRoot.Children[i], goal, newRoot.Children[i], procedure);

                if (find == true)
                {
                    return true;
                }

            }

            return false;
        }
    }

    /// <summary>
    /// find the height of the tree
    /// </summary>
    /// <param name="root">root of the tree</param>
    /// <returns></returns>
    private int CheckHeight(TreeNode root)
    {
        if (root.Children.Count == 0)
        {
            return 1;
        }
        else
        {
            int height = 0;
            for (int i = 0; i < root.Children.Count; i++)
            {
                int current = CheckHeight(root.Children[i]);
                if (i == 0)
                {
                    height = current;
                }
                else
                {
                    if (height < current)
                    {
                        height = current;
                    }
                }
            }

            return (height + 1);
        }
    }

    /// <summary>
    /// level-order traversing
    /// </summary>
    /// <param name="oriRoot">root of the british museum tree</param>
    /// <param name="goal">goal node</param>
    /// <param name="level">current level in the tree</param>
    /// <param name="newRoot">root of the breadth-first tree</param>
    /// <returns></returns>
    private bool TraversingBF(TreeNode oriRoot, MapNode goal, int level, TreeNode newRoot)
    {
        if (oriRoot.data == goal.Data)
        {
            return true;
        }

        if (level != 1)
        {
            for (int i = 0; i < oriRoot.Children.Count; i++)
            {
                bool find = TraversingBF(oriRoot.Children[i], goal, (level - 1), newRoot.Children[i]);
                if (find == true)
                {
                    return true;
                }
            }
        }
        else
        {
            for (int i = 0; i < oriRoot.Children.Count; i++)
            {
                TreeNode child = new GameObject("" + oriRoot.Children[i].data, typeof(TreeNode)).GetComponent<TreeNode>();
                child.data = oriRoot.Children[i].data;
                child.Parent = newRoot;
                newRoot.AddChild(child);
            }
        }

        return false;
    }

    /// <summary>
    /// delete the extra nodes
    /// </summary>
    /// <param name="root">root of the tree</param>
    /// <param name="level">current level in the tree</param>
    private void DeleteExtra(TreeNode root, int level)
    {
        if(level != 1)
        {
            for (int i = 0; i < root.Children.Count; i++)
            {
                DeleteExtra(root.Children[i], (level - 1));
            }
        }
        else
        {
            Destroy(root.gameObject);
        }
    }

    /// <summary>
    /// build breadth-first tree
    /// </summary>
    /// <param name="oriRoot">root of the british museum tree</param>
    /// <param name="goal">goal node</param>
    /// <param name="newRoot">root of the breadth-first tree</param>
    /// <param name="height">height of the tree</param>
    /// <returns></returns>
    private bool BuildBF(TreeNode oriRoot, MapNode goal, TreeNode newRoot, int height)
    {
        for (int i = 1; i <= height; i++)
        {
            bool find = TraversingBF(oriRoot, goal, i, newRoot);

            if (find == true)
            {
                DeleteExtra(newRoot, i+1);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// not complete yet
    /// </summary>
    /// <param name="root"></param>
    /// <param name="procedureBF"></param>
    /// <param name="level"></param>
    /// <param name="goal"></param>
    /// <param name="branch"></param>
    /// <param name="newProcedure"></param>
    /// <returns></returns>
    private bool ProcedureBFHelp(TreeNode root, List<List<string>> procedureBF, int level, MapNode goal, string branch, List<string> newProcedure)
    {
        if (root.data == goal.Data)
        {
            branch = branch + root.data;
            newProcedure.Add(branch);
            return true;
        }

        if (level != 1)
        {
            for (int i = 0; i < root.Children.Count; i++)
            {
                bool find = ProcedureBFHelp(root.Children[i], procedureBF, (level - 1), goal, branch, newProcedure);

                if (find == true)
                {
                    return true;
                }
            }
        }
        else
        {
            for (int i = 0; i < root.Children.Count; i++)
            {
                branch = branch + root.data;
                newProcedure.Add(branch);
            }
        }


        return false;
    }

    /// <summary>
    /// not complete yet
    /// </summary>
    /// <param name="oriRoot"></param>
    /// <param name="procedureBF"></param>
    /// <param name="height"></param>
    /// <param name="goal"></param>
    private void MakeProcedureBF(TreeNode oriRoot, List<List<string>> procedureBF, int height, MapNode goal)
    {
        string proString = oriRoot.data;
        List<string> temp = new List<string>();
        temp.Add(proString);
        procedureBF.Add(temp);

        for (int i = 2; i <= height; i++)
        {
            int num = procedureBF.Count;
            List<string> newProcedure = new List<string>(procedureBF[num - 1]);
            string branch = newProcedure[0];
            newProcedure.RemoveAt(0);
            
            ProcedureBFHelp(oriRoot, procedureBF, i, goal, branch, newProcedure);

            procedureBF.Add(newProcedure);
        }
    }
}
