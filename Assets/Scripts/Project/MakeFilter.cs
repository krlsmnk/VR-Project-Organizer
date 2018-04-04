using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CAVS.ProjectOrganizer.Project
{

    public class MakeFilter : MonoBehaviour
    {
		/// <summary>
		/// make a GameObject reference to assign later
		/// this will be used to get the text values from the vending machine
		/// </summary>
		public GameObject currentGameObject;


        //get the list of all items/nodes
		Item[] nodes = ProjectFactory.BuildItemsFromCSV("CarData.csv");

        //get the filter name from the scene
			currentGameObject = GameObject.Find("Filter Name");
			string filterName = currentGameObject.GetComponent(UI.Text).text;

		//get the filter type from the scene
			currentGameObject = GameObject.Find("Filter Type");
			string filterType = currentGameObject.GetComponent(UI.Text).text;

        //get the filter value from the scene
			currentGameObject = GameObject.Find("Filter Value");
			string filterValue = currentGameObject.GetComponent(UI.Text).text;
            
		//get the field from the scene
			currentGameObject = GameObject.Find("Field");
			string field = currentGameObject.GetComponent(UI.Text).text;

		//get the operator from the scene
			currentGameObject = GameObject.Find("Operator");
			string op = currentGameObject.GetComponent(UI.Text).text;

        //get pass/fail behavior from scene
			currentGameObject = GameObject.Find("Filter Behavior");
			string behavior = currentGameObject.GetComponent(UI.Text).text;
			currentGameObject = GameObject.Find("Behavior Value");
			string behaviorValue = currentGameObject.GetComponent(UI.Text).text;

        //call filterMaker method
        //filterMaker(filterName, nodes, filterType, filterValue, field, op, behavior, behaviorValue);

    }//end of class

	/*
    public void filterMaker(string filterName, Item[] nodes, string filterType, string filterValue, string field, string op, string behavior, string behaviorValue)
    {
        //do stuff based on passed values
        switch (filterType)
        {
            case "Number Filter":
            case "String Filter":
            case "Range Filter (Number)":
            case "Range Filter (String)":
		}//end of switch filterType


        //numberFilter:
            //new NumberFilter(field,  NumberFilter.Operator.op, filterValue)
        //stringFilter:
            //new StringFilter(field, StringFilter.Operator.op, filterValue)

        //create GameObject of new filter

    }
	*/

}//end of namespace




