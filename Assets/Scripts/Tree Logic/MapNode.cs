using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNode: MonoBehaviour
{
    public List<MapNode> Connections = new List<MapNode>();
    public string Data
    {
        get => transform.GetChild(0).GetComponent<Text>().text;
        set => transform.GetChild(0).GetComponent<Text>().text = value;
    }
}
