using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toFollow : MonoBehaviour {

public Transform followThis;
    private Vector3 offset;
 
    void Start()
    {
        offset = followThis.position - transform.position;
    }
    void Update()
    {
        transform.position = followThis.position - offset;
        transform.rotation = followThis.rotation;
    }
}
