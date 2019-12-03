using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorBlock : MonoBehaviour
{
    public static readonly string missingStart = "No Start Node";
    public static readonly string missingEnd = "No Goal Node";
    public static readonly string nodeError = "Node Error Present";
    public static readonly string blankNode = "A Node is Empty";
    public static readonly string unconnectedNode = "A Node is Unconnected";

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator DisplayError(string val)
    {
        gameObject.SetActive(true);    
        transform.GetChild(0).GetComponent<Text>().text = val;
        for (float i = 0; i < 50; i++)
        {
            GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f, 0.5f - (float)(i / 100.0));
            transform.GetChild(0).GetComponent<Text>().color = new Color(0, 0, 0, 1 - (float)(i / 50.0));
            yield return new WaitForSeconds(0.05f);
        }
        gameObject.SetActive(false);
    }
}
