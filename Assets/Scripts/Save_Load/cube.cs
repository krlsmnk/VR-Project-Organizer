using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour {

    public float xPos, yPos, zPos;
    public float moveSpeed = 10f;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
        //transform.Rotate(0, 1, 0);

        // if up arrow pressed move cube up
        if(Input.GetKey(KeyCode.UpArrow)){
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
        // If down arrow pressed move cube down
        if(Input.GetKey(KeyCode.DownArrow)){
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
        // If right arrow pressed move cube forward
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        // If left arrow pressed move cube backward
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }

        // Get x,y, and z position of transform
        xPos = transform.position.x;
        yPos = transform.position.y;
        zPos = transform.position.z;

        print(xPos + "," + yPos + "," + zPos + ",");
		
	}
}
