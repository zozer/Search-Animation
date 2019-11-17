using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTree : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject mapNode;
    void Start()
    {
        MakeTree test = gameObject.AddComponent<MakeTree>();
        test.mapNode = mapNode;
        test.CreateTest();
        test.root.Debug();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
