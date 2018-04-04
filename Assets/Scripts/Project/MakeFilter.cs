using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CAVS.ProjectOrganizer.Project
{

    public class MakeFilter : MonoBehaviour
    {

        //get the list of all items/nodes
		Item[] nodes = ProjectFactory.BuildItemsFromCSV("CarData.csv");

        //get the filter type from the scene
            //default: numberFilter
            string filterType = "NumberFilter";

        //get the filter value from the scene
            //defult: 0
            float filterValue = 0.0f;

        //get pass/fail behavior from scene
            //default fail case: Alpha *= 0.3
            string behavior = "Alpha";
            string behaviorValue = "0.3";

        //call filterMaker method
        //filterMaker(nodes, filterType, filterValue, behavior, behaviorValue);

    }//end of class

	/*
    public void filterMaker(Item[] nodes, string filterType, float filterValue, string behavior, string behaviorValue)
    {
        //do stuff based on passed values
        //numberFilter:
            //new NumberFilter("Year",  NumberFilter.Operator.GreaterThan, 1999)
        //stringFilter:
            //new StringFilter("Class", StringFilter.Operator.Equal, "Sport car")

        //create GameObject of new filter

    }
	*/

}//end of namespace




