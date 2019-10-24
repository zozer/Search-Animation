using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MenuHandler : MonoBehaviour, IDragHandler
{
    enum Mode { None, CreateNode, CreateLine};
    Mode currentMode = Mode.None;
    // Start is called before the first frame update
    GameObject selectedNode = null;
    public GameObject nodePrefab;
    public GameObject linePrefab;
    public GameObject canvasArea;
    Vector3[] corners = new Vector3[4];
    Vector2 totalDelta = new Vector2(0, 0);
    GameObject createdLine = null;
    void Start()
    {
        canvasArea.GetComponent<RectTransform>().GetWorldCorners(corners);
        DrawLines();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentMode)
        {
            case Mode.CreateNode:
                selectedNode.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Input.GetMouseButtonDown(0))
                {
                    selectedNode.GetComponent<Image>().color = Color.white;
                    selectedNode = null;
                    currentMode = Mode.None;
                }
                break;
            case Mode.CreateLine:
                if (selectedNode != null)
                {
                    LineRenderer tempRend = createdLine.GetComponent<LineRenderer>();
                    tempRend.SetPositions(new Vector3[] { selectedNode.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) });
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (selectedNode == null)
                    {
                        foreach (MapNode node in canvasArea.GetComponentsInChildren<MapNode>())
                        {
                            if (node.GetComponent<CircleCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                            {
                                selectedNode = node.gameObject;
                                createdLine = Instantiate(linePrefab, canvasArea.transform);
                                break;
                            }
                        }
                    } else
                    {
                        foreach (MapNode node in canvasArea.GetComponentsInChildren<MapNode>())
                        {
                            if (node.GetComponent<CircleCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                            {
                                if (node == selectedNode)
                                {
                                    break;
                                }
                                LineRenderer tempRend = createdLine.GetComponent<LineRenderer>();
                                tempRend.SetPositions(new Vector3[] { selectedNode.transform.position, node.transform.position });
                                createdLine = null;
                                currentMode = Mode.None;
                                selectedNode.GetComponent<MapNode>().Connections.Add(node);
                                selectedNode = null;
                                break;
                            }
                        }
                    }
                } else if (Input.GetMouseButtonDown(1))
                {
                    if (createdLine != null)
                    {
                        Destroy(createdLine);
                        createdLine = null;
                        currentMode = Mode.None;
                    }
                }
                break;
        }
    }
    public void OnDrag(PointerEventData data)
    {
        if (currentMode != Mode.None)
        {
            return;
        }
        foreach (LineRenderer line in canvasArea.GetComponentsInChildren<LineRenderer>())
        {
            if (line.transform.parent == canvasArea.transform)
            {
                Destroy(line.gameObject);
            }
        }
        foreach (MapNode node in canvasArea.GetComponentsInChildren<MapNode>())
        {
            node.transform.localPosition += (Vector3)data.delta;
        }
        List<(MapNode, MapNode)> tempList = new List<(MapNode, MapNode)>();
        foreach (MapNode node in canvasArea.GetComponentsInChildren<MapNode>())
        {
            foreach(MapNode child in node.Connections)
            {
                if (!tempList.Contains((child,node)))
                {
                    tempList.Add((node, child));
                    GameObject tempLine = Instantiate(linePrefab, canvasArea.transform);
                    LineRenderer tempRend = tempLine.GetComponent<LineRenderer>();
                    tempRend.SetPositions(new Vector3[] { node.transform.position, child.transform.position });
                }
            }
        }

        totalDelta += data.delta;
        foreach (Transform child in canvasArea.transform.Find("Grid"))
        {
            Destroy(child.gameObject);
        }
        DrawLines(Camera.main.ScreenToWorldPoint(totalDelta));
    }

    public void CreateNode()
    {
        selectedNode = Instantiate(nodePrefab,canvasArea.transform);
        selectedNode.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectedNode.GetComponent<Image>().color = Color.cyan;
        currentMode = Mode.CreateNode;
    }

    public void CreateLine()
    {
        currentMode = Mode.CreateLine;
    }
    void DrawLines()
    {
        for (int i = Mathf.RoundToInt(corners[0].x); i <= Mathf.RoundToInt(corners[3].x); i++)
        {
            GameObject tempLine = Instantiate(linePrefab, canvasArea.transform.Find("Grid"));
            LineRenderer tempRend = tempLine.GetComponent<LineRenderer>();
            tempRend.SetPositions(new Vector3[] { new Vector3(i, corners[0].y, 0), new Vector3(i, corners[1].y, 0) });
        }
        for (int j = Mathf.RoundToInt(corners[0].y); j < Mathf.RoundToInt(corners[1].y); j++)
        {
            GameObject tempLine = Instantiate(linePrefab, canvasArea.transform.Find("Grid"));
            LineRenderer tempRend = tempLine.GetComponent<LineRenderer>();
            tempRend.SetPositions(new Vector3[] { new Vector3(corners[0].x, j, 0), new Vector3(corners[3].x, j, 0) });
        }
    }

    void DrawLines(Vector2 baseDraw)
    {
        while (baseDraw[0] > Mathf.RoundToInt(corners[0].x))
        {
            baseDraw[0]--;
        }
        while (baseDraw[1] > Mathf.RoundToInt(corners[0].y))
        {
            baseDraw[1]--;
        }
        for (float i = baseDraw[0]; i <= Mathf.RoundToInt(corners[3].x); i++)
        {
            GameObject tempLine = Instantiate(linePrefab, canvasArea.transform.Find("Grid"));
            LineRenderer tempRend = tempLine.GetComponent<LineRenderer>();
            tempRend.SetPositions(new Vector3[] { new Vector3(i, corners[0].y, 0), new Vector3(i, corners[1].y, 0) });
        }
        for (float j = baseDraw[1]; j < Mathf.RoundToInt(corners[1].y); j++)
        {
            GameObject tempLine = Instantiate(linePrefab, canvasArea.transform.Find("Grid"));
            LineRenderer tempRend = tempLine.GetComponent<LineRenderer>();
            tempRend.SetPositions(new Vector3[] { new Vector3(corners[0].x, j, 0), new Vector3(corners[3].x, j, 0) });
        }
    }
}
