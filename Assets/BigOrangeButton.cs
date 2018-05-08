using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOrangeButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            //GameObject.FindObjectOfType<GameManager>().makeFilterButtonPressed();
            GameObject.FindObjectOfType<GameManager>().makeFilterOperands("TestFilter", "Number Filter", "1999", "Year", "GreaterThan", "1999", "3001", "Change Shape", "Cylinder");
        }

	}
}
