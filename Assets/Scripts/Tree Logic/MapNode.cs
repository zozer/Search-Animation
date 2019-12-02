using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MapNode: MonoBehaviour
{
    [HideInInspector]
    public Vector3 savedPos;
    public List<MapNode> Connections = new List<MapNode>();
    public char Data
    {
        get => transform.GetChild(0).GetComponent<Text>().text.FirstOrDefault();
        set
        {
            name = $"{value}";
            transform.GetChild(0).GetComponent<Text>().text = $"{value}";
        }
    }
}
