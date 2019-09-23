using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject selectedNode = null;
    public GameObject nodePefab;
    public GameObject canvasArea;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedNode != null)
        {
            selectedNode.transform.position = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                selectedNode.GetComponent<Image>().color = Color.white;
                selectedNode = null;
            }
        }
    }

    public void CreateNode()
    {
        selectedNode = Instantiate(nodePefab,canvasArea.transform);
        selectedNode.transform.position = Input.mousePosition;
        selectedNode.GetComponent<Image>().color = Color.cyan;
    }
}
