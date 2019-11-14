using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TreeNode : MonoBehaviour
{
    public string data;
    public TreeNode Parent
    {
        get => transform.parent?.GetComponent<TreeNode>();
        set => transform.parent = value.transform;
    }
    public List<TreeNode> Children
    {
        get => transform.GetComponentsInChildren<TreeNode>().Where(e => e.Parent == this).ToList();
    }
    public void AddChild(TreeNode node)
    {
        node.Parent = this;
    }

    public void Debug()
    {
        List<string> branches = new List<string>();
        List<TreeNode> leaf = transform.GetComponentsInChildren<TreeNode>().Where(e => e.transform.childCount == 0).ToList();
        TreeNode current;
        foreach (TreeNode node in leaf)
        {
            current = node;
            branches.Add(node.data);
            current = current.Parent;
            while (current != null)
            {
                branches[branches.Count - 1] += current.data;
                current = current.Parent;
            }
        }
        foreach(string branch in branches)
        {
            print(string.Join("->", branch.Reverse().ToList()));
        }
    }
}
