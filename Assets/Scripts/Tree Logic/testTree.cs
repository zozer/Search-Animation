using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestTree : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        MakeTree test = GetComponent<MakeTree>();
        test.CreateTest();
        List<List<string>> ret = test.BFSearch(FindObjectsOfType<MapNode>().First(e => e.Data == "s"), FindObjectsOfType<MapNode>().First(e => e.Data == "g"));

        //test.AnimateDFSteps(ret, test.rootBM);
        foreach (MapNode obj in FindObjectsOfType<MapNode>())
        {
            obj.gameObject.SetActive(false);
        }
        //test.AdjustNodes(test.rootBM);
        //StartCoroutine(test.AnimateSteps(ret, test.rootBM));
    }
}
