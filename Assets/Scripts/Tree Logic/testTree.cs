using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTree : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        MakeTree test = gameObject.AddComponent<MakeTree>();
        test.CreateTest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
