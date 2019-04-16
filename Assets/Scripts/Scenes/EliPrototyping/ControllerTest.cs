using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CAVS.ProjectOrganizer.Interation;

public class ControllerTest : MonoBehaviour {

	[SerializeField]
	RotateWidget selectable;

	void Start () {
		selectable.SelectPress(gameObject);	
	}
	
}
