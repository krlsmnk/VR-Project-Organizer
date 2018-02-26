using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CAVS.ProjectOrganizer.Project;
using CAVS.ProjectOrganizer.Project.Aggregations.Plot;

namespace CAVS.ProjectOrganizer.Scenes.Testing.PlottingTesting
{

    public class GameManager : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
			Item[] allItems = ProjectFactory.BuildItemsFromCSV("CarData.csv", 7);
			GameObject plot = new ItemPlot(allItems, "Year", "Length (in)", "Width (in)").Build(Vector3.one*3);
			plot.transform.position = Vector3.up*2;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}