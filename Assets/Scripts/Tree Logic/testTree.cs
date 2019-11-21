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
        List<List<string>> ret = test.DFSearch(FindObjectsOfType<MapNode>().First(e => e.Data == "s"), FindObjectsOfType<MapNode>().First(e => e.Data == "g"));
        StartCoroutine(test.AnimateSteps(ret, test.rootBM));
        //test.AnimateDFSteps(ret, test.rootBM);
        foreach (MapNode obj in FindObjectsOfType<MapNode>())
        {
            obj.gameObject.SetActive(false);
        }
        //test.rootBM.Debug();
        //test.rootDF.Debug();
        //test.rootBF.Debug();
        test.AdjustNodes(test.rootBM);
    }
}
