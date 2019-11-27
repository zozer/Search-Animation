using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class TreeNode : MonoBehaviour
{
    public string Data
    {
        get => GetComponentsInChildren<Text>().Where(e => e.transform.parent == transform).First().text;
        set
        {
            name = value;
            GetComponentsInChildren<Text>().Where(e => e.transform.parent == transform).First().text = value;
        }
    }
    public TreeNode Parent
    {
        get => transform.parent?.GetComponent<TreeNode>();
        set => transform.SetParent(value?.transform);
    }
    public List<TreeNode> Children => transform.GetComponentsInChildren<TreeNode>().Where(e => e.Parent == this).ToList();
    public List<TreeNode> Leafs => transform.GetComponentsInChildren<TreeNode>().Where(e => e.Children.Count == 0).ToList();
    public void AddChild(TreeNode node)
    {
        node.Parent = this;
    }

    public void Debug()
    {
        List<string> branches = new List<string>();
        List<TreeNode> leaf = Leafs;
        TreeNode current;
        foreach (TreeNode node in leaf)
        {
            current = node;
            branches.Add(node.Data);
            current = current.Parent;
            while (current)
            {
                branches[branches.Count - 1] += current.Data;
                current = current.Parent;
            }
        }
        foreach(string branch in branches)
        {
            print(string.Join("->", branch.Reverse().ToList()));
        }
    }
}
