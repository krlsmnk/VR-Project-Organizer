using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CAVS.ProjectOrganizer.Project.Filtering;
using CAVS.ProjectOrganizer.Project;
using UnityEngine.UI;
using System;
using System.Globalization;

public class GameManager : MonoBehaviour {

	/// <summary>
	/// make a GameObject reference to assign later
	/// this will be used to get the text values from the vending machine
	/// </summary>
	public GameObject currentGameObject;
    public AudioClip badBeep;
    public AudioClip goodBeep;
    public AudioSource audioSource;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
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
		Filter constructedFilter = null;
        float result = 0f;
        float result2 = 0f;
        bool ret = false;
        bool ret2 = false;

		switch (filterType)
		{
		case "Number Filter":
                //try to cast the value as a number
                ret = float.TryParse(filterValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
                //if sucessful, make a filter
                if (ret)
                {
                    constructedFilter = new NumberFilter(field, ParseEnum<NumberFilter.Operator>(op), result);
                }
                else
                {
                    //play sound badBeep @ object's location @ volume 7
                    audioSource.PlayOneShot(badBeep, 7);
                }
			    break;
		case "String Filter":
			    constructedFilter = new StringFilter (field, ParseEnum<StringFilter.Operator> (op), filterValue);
			    break;
		case "Range Filter (Number)":
                //try to cast the ranges as number values
                ret = float.TryParse(min, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
                ret2 = float.TryParse(max, NumberStyles.Any, CultureInfo.InvariantCulture, out result2);
                //if both have been sucessfully converted to numbers, call the filterConstructor
                if (ret && ret2) { 
                    constructedFilter = new RangeFilterNum (field, ParseEnum<RangeFilterNum.Operator> (op), result, result2);
                }
                else
                {
                    //play sound badBeep @ object's location @ volume 7
                    audioSource.PlayOneShot(badBeep, 7);
                }
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

            //play sound goodBeep @ object's location @ volume 7
            audioSource.PlayOneShot(goodBeep, 7);
        }

	}//end of buttonPressed

	public static T ParseEnum<T>(string value){
		return (T)Enum.Parse (typeof(T), value, true);
	}


}
