using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CAVS.ProjectOrganizer.Project.Filtering;
using CAVS.ProjectOrganizer.Project;
using UnityEngine.UI;
using System;



public class GameManager : MonoBehaviour {

	/// <summary>
	/// make a GameObject reference to assign later
	/// this will be used to get the text values from the vending machine
	/// </summary>
	public GameObject currentGameObject;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void makeFilterButtonPressed(){

		//get the list of all items/nodes
		Item[] nodes = ProjectFactory.BuildItemsFromCSV("CarData.csv");

		//get the filter name from the scene
		currentGameObject = GameObject.Find("Filter Name");
		string filterName = currentGameObject.GetComponent<Text>().text;

		//get the filter type from the scene
		currentGameObject = GameObject.Find("Filter Type");
		string filterType = currentGameObject.GetComponent<Text>().text;

		//get the filter value from the scene
		currentGameObject = GameObject.Find("Filter Value");
		string filterValue = currentGameObject.GetComponent<Text>().text;

		//get the field from the scene
		currentGameObject = GameObject.Find("Field");
		string field = currentGameObject.GetComponent<Text>().text;

		//get the operator from the scene
		currentGameObject = GameObject.Find("Operator");
		string op = currentGameObject.GetComponent<Text>().text;

		//get the min and max from the scene
		currentGameObject = GameObject.Find("Min");
		string min = currentGameObject.GetComponent<Text>().text;
		currentGameObject = GameObject.Find("Max");
		string max = currentGameObject.GetComponent<Text>().text;

		//get pass/fail behavior from scene
		currentGameObject = GameObject.Find("Filter Behavior");
		string behavior = currentGameObject.GetComponent<Text>().text;
		currentGameObject = GameObject.Find("Behavior Value");
		string behaviorValue = currentGameObject.GetComponent<Text>().text;

		//create filter
		Filter constructedFilter;

		switch (filterType)
		{
		case "Number Filter":
			constructedFilter = new NumberFilter (field, ParseEnum<NumberFilter.Operator> (op), float.Parse (filterValue));
			break;
		case "String Filter":
			constructedFilter = new StringFilter (field, ParseEnum<StringFilter.Operator> (op), filterValue);
			break;
		case "Range Filter (Number)":
			constructedFilter = new RangeFilterNum (field, ParseEnum<RangeFilterNum.Operator> (op), float.Parse(min), float.Parse (max));
			break;
		case "Range Filter (String)":
			constructedFilter = new RangeFilterString (field, ParseEnum<RangeFilterString.Operator> (op), min, max);
			break;
		default:
			constructedFilter = null;
			break;
		}//end of switch filterType

		if (constructedFilter != null) { 
			GameObject platform = GameObject.Find("Filter Platform");
			GameObject currentFilter = constructedFilter.Build ();
			currentFilter.transform.position = platform.transform.position + Vector3.up;
		}

	}//end of buttonPressed

	public static T ParseEnum<T>(string value){
		return (T)Enum.Parse (typeof(T), value, true);
	}


}
