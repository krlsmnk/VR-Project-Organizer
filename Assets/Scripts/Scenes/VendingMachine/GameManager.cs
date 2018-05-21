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
	private GameObject currentGameObject;
    public Item[] nodes;
    public AudioClip badBeep;
    public AudioClip goodBeep;
    public AudioSource audioSource;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        //GameObject.Find("Filter Name").GetComponentInChildren<Text>().text = "save me";
        //makeFilterButtonPressed();

        //get the list of all items/nodes
        nodes = ProjectFactory.BuildItemsFromCSV("CarData.csv");
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void MakeFilterButtonPressed() {

        //get the filter name from the scene
        currentGameObject = GameObject.Find("Filter Name");
        string filterName = currentGameObject.GetComponentInChildren<Text>().text;

        //get the filter type from the scene
        currentGameObject = GameObject.Find("Filter Type");
        string filterType = currentGameObject.GetComponentInChildren<Text>().text;

        //get the filter value from the scene
        currentGameObject = GameObject.Find("Filter Value");
        string filterValue = currentGameObject.GetComponentInChildren<Text>().text;

        //get the field from the scene
        currentGameObject = GameObject.Find("Field");
        string field = currentGameObject.GetComponentInChildren<Text>().text;

        //get the operator from the scene
        currentGameObject = GameObject.Find("Operator");
        string op = currentGameObject.GetComponentInChildren<Text>().text;

        //get the min and max from the scene
        currentGameObject = GameObject.Find("Min");
        string min = currentGameObject.GetComponentInChildren<Text>().text;
        currentGameObject = GameObject.Find("Max");
        string max = currentGameObject.GetComponentInChildren<Text>().text;

        //get pass/fail behavior from scene
        currentGameObject = GameObject.Find("Filter Behavior");
        string behavior = currentGameObject.GetComponentInChildren<Text>().text;
        currentGameObject = GameObject.Find("Behavior Value");
        string behaviorValue = currentGameObject.GetComponentInChildren<Text>().text;


        MakeFilterOperands(filterName, filterType, filterValue, field, op, min, max, behavior, behaviorValue);

    }//end of pressedButton()

    public void MakeFilterOperands(string filterName, string filterType, string filterValue, string field, string op, string min, string max, string behavior, string behaviorValue)
    {

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
                constructedFilter = new StringFilter(field, ParseEnum<StringFilter.Operator>(op), filterValue);
                break;
            case "Range Filter (Number)":
                //try to cast the ranges as number values
                ret = float.TryParse(min, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
                ret2 = float.TryParse(max, NumberStyles.Any, CultureInfo.InvariantCulture, out result2);
                //if both have been sucessfully converted to numbers, call the filterConstructor
                if (ret && ret2)
                {
                    constructedFilter = new RangeFilterNum(field, ParseEnum<RangeFilterNum.Operator>(op), result, result2);
                }
                else
                {
                    //play sound badBeep @ object's location @ volume 7
                    audioSource.PlayOneShot(badBeep, 7);
                }
                break;
            case "Range Filter (String)":
                constructedFilter = new RangeFilterString(field, ParseEnum<RangeFilterString.Operator>(op), min, max);
                break;
            default:
                constructedFilter = null;
                Debug.Log("DEFAULT REACHED");
                break;
        }//end of switch filterType

        if (constructedFilter != null)
        {
            GameObject platform = GameObject.Find("Filter Platform");
            GameObject currentFilter = constructedFilter.Build();
            currentFilter.transform.position = platform.transform.position + Vector3.up;

            //play sound goodBeep @ object's location @ volume 7
            audioSource.PlayOneShot(goodBeep, 7);
        }

    }//end of makeFilterOperands

    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

}//end of class





