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

        Item[] allItems;

        List<Filter> appliedFilters;

        GameObject currentPalace;

        // Use this for initialization
        void Start()
        {
            appliedFilters = new List<Filter>();
            allItems = ProjectFactory.buildItemsFromCSV("CarData.csv");
            Filter[] filters = new Filter[]{
                new NumberFilter("Year", NumberFilter.Operator.GreaterThan, 1999),
                new NumberFilter("Year", NumberFilter.Operator.LessThan, 2007)
            };
            new ItemSpiral(allItems, filters[0]).BuildPreview(new Vector3(2, 2, 2));
            new ItemSpiral(allItems, filters[1]).BuildPreview(new Vector3(-2, 2, 2));
        }


        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Triggered");
            SprialPreviewBehavior preview = other.gameObject.GetComponent<SprialPreviewBehavior>();
            if (preview != null)
            {
                Debug.Log("With Preview");
                appliedFilters.Add(preview.GetFilter());
                updatePalace();
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
                appliedFilters.Remove(preview.GetFilter());
                updatePalace();
            }
        }

        private void updatePalace()
        {
            if (currentPalace != null)
            {
                Destroy(currentPalace);
            }
            currentPalace = new ItemSpiral(allItems, new AggregateFilter(appliedFilters.ToArray())).BuildPalace();
        }

    }

}