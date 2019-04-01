using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CAVS.ProjectOrganizer.Interation;

public class ControllerTest : MonoBehaviour {

	[SerializeField]
	RotateWidget selectable;

	// Use this for initialization
	void Start () {
		selectable.Select(gameObject);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
