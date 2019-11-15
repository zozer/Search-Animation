using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNode: MonoBehaviour
{
    public List<MapNode> Connections = new List<MapNode>();
    private string _data;
    public string Data
    {
        get => _data;
        set {
            _data = value;
            transform.GetChild(0).GetComponent<Text>().text = value;
        }
    }
}
