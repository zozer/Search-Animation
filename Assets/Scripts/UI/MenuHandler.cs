using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuHandler : MonoBehaviour, IDragHandler
{
    enum Mode { None, SelectNode, CreateNode, CreateLine, DestoryNode, DestoryLine };

    Mode CurrentMode { get; set; }
    // Start is called before the first frame update
    GameObject _selectedNode;
    GameObject SelectedNode
    {
        get => _selectedNode;
        set
        {
            if (_selectedNode)
            {
                _selectedNode.GetComponent<SpriteRenderer>().color = Color.white;
            }
            if (value)
            {
                value.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            _selectedNode = value;
        }
    }
    public GameObject nodePrefab;
    public GameObject linePrefab;
    public GameObject treeNodePrefab;
    public GameObject canvasArea;
    public GameObject menuArea;

    private Vector2 MousePosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    readonly Vector3[] corners = new Vector3[4];

    int totalVLines = 0;
    int totalHLines = 0;
    Vector2 totalDelta = new Vector2(0, 0);
    GameObject createdLine = null;
    void Start()
    {
        canvasArea.GetComponent<RectTransform>().GetWorldCorners(corners);
        DrawLines();
        //for purpose of if coming from animation scene
        foreach (MapNode node in FindObjectsOfType<MapNode>())
        {
            node.transform.SetParent(canvasArea.transform);
            node.transform.localPosition = node.savedPos;
        }
        List<(MapNode, MapNode)> tempList = new List<(MapNode, MapNode)>();
        foreach (MapNode node in canvasArea.GetComponentsInChildren<MapNode>())
        {
            foreach (MapNode child in node.Connections)
            {
                if (!tempList.Contains((child, node)))
                {
                    tempList.Add((node, child));
                    GameObject tempLine = Instantiate(linePrefab, canvasArea.transform);
                    LineRenderer tempRend = tempLine.GetComponent<LineRenderer>();
                    tempRend.startColor = tempRend.endColor = Color.red;
                    tempRend.SetPositions(new Vector3[] { node.transform.position, child.transform.position });
                }
            }
        }
        CurrentMode = Mode.None;
    }
    void OnGUI()
    {
        Event e = Event.current;
        if (CurrentMode == Mode.SelectNode && e.type == EventType.KeyUp && e.keyCode >= KeyCode.A && e.keyCode <= KeyCode.Z)
        {
            char currentKey = (char)e.keyCode;
            SelectedNode.GetComponent<MapNode>().Data = currentKey;
            if (canvasArea.GetComponentsInChildren<MapNode>().Count(n => n.Data == currentKey) > 1)
            {
                canvasArea.GetComponentsInChildren<MapNode>()
                          .Where(n => n.Data == currentKey)
                          .ToList()
                          .ForEach(n => n.GetComponent<SpriteRenderer>().color = Color.red);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        MapNode currentNode = null;
        switch (CurrentMode)
        {
            case Mode.None:
                if (Input.GetMouseButtonDown(0))
                {
                    currentNode = GetMapNode(MousePosition);
                    if (currentNode)
                    {
                        SelectedNode = currentNode.gameObject;
                        CurrentMode = Mode.SelectNode;
                    }
                }
                break;
            case Mode.SelectNode:
                if (Input.GetMouseButtonDown(0))
                {
                    currentNode = GetMapNode(MousePosition);
                    if (currentNode)
                    {
                        SelectedNode = currentNode.gameObject;
                        CurrentMode = Mode.SelectNode;
                    }
                    else
                    {
                        CurrentMode = Mode.None;
                        SelectedNode = null;
                    }
                }
                break;
            case Mode.CreateNode:
                Vector2 temp = MousePosition;
                temp.x = Mathf.RoundToInt(temp.x) + totalDelta.x;
                temp.y = Mathf.RoundToInt(temp.y) + totalDelta.y;
                SelectedNode.transform.position = temp;
                if (canvasArea.GetComponentsInChildren<MapNode>().Any(
                    e => (Vector2)e.GetComponent<RectTransform>().position == temp && e.gameObject != SelectedNode)
                    )
                {
                    SelectedNode.GetComponent<SpriteRenderer>().color = Color.red;
                }
                else
                {
                    SelectedNode.GetComponent<SpriteRenderer>().color = Color.cyan;
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(canvasArea.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
                        {
                            SelectedNode = null;
                            CurrentMode = Mode.None;
                        }
                    }
                }
                break;
            case Mode.CreateLine:
                if (SelectedNode)
                {
                    LineRenderer tempRend = createdLine.GetComponent<LineRenderer>();
                    tempRend.SetPositions(new Vector3[] { SelectedNode.transform.position, MousePosition });
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (!(currentNode = GetMapNode(MousePosition)))
                    {
                        break;
                    }
                    if (!SelectedNode)
                    {
                        SelectedNode = currentNode.gameObject;
                        createdLine = Instantiate(linePrefab, canvasArea.transform);
                        createdLine.GetComponent<MapLine>().connector = (currentNode, null);
                        createdLine.GetComponent<LineRenderer>().startColor = Color.red;
                        createdLine.GetComponent<LineRenderer>().endColor = Color.red;
                        break;
                    }
                    else
                    {
                        if (currentNode.gameObject == SelectedNode)
                        {
                            break;
                        }
                        if (canvasArea.GetComponentsInChildren<MapLine>().Any(
                            e => (e.connector.Item1 == SelectedNode.GetComponent<MapNode>() && e.connector.Item2 == currentNode) ||
                                (e.connector.Item1 == currentNode && e.connector.Item2 == SelectedNode.GetComponent<MapNode>())))
                        {
                            break;
                        }
                        LineRenderer tempRend = createdLine.GetComponent<LineRenderer>();
                        tempRend.GetComponent<MapLine>().connector.Item2 = currentNode;
                        tempRend.SetPositions(new Vector3[] { SelectedNode.transform.position, currentNode.transform.position });
                        createdLine = null;
                        SelectedNode.GetComponent<MapNode>().Connections.Add(currentNode);
                        currentNode.Connections.Add(SelectedNode.GetComponent<MapNode>());
                        SelectedNode = null;
                        break;
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (createdLine)
                    {
                        Destroy(createdLine);
                        createdLine = null;
                        SelectedNode = null;
                    }
                }
                break;
            case Mode.DestoryNode:
                if (Input.GetMouseButtonDown(0))
                {
                    currentNode = GetMapNode(MousePosition);
                    if (!currentNode)
                    {
                        break;
                    }
                    foreach (MapLine line in canvasArea.GetComponentsInChildren<MapLine>())
                    {
                        if (line.connector.Item1 == currentNode || line.connector.Item2 == currentNode)
                        {
                            Destroy(line.gameObject);
                        }
                    }
                    Destroy(currentNode.gameObject);
                }
                break;
            case Mode.DestoryLine:
                if (Input.GetMouseButtonDown(0))
                {
                    foreach (MapNode node in canvasArea.GetComponentsInChildren<MapNode>())
                    {
                        if (node.GetComponent<CircleCollider2D>().OverlapPoint(MousePosition))
                        {
                            currentNode = node;
                            break;
                        }
                    }
                    if (!currentNode)
                    {
                        break;
                    }
                    if (!SelectedNode)
                    {
                        SelectedNode = currentNode.gameObject;
                    } else
                    {
                        foreach (MapLine line in canvasArea.GetComponentsInChildren<MapLine>())
                        {
                            if ((line.connector.Item1 == SelectedNode.GetComponent<MapNode>() && line.connector.Item2 == currentNode) ||
                                (line.connector.Item2 == SelectedNode.GetComponent<MapNode>() && line.connector.Item1 == currentNode)
                                )
                            {
                                Destroy(line.gameObject);
                            }
                        }
                        SelectedNode = null;
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    SelectedNode = null;
                }
                break;
        }
    }

    public MapNode GetMapNode(Vector2 point)
    {
        foreach (MapNode node in canvasArea.GetComponentsInChildren<MapNode>())
        {
            if (node.GetComponent<CircleCollider2D>().OverlapPoint(point))
            {
                return node;
            }
        }
        return null;
    }
    public void OnDrag(PointerEventData data)
    {
        if (CurrentMode != Mode.None)
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
                if (!tempList.Contains((child, node)))
                {
                    tempList.Add((node, child));
                    GameObject tempLine = Instantiate(linePrefab, canvasArea.transform);
                    LineRenderer tempRend = tempLine.GetComponent<LineRenderer>();
                    tempRend.startColor = tempRend.endColor = Color.red;
                    tempRend.SetPositions(new Vector3[] { node.transform.position, child.transform.position });
                }
            }
        }
        Vector2 tempDelta = Camera.main.ScreenToWorldPoint(data.delta) - corners[0];
        totalDelta += tempDelta;
        totalDelta.x %= 1;
        totalDelta.y %= 1;
        foreach (LineRenderer child in canvasArea.transform.Find("Grid").GetComponentsInChildren<LineRenderer>())
        {
            Vector3 tempPos;
            if (child.GetPosition(0).x == child.GetPosition(1).x)
            {
                //verticle line
                tempPos = child.GetPosition(0);
                tempPos.x += tempDelta.x;
                if (tempPos.x > Mathf.RoundToInt(corners[3].x))
                {
                    tempPos.x -= totalVLines;
                }
                else if (tempPos.x < Mathf.RoundToInt(corners[0].x))
                {
                    tempPos.x += totalVLines;
                }
                child.SetPosition(0, tempPos);
                tempPos.y = child.GetPosition(1).y;
                child.SetPosition(1, tempPos);
            }
            else if (child.GetPosition(0).y == child.GetPosition(1).y)
            {
                //horizontal line
                tempPos = child.GetPosition(0);
                tempPos.y += tempDelta.y;
                if (tempPos.y > Mathf.RoundToInt(corners[1].y))
                {
                    tempPos.y -= totalHLines;
                }
                else if (tempPos.y < Mathf.RoundToInt(corners[0].y))
                {
                    tempPos.y += totalHLines;
                }
                child.SetPosition(0, tempPos);
                tempPos.x = child.GetPosition(1).x;
                child.SetPosition(1, tempPos);
            }
        }
    }

    public void CreateNode()
    {
        CleanUp();
        SelectedNode = Instantiate(nodePrefab,canvasArea.transform);
        SelectedNode.transform.position = MousePosition;
        SelectedNode.GetComponent<SpriteRenderer>().color = Color.cyan;
        CurrentMode = Mode.CreateNode;
        UpdateButtonStatus();
    }

    public void CreateLine()
    {
        CleanUp();
        CurrentMode = (CurrentMode == Mode.CreateLine) ? Mode.None : Mode.CreateLine;
        UpdateButtonStatus();
    }

    public void DestroyNode()
    {
        CleanUp();
        CurrentMode = (CurrentMode == Mode.DestoryNode) ? Mode.None : Mode.DestoryNode;
        UpdateButtonStatus();
    }

    public void DestroyLine()
    {
        CleanUp();
        CurrentMode = (CurrentMode == Mode.DestoryLine) ? Mode.None : Mode.DestoryLine;
        UpdateButtonStatus();
    }

    public void RunSimulation()
    {
        CleanUp();
        CurrentMode = Mode.None;
        UpdateButtonStatus();
        IEnumerable<MapNode> mapNodes = canvasArea.GetComponentsInChildren<MapNode>();
        MapNode start = mapNodes.FirstOrDefault(e => e.Data == 's');
        if (!start)
        {
            return;
        }
        MapNode end = mapNodes.FirstOrDefault(e => e.Data == 'g');
        if (!end)
        {
            return;
        }
        if (mapNodes.Any(e=>e.GetComponent<SpriteRenderer>().color == Color.red))
        {
            return;
        }
        foreach (MapNode node in mapNodes)
        {
            node.savedPos = node.transform.localPosition;
            node.transform.SetParent(null);
            DontDestroyOnLoad(node);
        }
        SceneManager.LoadScene("Animation");
    }

    void CleanUp()
    {
        switch (CurrentMode)
        {
            case Mode.None:
            case Mode.SelectNode:
            case Mode.DestoryLine:
                SelectedNode = null;
                break;
            case Mode.CreateNode:
                Destroy(SelectedNode);
                SelectedNode = null;
                break;
            case Mode.CreateLine:
                if (!(createdLine is null))
                {
                    SelectedNode = null;
                    Destroy(createdLine);
                }
                break;
        }
    }
    void UpdateButtonStatus()
    {
        menuArea.transform.Find("LineButton").GetComponent<Image>().color =
            (CurrentMode == Mode.CreateLine) ? new Color(0.5f, 0.5f, 0.5f) : Color.white;
        menuArea.transform.Find("DeleteNodeButton").GetComponent<Image>().color =
            (CurrentMode == Mode.DestoryNode) ? new Color(0.5f, 0.5f, 0.5f) : Color.white;
        menuArea.transform.Find("DeleteLineButton").GetComponent<Image>().color =
            (CurrentMode == Mode.DestoryLine) ? new Color(0.5f, 0.5f, 0.5f) : Color.white;
    }

    void DrawLines()
    {
        int i;
        for (i = Mathf.RoundToInt(corners[0].x); i <= Mathf.RoundToInt(corners[3].x); i++)
        {
            GameObject tempLine = Instantiate(linePrefab, canvasArea.transform.Find("Grid"));
            Destroy(tempLine.GetComponent<MapLine>());
            LineRenderer tempRend = tempLine.GetComponent<LineRenderer>();
            tempRend.SetPositions(new Vector3[] { new Vector3(i, corners[0].y, 0), new Vector3(i, corners[1].y, 0) });
        }
        totalVLines = i - Mathf.RoundToInt(corners[0].x);
        int j;
        for (j = Mathf.RoundToInt(corners[0].y); j < Mathf.RoundToInt(corners[1].y); j++)
        {
            GameObject tempLine = Instantiate(linePrefab, canvasArea.transform.Find("Grid"));
            Destroy(tempLine.GetComponent<MapLine>());
            LineRenderer tempRend = tempLine.GetComponent<LineRenderer>();
            tempRend.SetPositions(new Vector3[] { new Vector3(corners[0].x, j, 0), new Vector3(corners[3].x, j, 0) });
        }
        totalHLines = j - Mathf.RoundToInt(corners[0].y);
    }
}
