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
        //test.rootBM.Debug();
        //test.rootDF.Debug();
        //test.rootBF.Debug();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
