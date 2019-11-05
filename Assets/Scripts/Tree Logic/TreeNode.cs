using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode : MonoBehaviour
{
    public string data;
    public TreeNode parent;
    public List<TreeNode> children = new List<TreeNode>();
}
