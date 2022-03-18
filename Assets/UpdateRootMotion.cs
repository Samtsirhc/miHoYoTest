using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRootMotion : MonoBehaviour
{
    public GameObject realRoot;
    public float buffer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        buffer = realRoot.transform.position.z - buffer;
        if (buffer > 0)
        {
            transform.Translate(new Vector3(0, 0, buffer), Space.Self);
        }
    }
}
