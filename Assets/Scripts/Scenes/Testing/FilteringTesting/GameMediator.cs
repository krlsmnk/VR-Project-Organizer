using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CAVS.ProjectOrganizer.Project;
using CAVS.ProjectOrganizer.Project.Filtering;
using CAVS.ProjectOrganizer.Project.Aggregations.Spiral;

namespace CAVS.ProjectOrganizer.Scenes.Testing.FilteringTesting
{

    public class GameMediator : MonoBehaviour
    {

        protected Item[] allItems;

        protected List<Filter> appliedFilters;

        private GameObject currentPalace;

        // Use this for initialization
        void Start()
        {
            appliedFilters = new List<Filter>();
            allItems = ProjectFactory.BuildItemsFromCSV("CarData.csv", 7);
            Filter[] filters = new Filter[]{
                new NumberFilter("Year",  NumberFilter.Operator.GreaterThan, 1999),
                //new NumberFilter("Year",  NumberFilter.Operator.LessThan, 2007),
                //new StringFilter("Model", StringFilter.Operator.Equal, "ES")
            };
            new ItemSpiral(allItems, filters[0]).BuildPreview(new Vector3(-2, 0, -4));
            //new ItemSpiral(allItems, filters[1]).BuildPreview(new Vector3(-2, 0, -4.2));
            //new ItemSpiral(allItems, filters[2]).BuildPreview(-2, 0, -4.4);
        }


        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            SprialPreviewBehavior preview = other.gameObject.GetComponent<SprialPreviewBehavior>();
            if (preview != null)
            {
                preview.gameObject.GetComponent<MeshRenderer>().material.color = Color.green * .5f;
                appliedFilters.Add(preview.GetFilter());
                displayPalace();
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            SprialPreviewBehavior preview = other.gameObject.GetComponent<SprialPreviewBehavior>();
            if (preview != null)
            {
                preview.gameObject.GetComponent<MeshRenderer>().material.color = new Color(54.0f / 255.0f, 172.0f / 255.0f, 1, 100.0f / 255.0f);
                appliedFilters.Remove(preview.GetFilter());
                displayPalace();
            }
        }

        protected virtual void displayPalace()
        {
            if (currentPalace != null)
            {
                Destroy(currentPalace);
            }
            currentPalace = new ItemSpiral(allItems, appliedFilters.ToArray()).BuildPalace();
        }

    }

}