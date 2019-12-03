using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class AnimationHandler : MonoBehaviour, IDragHandler
{
    private enum SearchMode { BF, DF };
    public GameObject canvasArea;
    public GameObject treeNode;
    public GameObject linePrefab;
    public Text stepTextField;

    MakeTree treeBuilder;
    List<List<string>> DFSteps;
    List<List<string>> BFSteps;

    private bool pause = false;
    private SearchMode mode = SearchMode.BF;
    private int currentStep = 0;
    private TreeNode treeRoot;
    private bool animStart = false;
    private void Start()
    {
        IEnumerable<MapNode> mapNodes = FindObjectsOfType<MapNode>();
        if (mapNodes.Count() > 0)
        {
            MapNode start = mapNodes.FirstOrDefault(e => e.Data == 's');
            MapNode end = mapNodes.FirstOrDefault(e => e.Data == 'g');
            treeRoot = Instantiate(treeNode, canvasArea.transform).GetComponent<TreeNode>();
            treeRoot.Data = start.Data;
            treeBuilder = GetComponent<MakeTree>();
            treeBuilder.rootBM = treeRoot;
            treeBuilder.BuildBM(start, end, start, new List<char>(), treeRoot);
            AdjustNodes(treeRoot);
            treeRoot.transform.parent.localPosition -= new Vector3(0, 0, 1);

            BFSteps = treeBuilder.BFSearch(end.Data);
            DFSteps = treeBuilder.DFSearch(end.Data);
        }

        UpdateButtons();
    }

    /// <summary>
    /// Adjust nodes visually on the screen to look somewhat presentable
    /// </summary>
    /// <param name="root"></param>
    public void AdjustNodes(TreeNode root, GameObject objectRoot = null, float parentOffset = 0)
    {
        //adjust werid stuff unity does
        if (root == treeRoot)
        {
            root.Children.ForEach(e =>
            {
                e.transform.localPosition = (Vector2)e.transform.localPosition;
                e.transform.localScale = Vector2.one;
            });
            root.transform.localPosition += new Vector3(-900, 300);

            //create blank root so we can adjust root properly
            GameObject blankRoot = new GameObject();
            blankRoot.AddComponent<RectTransform>();
            blankRoot.transform.SetParent(root.transform.parent);
            blankRoot.transform.localPosition = Vector3.zero;
            root.transform.SetParent(blankRoot.transform);
            Vector2 scale = root.transform.localScale;
            blankRoot.transform.localScale *= scale;
            root.transform.localScale /= scale;
            root.transform.localPosition /= scale;
            parentOffset = 0.2f * (root.Leafs.Count - 1);
            root.transform.localPosition -= new Vector3(root.transform.localPosition.x, 0, 0);
            objectRoot = blankRoot;
        }
        //end adjust
        List<TreeNode> current = root.Children;
        for (int i = 0; i < current.Count; i++)
        {
            int baseShift = i;
            if (i != 0 && current[i - 1].Leafs.Count != 0)
            {
                baseShift += current[i - 1].Leafs.Count - 1;
            }
            float offset = 0.2f * (current[i].Leafs.Count - 1);
            current[i].transform.localPosition = new Vector3((0.4f * baseShift) - parentOffset + offset, -0.3f, 0);
            AdjustNodes(current[i], objectRoot, offset);
            LineRenderer line = Instantiate(linePrefab, objectRoot.transform).GetComponent<LineRenderer>();
            line.SetPositions(new Vector3[] { (Vector2)root.transform.position, (Vector2)current[i].transform.position });
            line.startColor = line.endColor = Color.yellow;
        }
    }

    public void PlayAnimation()
    {
        if (treeRoot.GetComponent<SpriteRenderer>().color != Color.white)
        {
            ResetAnim();
        }
        _ = StartCoroutine(AnimateSteps(mode == SearchMode.BF ? BFSteps : DFSteps));
    }

    public void PauseAnmiation()
    {
        pause = true;
        UpdateButtons();
    }

    public void ResumeAnimation()
    {
        pause = false;
        UpdateButtons();
    }

    public void NextStep()
    {
        List<List<string>> steps = mode == SearchMode.BF ? BFSteps : DFSteps;
        
        if (currentStep >= steps.Count)
        {
            return;
        }

        PerformStep(steps[currentStep++]);

        if (currentStep >= steps.Count)
        {
            FindNodeByPath(steps.Last().First()).GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    public void PrevStep()
    {
        if (currentStep == 0)
        {
            return;
        }
        List<List<string>> steps = mode == SearchMode.BF ? BFSteps : DFSteps;
        int stepTemp = currentStep;
        //just reset and build back up, trying to backtrack is too much work
        foreach (TreeNode node in treeRoot.GetComponentsInChildren<TreeNode>())
        {
            node.GetComponent<SpriteRenderer>().color = Color.gray;
        }
        currentStep = 0;
        stepTextField.text = "";

        treeRoot.GetComponent<SpriteRenderer>().color = Color.yellow;

        for (currentStep = 1; currentStep < stepTemp - 1; currentStep++)
        {
            PerformStep(steps[currentStep]);
        }
    }

    public void Switch(Dropdown value)
    {
        mode = (SearchMode)value.value;
    }

    public void Back()
    {
        SceneManager.LoadScene("Main");
    }

    void UpdateButtons()
    {
        GameObject.Find("PlayButton").GetComponent<Button>().interactable = !animStart;
        GameObject.Find("ResumeButton").GetComponent<Button>().interactable = pause;
        GameObject.Find("NextStepButton").GetComponent<Button>().interactable = pause;
        GameObject.Find("PreviousStepButton").GetComponent<Button>().interactable = pause;
        GameObject.Find("PauseButton").GetComponent<Button>().interactable = !pause && animStart;
        GameObject.Find("BackButton").GetComponent<Button>().interactable = !animStart;
        GameObject.Find("Dropdown").GetComponent<Dropdown>().interactable = !animStart;
    }
    public IEnumerator AnimateSteps(List<List<string>> steps)
    {
        animStart = true;
        UpdateButtons();
        foreach (TreeNode node in treeRoot.GetComponentsInChildren<TreeNode>())
        {
            node.GetComponent<SpriteRenderer>().color = Color.gray;
        }
        //treeRoot.GetComponent<SpriteRenderer>().color = Color.yellow;
        //List<List<TreeNode>> nodeSteps = NodeSteps(steps);

        for (currentStep = 0; currentStep < steps.Count; currentStep++)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            yield return new WaitWhile(() => pause);
            //in case where next button was pressed to get to the end
            if (currentStep >= steps.Count)
            {
                break;
            }
            PerformStep(steps[currentStep]);
        }
        FindNodeByPath(steps.Last().First()).GetComponent<SpriteRenderer>().color = Color.green;
        animStart = false;
        UpdateButtons();
    }

    void PerformStep(List<string> step)
    {
        stepTextField.text += " -";
        for (int i = 0; i < step.Count; i++)
        {
            TreeNode node = FindNodeByPath(step[i]);
            if (i == 0)
            {
                print(canvasArea.GetComponent<RectTransform>().rect);
                print(node.transform.localPosition);
                print(node.transform.position);
                print(canvasArea.GetComponent<RectTransform>().rect.Contains(node.transform.localPosition));
            }
            node.GetComponent<SpriteRenderer>().color = i == 0 ? Color.yellow : Color.blue;
            stepTextField.text += " (" + step[i] + ")";
        }
        stepTextField.text += "\n";
    }

    public void ResetAnim()
    {
        foreach (TreeNode node in treeRoot.GetComponentsInChildren<TreeNode>())
        {
            node.GetComponent<SpriteRenderer>().color = Color.white;
        }
        currentStep = 0;
        stepTextField.text = "";
    }

    TreeNode FindNodeByPath(string path) => MakeTree.FindNodeByPath(treeRoot, path);

    public void OnDrag(PointerEventData data)
    {
        foreach (LineRenderer line in canvasArea.GetComponentsInChildren<LineRenderer>())
        {
            Destroy(line.gameObject);
        }
        treeRoot.transform.parent.localPosition += (Vector3)data.delta;
        DrawLines(treeRoot);

    }

    void DrawLines(TreeNode root)
    {
        foreach (TreeNode child in root.Children)
        {
            LineRenderer line = Instantiate(linePrefab, treeRoot.transform.parent.transform).GetComponent<LineRenderer>();
            line.SetPositions(new Vector3[] { (Vector2)root.transform.position, (Vector2)child.transform.position });
            line.startColor = line.endColor = Color.yellow;
            DrawLines(child);
        }   
    }
}
