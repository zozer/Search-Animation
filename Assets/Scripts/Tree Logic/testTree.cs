using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTree : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        MakeTree test = GetComponent<MakeTree>();
        test.CreateTest();
        foreach (MapNode obj in FindObjectsOfType<MapNode>())
        {
            obj.gameObject.SetActive(false);
        }
        //test.rootBM.Debug();
        //test.rootDF.Debug();
        //test.rootBF.Debug();
        test.AdjustNodes(test.rootBM);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
