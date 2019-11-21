using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeTree : MonoBehaviour
{
    public TreeNode rootBM;
    public TreeNode rootDF;
    public TreeNode rootBF;
    public GameObject mapNode;
    public GameObject treeNode;

    public List<List<string>> DFSearch(MapNode start, MapNode end)
    {
        List<List<string>> procedureDF = new List<List<string>>();
        rootDF = Instantiate(treeNode, transform).GetComponent<TreeNode>();
        rootDF.name = "DF-" + start.Data;
        rootDF.Data = start.Data;

        //build DF tree
        BuildDF(rootBM, end, rootDF, procedureDF);
        foreach (List<string> l in procedureDF)
        {
            //print("(" + string.Join(")(", l) + ")");
        }
        rootDF.gameObject.SetActive(false);
        return procedureDF;
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

        //add name to gameObject
        start.name = "s";
        nodez.name = "z";
        nodea.name = "a";
        nodex.name = "x";
        noded.name = "d";
        nodec.name = "c";
        nodef.name = "f";
        goal.name = "g";

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

        //place holder for visited nodes
        List<string> history = new List<string>();

        //new gameObject for BM tree
        rootBM = Instantiate(treeNode, transform).GetComponent<TreeNode>();
        rootBM.name = start.Data;
        rootBM.Data = start.Data;

        //build BM tree
        BuildBM(start, goal, start, history, rootBM);

        //new gameObject for DF tree

        /*//new gameObject for BF tree
        List<List<string>> procedureBF = new List<List<string>>();
        rootBF = new GameObject("", typeof(TreeNode)).GetComponent<TreeNode>();
        rootBF.name = start.Data;
        rootBF.data = start.Data;
        int height = CheckHeight(rootBM);

        //create BF tree 
        BuildBF(rootBM, goal, rootBF, height);

        //create procedure for BF
        height = CheckHeight(rootBF);
        MakeProcedureBF(rootBF, procedureBF, height, goal);*/

        /*
        //print out all procedure(DF)
        for (int i = 0; i < procedureDF.Count; i++)
        {
            string sentence = "";
            for (int j = 0; j < procedureDF[i].Count; j++)
            {
                sentence += "(" + procedureDF[i][j] +")";
            }
            Debug.Log(sentence);
        }*/

        /*
        //print out all procedure(BF)
        for (int i = 0; i < procedureBF.Count; i++)
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
    /// sorting the list in alphabetical order
    /// </summary>
    /// <param name="children"></param>
    public static void Order(List<MapNode> children)
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
                history.Contains(nextData) == false)
            {
                TreeNode child = Instantiate(treeNode).GetComponent<TreeNode>();
                child.Data = connection.Data;
                child.name = connection.Data;
                child.Parent = root;

                BuildBM(start, goal, connection, history, child);
            }
            history.Remove(currentData);
        }
    }

    public void AdjustNodes(TreeNode root)
    {
        //adjust werid stuff unity does
        if (root == rootBM)
        {
            root.Children.ForEach(e =>
            {
                e.transform.localPosition = (Vector2)e.transform.localPosition;
                e.transform.localScale = Vector2.one;
            });
            root.transform.localPosition += new Vector3(-900, 200);
        }
        //end adjust
        List<TreeNode> current = root.Children;
        for (int i = 0; i < current.Count; i++)
        {
            int baseShift = i;
            if (i != 0 && current[i - 1].Leafs.Count != 0) {
                baseShift += current[i - 1].Leafs.Count - 1;
            }
            current[i].transform.localPosition += new Vector3(0.4f * baseShift, -0.2f, 0);
            AdjustNodes(current[i]);
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

        //stop when goal is found
        if (oriRoot.Data == goal.Data)
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
                string proString = oriRoot.Data;
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
                string newBranch = branch + oriRoot.Children[i].Data;
                newProcedure.Insert(i, newBranch);
            }

            procedure.Add(newProcedure);

            for (int i = 0; i < oriRoot.Children.Count; i++)
            {
                TreeNode child = Instantiate(treeNode).GetComponent<TreeNode>();
                child.name = oriRoot.Children[i].Data;
                child.Data = oriRoot.Children[i].Data;
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
        if (oriRoot.Data == goal.Data)
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
                TreeNode child = new GameObject("" + oriRoot.Children[i].Data, typeof(TreeNode)).GetComponent<TreeNode>();
                child.Data = oriRoot.Children[i].Data;
                child.Parent = newRoot;
                newRoot.AddChild(child);
            }
        }

        return false;
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
                return true;
            }
        }

        return false;
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
        if (level != 1)
        {
            for (int i = 0; i < root.Children.Count; i++)
            {
                bool find = ProcedureBFHelp(root.Children[i], procedureBF, (level - 1), goal, newProcedure, branch);
                newProcedure = new List<string>(procedureBF[procedureBF.Count - 1]);
                branch = newProcedure[0];
                newProcedure.RemoveAt(0);
                if (find == true)
                {
                    return true;
                }
            }
        }
        else
        {
            if (root.Data == goal.Data)
            {
                return true;
            }
            else if(root.Children.Count == 0)
            {
                procedureBF.Add(newProcedure);
                newProcedure = new List<string>(procedureBF[procedureBF.Count - 1]);
                branch = newProcedure[0];
                newProcedure.RemoveAt(0);
                return false;
            }
            else
            {
                for (int i = 0; i < root.Children.Count; i++)
                {
                    string temp = branch;
                    branch = branch + root.Children[i].Data;
                    newProcedure.Add(branch);
                    branch = temp;
                }
                procedureBF.Add(newProcedure);
                return false;
            }
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
        string proString = oriRoot.Data;
        List<string> startLine = new List<string>();
        startLine.Add(proString);
        procedureBF.Add(startLine);

        for (int i = 1; i <= height; i++)
        {
            List<string> newProcedure = new List<string>(procedureBF[procedureBF.Count - 1]);
            string branch = newProcedure[0];
            newProcedure.RemoveAt(0);
            bool find = ProcedureBFHelp(oriRoot, procedureBF, i, goal, newProcedure, branch);
            if (find == true)
            {
              return;
            }
        }
    }

    public IEnumerator AnimateDFSteps(List<List<string>> steps, TreeNode root)
    {
        foreach (TreeNode node in root.GetComponentsInChildren<TreeNode>())
        {
            node.GetComponent<SpriteRenderer>().color = Color.gray;
        }
        steps.RemoveAt(0);
        root.GetComponent<SpriteRenderer>().color = Color.yellow;
        List<List<TreeNode>> nodeSteps = NodeSteps(steps, root);

        foreach (List<TreeNode> step in nodeSteps)
        {
            bool first = true;
            yield return new WaitForSecondsRealtime(0.5f);
            foreach (TreeNode node in step)
            {
                if (!first && node.GetComponent<SpriteRenderer>().color == Color.blue)
                {
                    continue;
                }
                node.GetComponent<SpriteRenderer>().color = first ? Color.yellow : Color.blue;
                if (first)
                {
                    first = false;
                }
            }
        }
        yield return null;
        
    }

    List<List<TreeNode>> NodeSteps(List<List<string>> steps, TreeNode root) =>
        steps.Select(e => e.Select(f => FindNodeByPath(f, root)).ToList()).ToList();
    TreeNode FindNodeByPath(string path, TreeNode root)
    {
        List<char> dataPath = path.ToList();
        IEnumerable<(TreeNode, TreeNode)> nodes = root.GetComponentsInChildren<TreeNode>().Select(e => (e, e));
        do
        {
            nodes = nodes.Where(e => e.Item2.Data == "" + dataPath.Last())
                .Select(e => (e.Item1, e.Item2.Parent))
                .Where(e => !(e.Parent is null))
                .ToList();
            dataPath.RemoveAt(dataPath.Count - 1);
        } while (nodes.Count() != 1);
        return nodes.First().Item1;
    }
}
