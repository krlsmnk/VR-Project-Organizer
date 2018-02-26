using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CAVS.ProjectOrganizer.Project;
using CAVS.ProjectOrganizer.Project.ParameterView;

namespace CAVS.ProjectOrganizer.Scenes.Testing.ParameterTesting
{

    public class GameManager : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Item[] allItems = ProjectFactory.BuildItemsFromCSV("CarData.csv");
			ControllerFactory.CreateParameterView(allItems[0].GetValues());
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}