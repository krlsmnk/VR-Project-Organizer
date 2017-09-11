using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeExample : MonoBehaviour {

    Material material;

	void Start () {
        this.material = gameObject.GetComponent<MeshRenderer>().material;
        this.material.color = Color.green;
    }

    void Update () {
        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
	}

}
