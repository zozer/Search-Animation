using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode: MonoBehaviour
{
    public List<MapNode> Connections = new List<MapNode>();
    public string data;
}
